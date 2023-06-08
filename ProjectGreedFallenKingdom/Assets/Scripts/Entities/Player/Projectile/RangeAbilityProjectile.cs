using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RangeAbilityProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D projectileRigidbody2D;

    private int rotStack;
    private float moveSpeed;
    private Vector3 moveDirection;
    private int particleDamage;
    private float lifetime = 2;
    private AbilityStatusEffect statusEffect;

    //===========================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            AbilityStatusEffect enemyStatusEffect = collision.gameObject.GetComponent<Enemy>().CheckStatusEffect();

            collision.gameObject.GetComponent<EnemyHealth>().UpdateCurrentHealth(-particleDamage);

            if (statusEffect == AbilityStatusEffect.none && enemyStatusEffect != AbilityStatusEffect.none)
            {
                statusEffect = enemyStatusEffect;
            }

            collision.gameObject.GetComponent<Enemy>().InflictStatusEffect(statusEffect, 3);
        }

        if (collision.gameObject.CompareTag("Collisions"))
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

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
    public void ProjectileConfig(int newRotStack, float newMoveSpeed, Transform startPositionTransform, int damage, AbilityStatusEffect _statusEffect)
    {
        rotStack = newRotStack;
        particleDamage = damage;
        moveSpeed = newMoveSpeed;
        moveDirection = (CultyMarbleHelper.GetMouseToWorldPosition() -
            startPositionTransform.position).normalized;
        statusEffect = _statusEffect;
    }
}