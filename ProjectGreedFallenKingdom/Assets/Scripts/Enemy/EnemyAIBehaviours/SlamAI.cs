using UnityEngine;

[RequireComponent(typeof(TargetingAI))]
public class SlamAI : MonoBehaviour
{
    [SerializeField] private float activateDistance;
    [SerializeField] private float damageRadius;
    [SerializeField][Range(0, 10)] private int damage;
    [SerializeField] private float coolDownTime;

    private TargetingAI targetingAI;
    private Animator animator;
    private float coolDownTimeCounter;
    private bool canSlam;

    //===========================================================================
    private void Awake()
    {
        animator = GetComponent<Animator>();
        targetingAI = GetComponent<TargetingAI>();
    }

    private void Start()
    {
        canSlam = false;
        coolDownTimeCounter = coolDownTime;
    }

    private void Update()
    {
        if (canSlam && targetingAI.currentTargetTransform != null)
        {
            if (Vector2.Distance(transform.position, targetingAI.currentTargetTransform.position) <= activateDistance)
            {
                if (targetingAI.isAttacking != true)
                {
                    Slam();
                    coolDownTimeCounter += coolDownTime;
                    canSlam = false;
                }
            }
        }
        else
        {
            coolDownTimeCounter -= Time.deltaTime;
            if (coolDownTimeCounter <= 0.0f)
            {
                canSlam = true;
            }
        }
    }

    private void Slam()
    {
        animator.SetTrigger("isSlamming");
    }

    public void DealDamage()
    {
        if (targetingAI.currentTargetTransform == null)
            return;

        if (Vector2.Distance(transform.position, targetingAI.currentTargetTransform.position) <= damageRadius)
        {
            targetingAI.currentTargetTransform.GetComponent<EnemyHealth>().UpdateCurrentHealth(damage);
        }

        targetingAI.isAttacking = false;
    }
}