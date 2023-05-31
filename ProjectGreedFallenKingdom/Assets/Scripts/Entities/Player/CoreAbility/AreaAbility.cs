using UnityEngine;

public class AreaAbility : CoreAbility
{
    [Header("Effect Animation Settings:")]
    [SerializeField] private SpriteRenderer[] abilityEffect;
    [SerializeField] private Sprite[] effectSprites;

    private readonly float effectAnimationSpeed = 0.05f;
    private float effectAnimationTimer;
    private int currentAnimationIndex;

    //============================================================================
    protected override void Update()
    {
        base.Update();

        switch (Player.Instance.playerActionState)
        {
            case PlayerActionState.none:
                AbilityInputHandler();
                break;

            case PlayerActionState.IsUsingAreaAbility:
                AbilityEffectAnimation();
                break;

            default:
                break;
        }
    }

    //============================================================================
    private void AbilityInputHandler()
    {
        if (Input.GetKeyDown(KeyCode.Q) && cooldownTimer == 0)
        {
            // Reset Animation Settings
            effectAnimationTimer = 0.0f;
            currentAnimationIndex = 0;

            // Push Enemy Back
            PushEnemyInRadius();

            // Set player is Attacking
            Player.Instance.playerActionState = PlayerActionState.IsUsingAreaAbility;
        }
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
                foreach(SpriteRenderer spriteRenderer in abilityEffect)
                {
                    spriteRenderer.sprite = null;
                }

                // Put ability on CD
                cooldownTimer = cooldown;

                Player.Instance.playerActionState = PlayerActionState.none;
                return;
            }

            foreach (SpriteRenderer spriteRenderer in abilityEffect)
            {
                spriteRenderer.sprite = effectSprites[currentAnimationIndex];
            }
            currentAnimationIndex++;
        }
    }

    private void PushEnemyInRadius()
    {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, pushRadius);
        if (collider2DArray.Length == 0)
            return;

        foreach (Collider2D collider2D in collider2DArray)
        {
            if (collider2D.CompareTag("Enemy"))
            {
                // Reduce health
                collider2D.GetComponent<EnemyHealth>().UpdateCurrentHealth((int)damage);

                // Push back
                Transform enemyTranform = collider2D.GetComponent<Transform>();

                Vector2 _pushDirection = (enemyTranform.position - GetComponentInParent<Player>().transform.position).normalized;
                float _eulerAngle = CultyMarbleHelper.GetAngleFromVector(_pushDirection);

                // Stop current movement
                //if (collider2D.GetComponent<ChasingAI>() != null)
                //{
                //    collider2D.GetComponent<ChasingAI>().holdMovementDirection = true;
                //    collider2D.GetComponent<ChasingAI>().holdtimer = 0.5f;
                //}
                //else if (collider2D.GetComponent<ChasingAIBasic>() != null)
                //{
                //    collider2D.GetComponent<ChasingAIBasic>().holdMovementDirection = true;
                //    collider2D.GetComponent<ChasingAIBasic>().holdtimer = 0.5f;
                //}

                collider2D.GetComponent<TargetingAI>().HoldMovement();
                // Add force
                collider2D.GetComponent<Enemy>().isPushBack = true;
                collider2D.GetComponent<Rigidbody2D>().AddForce(_pushDirection * pushPower, ForceMode2D.Force);
            }
        }
    }
}