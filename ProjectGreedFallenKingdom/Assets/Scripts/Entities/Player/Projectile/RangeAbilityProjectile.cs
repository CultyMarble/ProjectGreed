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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            AbilityStatusEffect enemyStatusEffect = collision.gameObject.GetComponent<Enemy>().CheckStatusEffect();
            //collision.GetComponent<GeneralStatusEffect>().IncreaseRotStack(10);
            collision.gameObject.GetComponent<EnemyHealth>().UpdateCurrentHealth(-particleDamage);
            if(enemyStatusEffect != AbilityStatusEffect.none)
            {
                statusEffect = enemyStatusEffect;
            }
            collision.gameObject.GetComponent<Enemy>().InflictStatusEffect(statusEffect);

            //gameObject.SetActive(false);
            //Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Collisions")
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