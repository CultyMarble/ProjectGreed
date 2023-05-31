using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Basic Movement")]
    [SerializeField] private float baseMoveSpeed;

    private Rigidbody2D Rigidbody2D;
    private Vector2 movementVector;
    private bool canMove = true;

    [Header("Dash")]
    [SerializeField] public float dashCD = 3.0f;
    [SerializeField] private float pauseTimeAfterDash = 1.0f;
    [SerializeField] private float dashTime = 0.1f;
    [SerializeField] private float dashSpeed = 100.0f;

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

    private void PlayerMovePosition()
    {
        switch (Player.Instance.playerActionState)
        {
            case PlayerActionState.IsDashing:
                Rigidbody2D.MovePosition(Rigidbody2D.position +
                dashSpeed * Time.deltaTime * dashVector);
                break;
            case PlayerActionState.IsUsingBasicAbility:
                Rigidbody2D.MovePosition(Rigidbody2D.position +
                (0.1f * baseMoveSpeed) * Time.deltaTime * movementVector);
                break;
            case PlayerActionState.IsUsingRangeAbility:
                break;
            case PlayerActionState.IsUsingBombAbility:
                break;
            default:
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
            if (dashCDTimeCounter <= 0  && movementVector != Vector2.zero)
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