using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PrefabEnemyBullet : MonoBehaviour
{
    private int damage;
    private float moveSpeed;
    private Vector3 moveDirection;

    //===========================================================================
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            collision.GetComponent<EnemyHealth>().UpdateCurrentHealth(damage);
            Destroy(this.gameObject);
        }

        Destroy(this.gameObject);
    }

    //===========================================================================
    private void Update()
    {
        transform.position += moveSpeed * Time.deltaTime * moveDirection;
    }

    //===========================================================================
    public void SetDamage(int newAmount)
    {
        damage = newAmount;
    }
    public void SetMoveDirectionAndSpeed(Vector3 newMoveDirection, float newSpeed)
    {
        moveDirection = newMoveDirection;
        moveSpeed = newSpeed;
    }
}