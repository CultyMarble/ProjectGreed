using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingAI : MonoBehaviour
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
        if (currentTargetTransform != null &&
            Vector2.Distance(currentTargetTransform.position, transform.position) >= breakDistance)
        {
            currentTargetTransform = null;
        }

        if (currentTargetTransform && currentTargetTransform.gameObject.activeSelf == false)
        {
            currentTargetTransform = null;
        }

        if (currentTargetTransform == null)
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
                currentTargetTransform = collider2D.transform;
                return;
            }
        }
    }
}
