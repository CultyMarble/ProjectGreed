using UnityEngine;

public class TargetingAI : MonoBehaviour
{
    [HideInInspector] public bool isAttacking;

    [SerializeField] public Transform currentTargetTransform;

    //[SerializeField] private bool keepDistance;

    [SerializeField] private float searchRadius;
    [SerializeField] private float breakDistance;

    private float lookForTargetTimeCounter;
    private float lookForTargetTimeMin = 0.5f;
    private float lookForTargetTimeMax = 1.5f;
    private float targetDistance;
    private float targetDir;

    private void Start()
    {
        currentTargetTransform.position = transform.position;
    }

    //===========================================================================
    private void FixedUpdate()
    {
        HandleTargeting();
    }

    //===========================================================================
    private void HandleTargeting()
    {
        if(currentTargetTransform.position != transform.position)
        {
            targetDistance = Vector2.Distance(currentTargetTransform.position, transform.position);
            //targetDir = (currentTargetTransform.position - GetComponent<Transform>().position).normalized;
            //targetDir = Mathf.Atan(GetComponent<Transform>().position.y - transform.position.y / GetComponent<Transform>().position.x - transform.position.x);
        }
        if (currentTargetTransform.position != transform.position && targetDistance >= breakDistance)
        {
            ClearTarget();
        }
        //if (keepDistance && currentTargetTransform.position != transform.position && targetDistance <= breakDistance)
        //{
        //    //currentTargetTransform.Translate(Mathf.Abs(breakDistance - targetDistance) * -targetDir);
        //    float newPosX = currentTargetTransform.position.x - (breakDistance + 0.2f * ((transform.position.x - currentTargetTransform.position.x) / Mathf.Sqrt(Mathf.Pow(transform.position.x - currentTargetTransform.position.x, 2) + Mathf.Pow(transform.position.y - currentTargetTransform.position.y, 2))));
        //    float newPosY = currentTargetTransform.position.y - (breakDistance + 0.2f * ((transform.position.y - currentTargetTransform.position.y) / Mathf.Sqrt(Mathf.Pow(transform.position.x - currentTargetTransform.position.x, 2) + Mathf.Pow(transform.position.y - currentTargetTransform.position.y, 2))));

        //    float newDistance = Vector2.Distance(currentTargetTransform.position, transform.position);

        //    currentTargetTransform.position.Set(newPosX,newPosY, 0);

        //    //currentTargetTransform.position = transform.position + breakDistance * ((transform.position - currentTargetTransform.position) * (transform.position - currentTargetTransform.position).normalized);
        //    //float rise = currentTargetTransform.position.y - transform.position.y;
        //    //float run = currentTargetTransform.position.x - transform.position.x;
        //    //float slope = rise / run;
        //}
        if (currentTargetTransform.position != transform.position && currentTargetTransform.gameObject.activeSelf == false)
        {
            ClearTarget();
        }

        if (currentTargetTransform.position == transform.position)
        {
            LookForTarget();
        }
        else
        {
            lookForTargetTimeCounter -= Time.deltaTime;
            if (lookForTargetTimeCounter <= 0.0f)
            {
                lookForTargetTimeCounter += UnityEngine.Random.Range(lookForTargetTimeMin, lookForTargetTimeMax);
                LookForTarget();
            }
        }

    }

    private void LookForTarget()
    {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, searchRadius);
        foreach (Collider2D collider2D in collider2DArray)
        {
            if (collider2D.GetComponent<Player>() != null)
            {
                currentTargetTransform.position = collider2D.transform.position;
                return;
            }
        }
    }
    public void ClearTarget()
    {
        currentTargetTransform.position = transform.position;
    }
}
