using UnityEngine;

public class TargetingAIBasic : MonoBehaviour
{
    [HideInInspector] public Transform currentTargetTransform;
    [HideInInspector] public bool isAttacking;

    [SerializeField] private float searchRadius;
    [SerializeField] private float breakDistance;

    private float lookForTargetTimeCounter;
    private float lookForTargetTimeMin = 0.5f;
    private float lookForTargetTimeMax = 1.5f;

    //===========================================================================
    private void FixedUpdate()
    {
        HandleTargeting();
    }

    //===========================================================================
    private void HandleTargeting()
    {
        if (currentTargetTransform)
        {
            if (Vector2.Distance(currentTargetTransform.position, transform.position) >= breakDistance ||
                currentTargetTransform.gameObject.activeSelf == false)
            {
                currentTargetTransform = null;
                return;
            }
        }

        if (currentTargetTransform == null)
        {
            LookForTarget();
            return;
        }

        lookForTargetTimeCounter -= Time.deltaTime;
        if (lookForTargetTimeCounter <= 0.0f)
        {
            lookForTargetTimeCounter += UnityEngine.Random.Range(lookForTargetTimeMin, lookForTargetTimeMax);
            LookForTarget();
        }
    }

    private void LookForTarget()
    {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, searchRadius);
        foreach (Collider2D collider2D in collider2DArray)
        {
            if (collider2D.GetComponent<Player>() != null)
            {
                currentTargetTransform = collider2D.transform;
                return;
            }
        }
    }
}