using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public static event UnityAction<Transform> OnPlayerReady;

    private float moveSpeed = default;

    private Rigidbody2D Rigidbody2D;
    private Vector2 movementVector;
    private bool canMove = true;

    private float impairTimer = default;

    [Header("Dash")]
    private float dashPenalty = default;
    private float dashCooldown = default;
    private float dashTime = default;
    private float dashSpeed = default;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;

    private Vector3 toMouseDirectionVector = default;
    private Vector2 dashVector;
    private float dashTimeCounter;
    private float dashCooldownTimer;

    public float DashCDTimeCounter => dashCooldownTimer;

    // Components
    private CapsuleCollider2D CapsuleCollider2D;

    //===========================================================================
    // NEW INPUT SYSTEM
    private PlayerInput playerInput;

    //===========================================================================
    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();

        Rigidbody2D = GetComponent<Rigidbody2D>();
        CapsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        if (SceneControlManager.Instance.GameState == GameState.PauseMenu ||
            SceneControlManager.Instance.GameState == GameState.OptionMenu ||
            SceneControlManager.Instance.GameState == GameState.Dialogue)
            return;

        PlayerInput();

        UpdateAnimator();

        DashCoolDownTimeCounter();

        DashHandler();

        UpdateImpairTimer();

        if (canMove == false)
            movementVector = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (SceneControlManager.Instance.GameState == GameState.PauseMenu ||
            SceneControlManager.Instance.GameState == GameState.OptionMenu ||
            SceneControlManager.Instance.GameState == GameState.Dialogue)
            return;

        MovePlayerPosition();
    }

    //======================================================================
    private void PlayerInput()
    {
        if (Player.Instance.actionState == PlayerActionState.IsDashing)
            return;

        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();

        movementVector.x = input.x;
        movementVector.y = input.y;
        movementVector = movementVector.normalized;
    }

    private void UpdateAnimator()
    {
        toMouseDirectionVector = (CultyMarbleHelper.GetMouseToWorldPosition() - transform.position).normalized;
        Vector2 direction;

        switch (Player.Instance.actionState)
        {
            case PlayerActionState.IsUsingBasicAbility:
            case PlayerActionState.IsUsingRangeAbility:
                direction = toMouseDirectionVector;
                break;
            default:
                direction = movementVector.normalized;
                break;
        }

        if (direction.x > 0.5)
        {
            animator.SetBool("IsWalkingRight", true);
            animator.SetBool("IsIdle", false);
            playerSpriteRenderer.flipX = false;
        }
        else if (direction.x < -0.5)
        {
            animator.SetBool("IsWalkingRight", true);
            animator.SetBool("IsIdle", false);
            playerSpriteRenderer.flipX = true;
        }
        if (direction.y < -0.5)
        {
            animator.SetBool("IsWalkingDown", true);
            animator.SetBool("IsWalkingUp", false);
            animator.SetBool("IsIdle", false);
        }
        else if (direction.y > 0.5)
        {
            animator.SetBool("IsWalkingUp", true);
            animator.SetBool("IsWalkingDown", false);
            animator.SetBool("IsIdle", false);
        }

        if (Mathf.Abs(direction.y) <= 0.5 && Mathf.Abs(direction.x) <= 0.5)
        {
            animator.SetBool("IsWalkingRight", false);
            animator.SetBool("IsWalkingDown", false);
            animator.SetBool("IsWalkingUp", false);
            animator.SetBool("IsIdle", true);
        }
        else if (direction.y == 0)
        {
            animator.SetBool("IsWalkingDown", false);
            animator.SetBool("IsWalkingUp", false);
        }
        else if (direction.x == 0)
        {
            animator.SetBool("IsWalkingRight", false);
        }
        if (Mathf.Abs(movementVector.y) <= 0.5 && Mathf.Abs(movementVector.x) <= 0.5)
        {
            animator.SetBool("IsIdle", true);
        }
    }

    private void MovePlayerPosition()
    {
        switch (Player.Instance.actionState)
        {
            case PlayerActionState.IsDashing:
                //Rigidbody2D.MovePosition(Rigidbody2D.position + dashSpeed * Time.deltaTime * dashVector);
                Rigidbody2D.MovePosition(Rigidbody2D.position + (dashSpeed / (dashTime / dashTimeCounter)) * Time.deltaTime * dashVector);
                break;
            case PlayerActionState.IsUsingBasicAbility:
                Rigidbody2D.MovePosition(Rigidbody2D.position + (moveSpeed * PlayerDataManager.Instance.PlayerDataRuntime.Ba_basePlayerSpeedPenalty) * Time.deltaTime * movementVector);
                break;
            case PlayerActionState.IsUsingRangeAbility:
                Rigidbody2D.MovePosition(Rigidbody2D.position + moveSpeed * Time.deltaTime * movementVector);
                break;
            default:
                Rigidbody2D.MovePosition(Rigidbody2D.position + moveSpeed * Time.deltaTime * movementVector);
                break;
        }
        Debug.Log(movementVector.x);
    }

    private void DashCoolDownTimeCounter()
    {
        if (dashCooldownTimer <= 0)
            return;

        dashCooldownTimer -= Time.deltaTime;
    }

    private void DashHandler()
    {
        // Trigger Dash
        if (playerInput.actions["Dash"].triggered)
        {
            if (dashCooldownTimer <= 0 && movementVector != Vector2.zero)
            {
                Player.Instance.actionState = PlayerActionState.IsDashing;
                impairTimer = 0;
                UpdateImpairTimer();
                dashTimeCounter = dashTime;
                dashCooldownTimer = dashCooldown;
                dashVector = movementVector;

                // Player Collision
                CapsuleCollider2D.enabled = !CapsuleCollider2D.enabled;
            }
        }

        if (Player.Instance.actionState == PlayerActionState.IsDashing)
        {
            dashTimeCounter -= Time.deltaTime;
            if (dashTimeCounter <= 0)
            {
                Player.Instance.actionState = PlayerActionState.none;

                //SetImpairDuration(dashPenalty);

                // Player Collision
                CapsuleCollider2D.enabled = !CapsuleCollider2D.enabled;
            }
        }
    }

    private void UpdateImpairTimer()
    {
        //if (impairTimer <= 0)
        //    return;

        impairTimer -= Time.deltaTime;
        if (impairTimer <= 0)
        {
            canMove = true;
        }
    }

    //======================================================================
    public void UpdateMovementParameters()
    {
        moveSpeed = PlayerDataManager.Instance.PlayerDataRuntime.BaseMoveSpeed;

        dashPenalty = PlayerDataManager.Instance.PlayerDataRuntime.BaseDashPenalty;
        dashCooldown = PlayerDataManager.Instance.PlayerDataRuntime.BaseDashCooldown;
        dashTime = PlayerDataManager.Instance.PlayerDataRuntime.BaseDashTime;
        dashSpeed = PlayerDataManager.Instance.PlayerDataRuntime.BaseDashSpeed;

        OnPlayerReady?.Invoke(transform);
    }

    public void SetImpairDuration(float impairDuration)
    {
        impairTimer = impairDuration;
        dashVector = Vector2.zero;
        canMove = false;
    }

    public void UpdateDashTime(float duration)
    {
        dashTime += duration;
    }

    public void SetMoveSpeed(float newMoveSpeed)
    {
        moveSpeed = newMoveSpeed;
    }

    public void SetDashParameter(float newDashTime, float newDashSpeed)
    {
        dashTime = newDashTime;
        dashSpeed = newDashSpeed;
    }

    public void IncreaseDashParameter(float newDashTime, float newDashSpeed)
    {
        dashTime += newDashTime;
        dashSpeed += newDashSpeed;
    }
}