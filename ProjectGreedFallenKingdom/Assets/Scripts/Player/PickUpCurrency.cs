using UnityEngine;

public class PickUpCurrency : MonoBehaviour
{
    [SerializeField] private float pickUpRadius;

    private void FixedUpdate()
    {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, pickUpRadius);
        foreach (Collider2D collider2D in collider2DArray)
        {
            if (collider2D.GetComponent<MoveToPlayer>() != null)
            {
                collider2D.GetComponent<MoveToPlayer>().moveToPlayer = true;
            }
        }
    }
}
