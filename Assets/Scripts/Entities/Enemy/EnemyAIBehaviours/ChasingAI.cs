using UnityEngine;

[RequireComponent(typeof(TargetingAI))]
public class ChasingAI : MonoBehaviour
{
    [HideInInspector] public bool holdMovementDirection = false;
    [HideInInspector] public float holdtimer;

    [SerializeField] private float speed;
    [SerializeField] private float distanceToKeep;

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
        if (holdMovementDirection == true)
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
            if (enemy_rb2D.velocity != Vector2.zero)
                enemy_rb2D.velocity = Vector2.zero;

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
    public void SetMovementSpeed(float modifier)
    {
        speed = speed * (1 + (modifier * 0.01f));
    }
}