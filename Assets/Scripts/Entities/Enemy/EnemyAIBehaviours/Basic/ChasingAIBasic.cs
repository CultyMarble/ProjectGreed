using UnityEngine;

[RequireComponent(typeof(TargetingAIBasic))]
public class ChasingAIBasic : MonoBehaviour
{
    [HideInInspector] public bool holdMovementDirection = false;
    [HideInInspector] public float holdtimer;

    [SerializeField] private float speed;
    [SerializeField] private float distanceToKeep;

    [HideInInspector] public Rigidbody2D enemy_rb2D;
    private TargetingAIBasic targetingAIBasic;
    private Vector2 movingDirection;
    private float currentSpeed;

    //===========================================================================
    private void Awake()
    {
        enemy_rb2D = GetComponent<Rigidbody2D>();
        targetingAIBasic = GetComponent<TargetingAIBasic>();
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
        if (targetingAIBasic.currentTargetTransform == null)
        {
            if (enemy_rb2D.velocity != Vector2.zero)
                enemy_rb2D.velocity = Vector2.zero;

            return;
        }

        movingDirection = (targetingAIBasic.currentTargetTransform.transform.position - transform.position).normalized;

        if (Vector2.Distance(targetingAIBasic.currentTargetTransform.position, transform.position) <= distanceToKeep)
            currentSpeed = 0;
        else
            currentSpeed = speed;

        enemy_rb2D.velocity = movingDirection * currentSpeed;
    }
}