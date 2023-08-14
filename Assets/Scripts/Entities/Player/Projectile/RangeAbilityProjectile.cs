using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RangeAbilityProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D projectileRigidbody2D;

    private float moveSpeed;
    private Vector3 moveDirection;
    private float particleDamage;
    private float lifetime = 2;
    private AbilityStatusEffect statusEffect;

    //===========================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && collision.GetType().ToString() != Tags.CIRCLECOLLIDER2D)
        {
            AbilityStatusEffect enemyStatusEffect = collision.gameObject.GetComponent<Enemy>().CheckStatusEffect();

            collision.gameObject.GetComponent<EnemyHealth>().UpdateCurrentHealth(-particleDamage);
            collision.gameObject.GetComponent<Enemy>().InflictStatusEffect(statusEffect, 3);

            if (statusEffect == AbilityStatusEffect.none && enemyStatusEffect != AbilityStatusEffect.none)
            {
                statusEffect = enemyStatusEffect;
            }
        }

        if (collision.gameObject.CompareTag("Collisions"))
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    //===========================================================================
    private void Update()
    {
        transform.position += moveSpeed * Time.deltaTime * moveDirection;
        lifetime -= Time.deltaTime;
        if(lifetime <= 0)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    //===========================================================================
    public void ProjectileConfig(float newMoveSpeed, Transform startPositionTransform, float damage)
    {
        particleDamage = damage;
        moveSpeed = newMoveSpeed;
        moveDirection = (CultyMarbleHelper.GetMouseToWorldPosition() - startPositionTransform.position).normalized;
    }
}