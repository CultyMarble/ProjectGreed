using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TargetingAI))]
public class MeleeAI : MonoBehaviour
{
    [SerializeField] private float activateDistance;
    [SerializeField] private int damage;
    [SerializeField] private float coolDownTime;

    private TargetingAI targetingAI;
    private Animator animator;
    private float coolDownTimeCounter;
    private bool canMelee;

    //===========================================================================
    private void Awake()
    {
        animator = GetComponent<Animator>();
        targetingAI = GetComponent<TargetingAI>();
    }

    private void Start()
    {
        canMelee = false;
        coolDownTimeCounter = coolDownTime;
    }

    private void Update()
    {
        if (canMelee && targetingAI.currentTargetTransform != null)
        {
            if (Vector2.Distance(transform.position, targetingAI.currentTargetTransform.position) <= activateDistance)
            {
                if (targetingAI.isAttacking != true)
                {
                    Melee();
                    coolDownTimeCounter += coolDownTime;
                    canMelee = false;
                }
            }
        }
        else
        {
            coolDownTimeCounter -= Time.deltaTime;
            if (coolDownTimeCounter <= 0.0f)
            {
                canMelee = true;
            }
        }
    }

    private void Melee()
    {
        animator.SetTrigger("isMeleeing");
    }

    public void DealDamage()
    {
        if (targetingAI.currentTargetTransform == null)
            return;

        if (Vector2.Distance(transform.position, targetingAI.currentTargetTransform.position) <= activateDistance)
            //targetingAI.currentTargetTransform.GetComponent<PlayerPrefs>().UpdateCurrentHealth(damage);

        targetingAI.isAttacking = false;
    }
}
