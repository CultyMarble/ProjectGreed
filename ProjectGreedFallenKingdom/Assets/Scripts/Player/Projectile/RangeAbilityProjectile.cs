using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RangeAbilityProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D projectileRigidbody2D;

    private int rotStack;
    private float moveSpeed;
    private Vector3 moveDirection;

    //===========================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            //collision.GetComponent<GeneralStatusEffect>().IncreaseRotStack(10);
            //collision.GetComponent<EnemyHealth>().DamageFeedBack();
        }
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    //===========================================================================
    private void Update()
    {
        transform.position += moveSpeed * Time.deltaTime * moveDirection;
    }

    //===========================================================================
    public void ProjectileConfig(int newRotStack, float newMoveSpeed, Transform startPositionTransform)
    {
        rotStack = newRotStack;

        moveSpeed = newMoveSpeed;
        moveDirection = (CultyMarbleHelper.GetMouseToWorldPosition() -
            startPositionTransform.position).normalized;
    }
}