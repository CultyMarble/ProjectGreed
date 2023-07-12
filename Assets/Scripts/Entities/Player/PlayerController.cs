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

    [Header("Dash")]
    public float dashCD = 3.0f;
    [SerializeField] private float pauseTimeAfterDash = 1.0f;
    [SerializeField] private float dashTime = 0.1f;
    [SerializeField] private float dashSpeed = 100.0f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;

    private Vector3 toMouseDirectionVector = default;
    private Vector2 dashVector;
    private float dashTimeCounter;
    private float dashCDTimeCounter;
    private float pauseTimeAfterDashCounter;

    // Components
    private CapsuleCollider2D CapsuleCollider2D;

    //======================================================================
    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        CapsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void FixedUpdate()
    {
        PlayerMovePosition();
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
        if (Player.Instance.playerActionState == PlayerActionState.IsDashing || Player.Instance.playerActionState == PlayerActionState.IsUsingRangeAbility)
            return;

        movementVector.x = Input.GetAxisRaw("Horizontal");
        movementVector.y = Input.GetAxisRaw("Vertical");
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

    private void PlayerMovePosition()
    {
        switch (Player.Instance.playerActionState)
        {
            case PlayerActionState.IsDashing:
                GetComponentInParent<TrailRenderer>().enabled = true;
                Rigidbody2D.MovePosition(Rigidbody2D.position +
                dashSpeed * Time.deltaTime * dashVector);
                break;
            case PlayerActionState.IsUsingBasicAbility:
                GetComponentInParent<TrailRenderer>().enabled = false;
                Rigidbody2D.MovePosition(Rigidbody2D.position +
                (0.1f * baseMoveSpeed) * Time.deltaTime * movementVector);
                break;
            case PlayerActionState.IsUsingRangeAbility:
                GetComponentInParent<TrailRenderer>().enabled = false;
                break;
            case PlayerActionState.IsUsingBombAbility:
                GetComponentInParent<TrailRenderer>().enabled = false;
                break;
            default:
                GetComponentInParent<TrailRenderer>().enabled = false;
                Rigidbody2D.MovePosition(Rigidbody2D.position +
                baseMoveSpeed * Time.deltaTime * movementVector);
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
        if (Input.GetKeyDown(KeyCode.Space) && Player.Instance.playerActionState == PlayerActionState.none)
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
                pauseTimeAfterDashCounter = pauseTimeAfterDash;
                dashVector = Vector2.zero;
                canMove = false;

                // Player Collision
                CapsuleCollider2D.enabled = !CapsuleCollider2D.enabled;
            }
        }
    }

    private void PauseAfterDash()
    {
        if (pauseTimeAfterDashCounter <= 0)
            return;

        pauseTimeAfterDashCounter -= Time.deltaTime;
        if (pauseTimeAfterDashCounter <= 0)
        {
            canMove = true;
        }
    }

    //======================================================================
    public float GetDashCDCounter()
    {
        return dashCDTimeCounter;
    }
}