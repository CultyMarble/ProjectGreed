using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RangeAbilityProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D projectileRigidbody2D;

    private float moveSpeed;
    private Vector3 moveDirection;
    private float damage;

    [Header("Effect Animation Settings:")]
    [SerializeField] private SpriteRenderer abilityEffect;
    [SerializeField] private Sprite[] effectSprites;

    private readonly float animationSpeed = 0.1f;
    private float animationTimer;
    private int currentAnimationIndex;

    //===========================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && collision.GetType().ToString() != Tags.CIRCLECOLLIDER2D)
        {
            // Deal Damage
            collision.gameObject.GetComponent<EnemyHealth>().UpdateCurrentHealth(-damage);

            // Deal Status Effect

        }

        if (collision.gameObject.CompareTag("Breakable"))
        {
            collision.gameObject.GetComponent<BreakableItem>().UpdateCurrentHealth(-damage);

        }

        if (collision.gameObject.CompareTag("Collisions"))
        {
            gameObject.SetActive(false);
            gameObject.transform.localPosition = Vector2.zero;
        }
    }

    //===========================================================================
    private void Update()
    {
        if (SceneControlManager.Instance.CurrentGameplayState == GameplayState.Pause)
            return;
        transform.position += moveSpeed * Time.deltaTime * moveDirection;

        ProjectileAnimation();
    }

    //===========================================================================
    private void ProjectileAnimation()
    {
        animationTimer += Time.deltaTime;
        if (animationTimer >= animationSpeed)
        {
            animationTimer -= animationSpeed;

            if (currentAnimationIndex == effectSprites.Length)
                currentAnimationIndex = 0;

            abilityEffect.sprite = effectSprites[currentAnimationIndex];
            currentAnimationIndex++;
        }
    }

    //===========================================================================
    public void ProjectileConfig(float newMoveSpeed, Transform startPositionTransform, float newDamage)
    {
        damage = newDamage;
        moveSpeed = newMoveSpeed;

        moveDirection = (CultyMarbleHelper.GetMouseToWorldPosition() - startPositionTransform.position).normalized;
        transform.up = moveDirection;
    }
}