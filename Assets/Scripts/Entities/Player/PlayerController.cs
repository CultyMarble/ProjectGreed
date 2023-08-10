using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Basic Movement")]
    [SerializeField] private float baseMoveSpeed;

    private Rigidbody2D Rigidbody2D;
    private Vector2 movementVector;
    private bool canMove = true;

    private float impairDurationTimer = default;

    [Header("Dash")]
    private readonly float dashSpeed = 20.0f;
    public float dashCD = default;
    [SerializeField] private float impairAfterDashDuration = default;
    [SerializeField] private float dashTime = default;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;

    private Vector3 toMouseDirectionVector = default;
    private Vector2 dashVector;
    private float dashTimeCounter;
    private float dashCDTimeCounter;

    public float DashCDTimeCounter => dashCDTimeCounter;

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

    private void FixedUpdate()
    {
        MovePlayerPosition();
    }

    private void Update()
    {
        PlayerInput();

        UpdateAnimator();

        DashCoolDownTimeCounter();

        DashHandler();

        PauseAfterDash();

        if (canMove == false)
            movementVector = Vector2.zero;
    }

    //======================================================================
    private void PlayerInput()
    {
        if (Player.Instance.playerActionState == PlayerActionState.IsDashing)
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

        switch (Player.Instance.playerActionState)
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
        switch (Player.Instance.playerActionState)
        {
            case PlayerActionState.IsDashing:
                Rigidbody2D.MovePosition(Rigidbody2D.position + dashSpeed * Time.deltaTime * dashVector);
                break;
            case PlayerActionState.IsUsingBasicAbility:
                Rigidbody2D.MovePosition(Rigidbody2D.position + (0.1f * baseMoveSpeed) * Time.deltaTime * movementVector);
                break;
            case PlayerActionState.IsUsingRangeAbility:
                Rigidbody2D.MovePosition(Rigidbody2D.position + baseMoveSpeed * Time.deltaTime * movementVector);
                break;
            case PlayerActionState.IsUsingBombAbility:
                break;
            default:
                Rigidbody2D.MovePosition(Rigidbody2D.position + baseMoveSpeed * Time.deltaTime * movementVector);
                break;
        }
    }

    private void DashCoolDownTimeCounter()
    {
        if (dashCDTimeCounter <= 0)
            return;

        dashCDTimeCounter -= Time.deltaTime;
    }

    private void DashHandler()
    {
        // Trigger Dash
        if (playerInput.actions["Dash"].triggered && Player.Instance.playerActionState == PlayerActionState.none)
        {
            if (dashCDTimeCounter <= 0 && movementVector != Vector2.zero)
            {
                Player.Instance.playerActionState = PlayerActionState.IsDashing;
                dashTimeCounter = dashTime;
                dashCDTimeCounter = dashCD;
                dashVector = movementVector;

                // Player Collision
                CapsuleCollider2D.enabled = !CapsuleCollider2D.enabled;
            }
        }

        if (Player.Instance.playerActionState == PlayerActionState.IsDashing)
        {
            dashTimeCounter -= Time.deltaTime;
            if (dashTimeCounter <= 0)
            {
                Player.Instance.playerActionState = PlayerActionState.none;

                SetImpairDuration(impairAfterDashDuration);

                // Player Collision
                CapsuleCollider2D.enabled = !CapsuleCollider2D.enabled;
            }
        }
    }

    private void PauseAfterDash()
    {
        if (impairDurationTimer <= 0)
            return;

        impairDurationTimer -= Time.deltaTime;
        if (impairDurationTimer <= 0)
        {
            canMove = true;
        }
    }

    //======================================================================
    public void SetImpairDuration(float impairDuration)
    {
        impairDurationTimer = impairDuration;
        dashVector = Vector2.zero;
        canMove = false;
    }

    public void UpdateDashTime(float duration)
    {
        dashTime += duration;
    }

    public void SetMoveSpeed(float newMoveSpeed)
    {
        baseMoveSpeed = newMoveSpeed;
    }
}