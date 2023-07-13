using UnityEngine;

public class TargetingAI : MonoBehaviour
{
    [HideInInspector] public bool isAttacking;

    [SerializeField] public Transform currentTargetTransform;

    //[SerializeField] private bool keepDistance;

    [SerializeField] private float searchRadius;
    [SerializeField] private float breakDistance;
    [SerializeField] private bool keepDistance;
    [SerializeField] private bool patrolArea;
    [SerializeField] GameObject patrolTransforms;


    private float lookForTargetTimeCounter;
    private float lookForTargetTimeMin = 0.5f;
    private float lookForTargetTimeMax = 1.5f;
    private float targetDistance;
    private Vector3 targetDir;
    private bool holdMovement;
    private float holdTimer = 0.5f;
    private float patrolTime = 3f;
    private float patrolTimeCounter;


    private void Start()
    {
        currentTargetTransform.position = transform.position;
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
        if(currentTargetTransform.position != transform.position)
        {
            targetDistance = Vector2.Distance(currentTargetTransform.position, transform.position);
            targetDir = (currentTargetTransform.position - GetComponent<Transform>().position).normalized;
            //targetDir = Mathf.Atan(GetComponent<Transform>().position.y - transform.position.y / GetComponent<Transform>().position.x - transform.position.x);
        }
        if (!CheckNoTarget() && targetDistance >= breakDistance)
        {
            ClearTarget();
        }

        //if (keepDistance && !CheckClear() && targetDistance <= breakDistance)
        //{
        //    //currentTargetTransform.Translate(Mathf.Abs(breakDistance - targetDistance) * -targetDir);
        //    //float newPosX = currentTargetTransform.position.x - (breakDistance + 0.2f * ((transform.position.x - currentTargetTransform.position.x) / Mathf.Sqrt(Mathf.Pow(transform.position.x - currentTargetTransform.position.x, 2) + Mathf.Pow(transform.position.y - currentTargetTransform.position.y, 2))));
        //    //float newPosY = currentTargetTransform.position.y - (breakDistance + 0.2f * ((transform.position.y - currentTargetTransform.position.y) / Mathf.Sqrt(Mathf.Pow(transform.position.x - currentTargetTransform.position.x, 2) + Mathf.Pow(transform.position.y - currentTargetTransform.position.y, 2))));

        //    //float newDistance = Vector2.Distance(currentTargetTransform.position, transform.position);

        //    //currentTargetTransform.position.Set(newPosX, newPosY, 0);

        //    //currentTargetTransform.position = transform.position + breakDistance * ((transform.position - currentTargetTransform.position) * (transform.position - currentTargetTransform.position).normalized);
        //    //float rise = currentTargetTransform.position.y - transform.position.y;
        //    //float run = currentTargetTransform.position.x - transform.position.x;
        //    //float slope = rise / run;

        //    //currentTargetTransform.position = transform.position + (-targetDir * 5);
        //}
        

        //if (currentTargetTransform.position != transform.position && currentTargetTransform.gameObject.activeSelf == false)
        //{
        //    ClearTarget();
        //}
        if (patrolArea)
        {
            patrolTimeCounter -= Time.deltaTime;
            if (patrolTimeCounter <= 0.0f)
            {
                int index = Random.Range(0, patrolTransforms.transform.childCount);
                currentTargetTransform.position = patrolTransforms.transform.GetChild(index).transform.position;
                patrolTimeCounter = patrolTime;
            }
        }
        else if (currentTargetTransform.position == transform.position)
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
                if (keepDistance && targetDistance < breakDistance)
                {
                    currentTargetTransform.position = collider2D.transform.position + (-targetDir * 5);
                }
                else if(keepDistance && !CheckNoTarget() && Mathf.Abs(targetDistance - breakDistance) < 0.5)
                {
                    ClearTarget();
                }
                else
                {
                    currentTargetTransform.position = collider2D.transform.position;
                }
                return;
            }
        }
    }
    public void ClearTarget()
    {
        currentTargetTransform.position = transform.position;
    }
    public bool CheckNoTarget()
    {
        if(currentTargetTransform.position == transform.position)
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
