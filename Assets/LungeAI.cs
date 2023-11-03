using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LungeAI : MonoBehaviour
{
    [SerializeField] private float lungeActivateDistance;
    
    [SerializeField] private int damage;
    [SerializeField] private float coolDownTime;
    [SerializeField] private float chargeTime;

    private Pathfinding.AIPath pathfinder;
    private TargetingAI targetingAI;
    private Animator animator;
    private float coolDownTimeCounter;
    private bool canLunge;
    private bool isLunging;
    private bool isCharging;
    private float chargeTimeCounter;
    private float lungeTimer = 0;

    //===========================================================================
    private void Awake()
    {
        animator = GetComponent<Animator>();
        targetingAI = GetComponent<TargetingAI>();
    }

    private void Start()
    {
        canLunge = false;
        coolDownTimeCounter = coolDownTime;
        chargeTimeCounter = chargeTime;
        pathfinder = targetingAI.pathfinder;
    }

    private void Update()
    {
        if (SceneControlManager.Instance.GameState == GameState.PauseMenu ||
            SceneControlManager.Instance.GameState == GameState.OptionMenu)
        {
            return;
        }
        if (canLunge && !targetingAI.CheckNoTarget() && !isLunging && !isCharging)
        {
            if (Vector2.Distance(transform.position, targetingAI.currentTarget) <= lungeActivateDistance)
            {
                if (targetingAI.isAttacking != true)
                {
                    ChargeLunge();
                    coolDownTimeCounter += coolDownTime;
                    canLunge = false;
                }
            }
        }
        else
        {
            coolDownTimeCounter -= Time.deltaTime;
            if (coolDownTimeCounter <= 0.0f)
            {
                canLunge = true;
                coolDownTimeCounter = 0.0f;
            }
        }
        if (isLunging)
        {
            lungeTimer += Time.deltaTime;
            pathfinder.maxSpeed = 8 / (lungeTimer/0.5f);
            DealLungeDamage();
            return;
        }
        else if (isCharging)
        {
            chargeTimeCounter -= Time.deltaTime;
            pathfinder.maxSpeed = 0;
            if (chargeTimeCounter <= 0.0f)
            {
                chargeTimeCounter = chargeTime;
                Lunge();
            }
        }
    }

    private void Lunge()
    {
        //targetingAI.dontUpdateDestination = true;
        targetingAI.DontUpdateDestination(true);
        isCharging = false;
        isLunging = true;
        animator.SetBool("isCharging", false);
        animator.SetBool("isLunging", true);
        pathfinder.maxSpeed = 12;
    }

    public void EndLunge()
    {
        isLunging = false;
        isCharging = false;
        targetingAI.isAttacking = false;
        targetingAI.DontUpdateDestination(false);
        animator.SetBool("isLunging", false);
        animator.SetBool("isCharging", false);
        animator.SetBool("isIdle", true);
        pathfinder.maxSpeed = 0;
        Invoke(nameof(ResetSpeed), 0.2f);
        //ResetSpeed();
        lungeTimer = 0;
    }

    private void ChargeLunge()
    {
        isCharging = true;
        animator.SetBool("isCharging", true);
    }

    private void ResetSpeed()
    {
        pathfinder.maxSpeed = 2;
    }

    public void DealLungeDamage()
    {
        if (targetingAI.currentDestination == null)
            return;

        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (Collider2D collider2D in collider2DArray)
        {
            if (collider2D.GetComponent<Player>() != null)
            {
                collider2D.GetComponent<PlayerHeart>().UpdateCurrentHeart(-damage);
                EndLunge();
                return;
            }
        }
    }
}
