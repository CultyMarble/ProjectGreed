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
    private bool isDashing;
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
        if (isDashing)
            return;

        movementVector.x = Input.GetAxisRaw("Horizontal");
        movementVector.y = Input.GetAxisRaw("Vertical");
        movementVector = movementVector.normalized;
    }

    private void PlayerMovePosition()
    {
        if (isDashing)
        {
            Rigidbody2D.MovePosition(Rigidbody2D.position +
                dashSpeed * Time.deltaTime * dashVector);
        }
        else
        {
            Rigidbody2D.MovePosition(Rigidbody2D.position +
                baseMoveSpeed * Time.deltaTime * movementVector);
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
        if (Input.GetKeyDown(KeyCode.Space) && isDashing == false)
        {
            if (dashCDTimeCounter <= 0  && movementVector != Vector2.zero)
            {
                isDashing = true;
                dashTimeCounter = dashTime;
                dashCDTimeCounter = dashCD;
                dashVector = movementVector;

                // Player Collision
                CapsuleCollider2D.enabled = !CapsuleCollider2D.enabled;
            }
        }

        if (isDashing)
        {
            dashTimeCounter -= Time.deltaTime;
            if (dashTimeCounter <= 0)
            {
                isDashing = false;
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
}