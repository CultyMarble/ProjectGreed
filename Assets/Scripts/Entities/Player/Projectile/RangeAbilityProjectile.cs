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

    [Header("Poison Pool")]
    [SerializeField] private Transform pfPoisonPool = default;
    private bool canCreatePoisonPool = default;
    private int amount = default;

    //===========================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canCreatePoisonPool)
        {
            if (collision.gameObject.CompareTag("Enemy") ||collision.gameObject.CompareTag("Breakable") ||
                collision.gameObject.CompareTag("FinalBoss") ||collision.gameObject.CompareTag("Collisions"))
                Despawn();
            return;
        }

        if (collision.gameObject.CompareTag("Enemy") && collision.GetType().ToString() != Tags.CIRCLECOLLIDER2D)
            collision.gameObject.GetComponent<EnemyHealth>().UpdateCurrentHealth(-damage);

        if (collision.gameObject.CompareTag("Breakable"))
            collision.gameObject.GetComponent<BreakableItem>().UpdateCurrentHealth(-damage);

        if (collision.gameObject.CompareTag("FinalBoss"))
            collision.transform.parent.GetComponent<EnemyHealth>().UpdateCurrentHealth(-damage);

        if (collision.gameObject.CompareTag("Collisions"))
            Despawn();
    }

    //===========================================================================
    private void OnEnable()
    {
        amount = 1;
    }

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
    public void Despawn()
    {
        if (canCreatePoisonPool && amount == 1)
        {
            amount--;
            Transform _parent = GameObject.Find("AbilityPool").transform;
            Transform _pool = Instantiate(pfPoisonPool, _parent);
            _pool.position = transform.position;

            Destroy(_pool.gameObject, 3.0f);
        }

        gameObject.SetActive(false);
        gameObject.transform.localPosition = Vector2.zero;
    }

    public void ProjectileConfig(float newMoveSpeed, Transform startPositionTransform, float newDamage)
    {
        damage = newDamage;
        moveSpeed = newMoveSpeed;

        moveDirection = (CultyMarbleHelper.GetMouseToWorldPosition() - startPositionTransform.position).normalized;
        transform.up = moveDirection;
    }

    public void CanCreatePoisonPool(bool active)
    {
        canCreatePoisonPool = active;
    }
}