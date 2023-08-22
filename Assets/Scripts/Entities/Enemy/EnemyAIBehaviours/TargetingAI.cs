using UnityEngine;

public class TargetingAI : MonoBehaviour
{
    [HideInInspector] public bool isAttacking;

    [SerializeField] public Transform targetTransform;
    public Vector3 targetPosition;

    [SerializeField] private float searchRadius;
    [SerializeField] private float breakDistanceMin;
    [SerializeField] private float breakDistanceMax;

    [SerializeField] private bool keepDistance;
    [SerializeField] private bool patrolArea;
    [SerializeField] GameObject patrolTransforms;

    private float targetDistance;
    private Vector3 targetDir;
    private bool holdMovement;
    private float holdTimer = 0.5f;
    private float patrolTime = 3f;
    private float patrolTimeCounter;

    private void Start()
    {
        targetPosition = transform.position;
        targetTransform.position = transform.position;
    }

    //===========================================================================
    private void FixedUpdate()
    {
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
        if (patrolArea)
        {
            patrolTimeCounter -= Time.deltaTime;
            if (patrolTimeCounter <= 0.0f)
            {
                int index = Random.Range(0, patrolTransforms.transform.childCount);
                targetPosition = patrolTransforms.transform.GetChild(index).transform.position;
                patrolTimeCounter = patrolTime;
            }
        }
        else
        {
            LookForTarget();
        }
    }
    private void UpdateTargetTransform()
    {
        if (targetDistance >= searchRadius)
        {
            ClearTarget();
        }
        else if(keepDistance && Mathf.Abs(targetDistance) < breakDistanceMax && Mathf.Abs(targetDistance) > breakDistanceMin)
        {
            targetTransform.position = transform.position;
        }
        else if (keepDistance && Mathf.Abs(targetDistance) < breakDistanceMin)
        {
            Vector3 newPosition;
            newPosition.x = targetPosition.x + (-targetDir.x * 10);
            newPosition.y = targetPosition.y + (-targetDir.y * 10);
            newPosition.z = 0;
            targetTransform.position = newPosition;
        }
        else
        {
            targetTransform.position = targetPosition;
        }
    }

    private void LookForTarget()
    {
        ClearTarget();
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, searchRadius);
        foreach (Collider2D collider2D in collider2DArray)
        {
            if (collider2D.gameObject.CompareTag("Player"))
            {
                targetPosition = collider2D.transform.position;
                targetDistance = Vector2.Distance(targetPosition, transform.position);
                targetDir = (targetPosition - GetComponent<Transform>().position).normalized;
                
                UpdateTargetTransform();
                break;
            }
        }
    }
    public void ClearTarget()
    {
        targetPosition = transform.position;
        targetTransform.position = transform.position;
    }
    public bool CheckNoTarget()
    {
        if(targetPosition == transform.position)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void HoldMovement()
    {
        holdMovement = true;
    }
}
