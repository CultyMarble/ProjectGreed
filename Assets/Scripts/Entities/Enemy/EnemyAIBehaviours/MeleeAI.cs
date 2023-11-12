using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TargetingAI))]
public class MeleeAI : MonoBehaviour
{
    [SerializeField] private float activateDistance;
    [SerializeField] private int damage;
    [SerializeField] private float coolDownTime;
    [SerializeField] private bool dieAfter;
    [SerializeField] private bool delayAttack;
    [SerializeField] private float attackDelayTime;

    private TargetingAI targetingAI;
    private Animator animator;
    private float coolDownTimeCounter;
    private float attackDelayTimer;
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
        attackDelayTimer = attackDelayTime;
    }

    private void Update()
    {
        if (canMelee && !targetingAI.CheckNoTarget())
        {
            if (Vector2.Distance(transform.position, targetingAI.currentTarget) <= activateDistance)
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
                coolDownTimeCounter = 0.0f;
            }
        }
    }

    private void Melee()
    {
        animator.SetBool("isMeleeing",true);
    }

    public void DealMeleeDamage()
    {
        if (targetingAI.currentDestination == null)
            return;

        if(!delayAttack || attackDelayTimer <= 0)
        {
            Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, activateDistance);
            foreach (Collider2D collider2D in collider2DArray)
            {
                if (collider2D.GetComponent<Player>() != null)
                {
                    collider2D.GetComponent<PlayerHeart>().UpdateCurrentHeart(-damage);
                    break;
                }
            }
            targetingAI.isAttacking = false;
            animator.SetBool("isMeleeing", false);
            if (dieAfter)
            {
                GetComponent<EnemyHealth>().Despawn();
            }
            attackDelayTimer = attackDelayTime;
        }
        else if (delayAttack)
        {
            attackDelayTimer -= Time.deltaTime;
        }
        
    }
}
