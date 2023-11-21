using UnityEngine;

public class PickupHealth : MonoBehaviour
{
    //===========================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetType().ToString() == Tags.BOXCOLLIDER2D)
        {
            Player.Instance.GetComponent<PlayerHeart>().UpdateCurrentHeart(1);

            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
