using UnityEngine;

[RequireComponent(typeof(TargetingAI))]
public class SlamAI : MonoBehaviour
{
    [SerializeField] private float activateDistance;
    [SerializeField] private float damageRadius;
    [SerializeField][Range(0, 50)] private int damage;
    [SerializeField] private float coolDownTime;

    [SerializeField] private SpriteRenderer[] abilityEffect;
    [SerializeField] private Sprite[] effectSprites;

    private readonly float effectAnimationSpeed = 0.05f;
    private float effectAnimationTimer;
    private int currentAnimationIndex;

    private TargetingAI targetingAI;
    private Animator animator;
    private float coolDownTimeCounter;
    private bool canSlam;
    private bool isSlamming;

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
        if (canSlam && !targetingAI.CheckNoTarget())
        {
            if (Vector2.Distance(transform.position, targetingAI.currentTarget) <= activateDistance)
            {
                if (targetingAI.isAttacking != true)
                {
                    Slam();
                    coolDownTimeCounter = coolDownTime;
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
        if (isSlamming)
        {
            //Invoke(nameof(AbilityEffectAnimation), 1f);
            AbilityEffectAnimation();
        }
    }

    private void Slam()
    {
        animator.SetBool("isSlamming",true);
    }

    public void DealSlamDamage()
    {
        if (targetingAI.CheckNoTarget())
            return;

        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, activateDistance);
        foreach (Collider2D collider2D in collider2DArray)
        {
            if (collider2D.GetComponent<Player>() != null)
            {
                collider2D.GetComponent<PlayerHeart>().UpdateCurrentHeart(-damage);
                return;
            }
        }
        targetingAI.isAttacking = false;
        animator.SetBool("isSlamming",false);

        return;
    }
    public void StartAbilityAnimation()
    {
        isSlamming = true;
    }
    public void AbilityEffectAnimation()
    {
        effectAnimationTimer += Time.deltaTime;
        if (effectAnimationTimer >= effectAnimationSpeed)
        {
            effectAnimationTimer -= effectAnimationSpeed;

            if (currentAnimationIndex == effectSprites.Length)
            {
                // Hide effect sprite
                foreach (SpriteRenderer spriteRenderer in abilityEffect)
                {
                    spriteRenderer.sprite = null;
                }
                currentAnimationIndex = 0;
                isSlamming = false;
                return;
            }
            foreach (SpriteRenderer spriteRenderer in abilityEffect)
            {
                spriteRenderer.sprite = effectSprites[currentAnimationIndex];
            }
            currentAnimationIndex++;
        }
    }

}