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

    //===========================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            //collision.GetComponent<GeneralStatusEffect>().IncreaseRotStack(10);
            collision.GetComponent<EnemyHealth>().UpdateCurrentHealth(-particleDamage);
        }
        if (collision.CompareTag("Collisions"))
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
    public void ProjectileConfig(int newRotStack, float newMoveSpeed, Transform startPositionTransform, int damage)
    {
        rotStack = newRotStack;
        particleDamage = damage;
        moveSpeed = newMoveSpeed;
        moveDirection = (CultyMarbleHelper.GetMouseToWorldPosition() -
            startPositionTransform.position).normalized;
    }
}