using UnityEngine;

[RequireComponent(typeof(TargetingAI))]
public class ChasingAI : MonoBehaviour
{
    [HideInInspector] public bool holdMovementDirection = false;
    [HideInInspector] public float holdtimer;

    [SerializeField] private float speed;
    [SerializeField] private float distanceToKeep;

    private bool stopMovement = false;
    private float stopMovementTimer = default;

    private Rigidbody2D enemy_rb2D;
    private TargetingAI targetingAI;
    private Vector2 movingDirection;
    private float currentSpeed;
    //===========================================================================
    private void Awake()
    {
        enemy_rb2D = GetComponent<Rigidbody2D>();
        targetingAI = GetComponent<TargetingAI>();
    }

    private void FixedUpdate()
    {
        if (stopMovement)
        {
            stopMovementTimer -= Time.deltaTime;
            if (stopMovementTimer <= 0.0f)
            {
                currentSpeed = speed;
                stopMovement = false;
            }
        }
        else if (holdMovementDirection == true)
        {
            holdtimer -= Time.deltaTime;
            if (holdtimer <= 0.0f)
            {
                holdMovementDirection = false;
            }
        }
        else
        {
            MoveTowardCurrentTarget();
        }
    }

    //===========================================================================
    private void MoveTowardCurrentTarget()
    {
        if (targetingAI.currentTargetTransform == null)
        {
            movingDirection = Vector2.zero;
            return;
        }

        movingDirection = (targetingAI.currentTargetTransform.transform.position - transform.position).normalized;

        if (Vector2.Distance(targetingAI.currentTargetTransform.position, transform.position) <= distanceToKeep)
            currentSpeed = 0;
        else
            currentSpeed = speed;

        enemy_rb2D.velocity = movingDirection * currentSpeed;
    }

    //===========================================================================
    public void StopMovement(float newTime)
    {
        stopMovement = true;
        currentSpeed = 0.0f;

        stopMovementTimer = newTime;
        enemy_rb2D.velocity = movingDirection * currentSpeed;
    }
}