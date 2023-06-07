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
        if (isSlamming)
        {
            Invoke(nameof(AbilityEffectAnimation), 0.6f);
        }
    }

    private void Slam()
    {
        Collider2D player = FindPlayer();
        if (player != null)
        {
            animator.SetTrigger("isSlamming");
            isSlamming = true;
            DealDamage(player);
        }
    }

    public void DealDamage(Collider2D player)
    {
        player.GetComponent<PlayerHealth>().UpdateCurrentHealth(-damage);

        targetingAI.isAttacking = false;
    }
    public Collider2D FindPlayer()
    {
        if (targetingAI.CheckNoTarget())
            return null;

        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, activateDistance);
        foreach (Collider2D collider2D in collider2DArray)
        {
            if (collider2D.GetComponent<Player>() != null)
            {
                return collider2D;
            }
        }
        return null;
    }
    private void AbilityEffectAnimation()
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