using UnityEngine;

public class TargetingAI : MonoBehaviour
{
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isLunging;
    [HideInInspector] public bool isCharging;
    [SerializeField] public int movementSpeed;

    [SerializeField] public Transform currentDestination;
    [HideInInspector] public Vector3 currentTarget;

    [SerializeField] private float searchRadius;
    [SerializeField] private float breakDistanceMin;
    [SerializeField] private float breakDistanceMax;
    [SerializeField] private bool requireLineOfSight = false;
    [SerializeField] private bool keepDistance;
    [SerializeField] private bool patrolArea;
    [SerializeField] private bool flee;

    [SerializeField] GameObject patrolTransforms;

    private float targetDistance;
    private Vector3 targetDir;
    private bool holdMovement;
    private float holdTimer = 0.5f;
    private readonly float patrolTime = 3f;
    private float patrolTimeCounter;
    private Vector3 lastKnownPosition;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public bool dontUpdateDestination = false;
    public Pathfinding.AIPath pathfinder;

    private void Start()
    {
        currentTarget = Vector3.zero;
        currentDestination.position = transform.position;
        lastKnownPosition = transform.position;
        pathfinder = GetComponent<Pathfinding.AIPath>();
        animator = GetComponent<Animator>();
        spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
    }

    //===========================================================================
    private void FixedUpdate()
    {
        if (SceneControlManager.Instance.CurrentGameplayState == GameplayState.Pause)
        {
            pathfinder.maxSpeed = 0;
            return;
        }
        else if (!isCharging && !isLunging)
        {
            pathfinder.maxSpeed = movementSpeed;
        }
        if (!CheckNoTarget())
        {
            if (Vector3.Distance(transform.position,currentDestination.position) < 0.1f)
            {
                animator.SetBool("isIdle", true);
            }
            else if (currentDestination.position.x > transform.position.x)
            {
                animator.SetBool("isWalkingRight", true);
                animator.SetBool("isWalkingLeft", false);
                animator.SetBool("isIdle", false);
                spriteRenderer.flipX = true;
            }
            else if (currentDestination.position.x < transform.position.x)
            {
                animator.SetBool("isWalkingLeft", true);
                animator.SetBool("isWalkingRight", false);
                animator.SetBool("isIdle", false);
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            animator.SetBool("isWalkingLeft", false);
            animator.SetBool("isWalkingRight", false);
            animator.SetBool("isIdle", true);
        }
        if (!holdMovement)
        {
            HandleTargeting();
        }
        else
        {
            ClearTarget();
            holdTimer -= Time.deltaTime;
            if(holdTimer <= 0)
            {
                holdMovement = false;
            }
        }
    }

    //===========================================================================
    private void HandleTargeting()
    {
        if (dontUpdateDestination)
        {
            currentDestination.position = lastKnownPosition;
            return;
        }
        LookForTarget();
    }
    private void UpdateTargetTransform()
    {
        if (!patrolArea && targetDistance >= searchRadius)
        {
            ClearTarget();
        }
        else if (flee && Mathf.Abs(targetDistance) < breakDistanceMin)
        {
            Vector3 newPosition;
            newPosition.x = currentTarget.x + (-targetDir.x * breakDistanceMin);
            newPosition.y = currentTarget.y + (-targetDir.y * breakDistanceMin);
            newPosition.z = 0;
            currentDestination.position = newPosition;
        }
        else if(flee && Mathf.Abs(targetDistance) > breakDistanceMin)
        {
            currentDestination.position = transform.position;
            return;
        }
        else if(keepDistance && Mathf.Abs(targetDistance) < breakDistanceMax && Mathf.Abs(targetDistance) > breakDistanceMin)
        {
            currentDestination.position = transform.position;
        }
        else if (keepDistance && Mathf.Abs(targetDistance) < breakDistanceMin)
        {
            Vector3 newPosition;
            newPosition.x = currentTarget.x + (-targetDir.x * breakDistanceMin);
            newPosition.y = currentTarget.y + (-targetDir.y * breakDistanceMin);
            newPosition.z = 0;
            currentDestination.position = newPosition;
        }
        else if (patrolArea)
        {
            patrolTimeCounter -= Time.deltaTime;
            if (patrolTimeCounter <= 0.0f)
            {
                int index = Random.Range(0, patrolTransforms.transform.childCount);
                currentDestination.position = patrolTransforms.transform.GetChild(index).transform.position;
                patrolTimeCounter = patrolTime;
            }
        }
        else
        {
            currentDestination.position = currentTarget;
        }
    }

    private void LookForTarget()
    {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, searchRadius);
        foreach (Collider2D collider2D in collider2DArray)
        {
            if (collider2D.gameObject.CompareTag("Player"))
            {
                currentTarget = collider2D.transform.position;
                targetDistance = Vector2.Distance(currentTarget, transform.position);
                targetDir = (currentTarget - transform.position).normalized;
                if (!requireLineOfSight)
                {
                    UpdateTargetTransform();
                    break;
                }
                else if (CheckLineOfSight())
                {
                    UpdateTargetTransform();
                    lastKnownPosition = currentTarget;
                }
                else if (lastKnownPosition != Vector3.zero)
                {
                    currentDestination.position = lastKnownPosition;
                }
                break;
            }
        }
    }
    public bool CheckLineOfSight()
    {
        if (!requireLineOfSight)
        {
            return true;
        }
        RaycastHit2D hit;
        LayerMask playerMask = LayerMask.GetMask("Player");
        LayerMask collisionMask = LayerMask.GetMask("Obstacles");
        LayerMask mask = playerMask | collisionMask;

        hit = Physics2D.Raycast(transform.position, targetDir, searchRadius, mask, 0, 0);
        
        if(hit.collider == null)
        {
            return false;
        }
        Debug.DrawRay(transform.position, (targetDir * searchRadius) - (targetDir * Vector2.Distance(currentTarget, hit.collider.transform.position)), Color.red);
        if (hit.collider.gameObject.CompareTag("Player"))
        {
            return true;
        }
        return false;
    }
    public void ClearTarget()
    {
        //currentTarget = transform.position;
        currentDestination.position = transform.position;
        lastKnownPosition = Vector3.zero;
    }
    public bool CheckNoTarget()
    {
        if(Vector2.Distance(currentDestination.position,transform.position) < 0.1f && !CheckLineOfSight())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void DontUpdateDestination(bool input)
    {
        if(input == true)
        {
            dontUpdateDestination = input;
            lastKnownPosition = currentDestination.position;
        }
        else
        {
            dontUpdateDestination = input;
            LookForTarget();
        }
    }
    public void TogglePatrol(bool toggle)
    {
        patrolArea = toggle;
    }
    public void HoldMovement(bool input)
    {
        holdMovement = input;
    }
    private void OnDisable()
    {
        isLunging = false;
        isCharging = false;
        isAttacking = false;
    }
}
