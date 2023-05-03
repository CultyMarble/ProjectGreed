using UnityEngine;

public class AreaAbility : MonoBehaviour
{
    [Header("Effect Settings:")]
    [SerializeField] private SpriteRenderer abilityEffect;
    [SerializeField] private Sprite[] effectSprites;

    private readonly float effectAnimationSpeed = 0.05f;
    private float effectAnimationTimer;
    private int currentAnimationIndex;

    [Header("Ability Settings:")]
    [SerializeField] private float cooldown;
    [SerializeField] private float damage;
    [SerializeField] private float pushPower;
    [SerializeField] private float pushRadius;

    private float cooldownTimer = default;

    //============================================================================
    private void Update()
    {
        AbilityCooldown();

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
    private void AbilityCooldown()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer < 0)
                cooldownTimer = 0;
        }
    }

    private void AbilityInputHandler()
    {
        if (Input.GetKeyDown(KeyCode.Q) && cooldownTimer == 0)
        {
            // Reset Animation Settings
            effectAnimationTimer = 0.0f;
            currentAnimationIndex = 0;

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
                abilityEffect.sprite = null;

                // Put ability on CD
                cooldownTimer = cooldown;

                Player.Instance.playerActionState = PlayerActionState.none;
                return;
            }

            abilityEffect.sprite = effectSprites[currentAnimationIndex];
            currentAnimationIndex++;
        }
    }

    private void PushEnemyInRadius()
    {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, pushRadius);
        foreach (Collider2D collider2D in collider2DArray)
        {
            if (collider2D.CompareTag("Enemy"))
            {
                // Reduce health
                collider2D.GetComponent<EnemyHealth>().UpdateCurrentHealth((int)damage);

                // Push back
                Transform enemyTranform = collider2D.GetComponent<Transform>();

                Vector2 _pushDirection = (enemyTranform.position - GetComponent<Player>().transform.position).normalized;
                float _eulerAngle = CultyMarbleHelper.GetAngleFromVector(_pushDirection);

                // Stop current movement
                if (collider2D.GetComponent<ChasingAI>() != null)
                {
                    collider2D.GetComponent<ChasingAI>().holdMovementDirection = true;
                    collider2D.GetComponent<ChasingAI>().holdtimer = 0.5f;
                }

                // Add force
                collider2D.GetComponent<Rigidbody2D>().AddForce(_pushDirection * pushPower, ForceMode2D.Impulse);
            }
        }
    }
}