using UnityEngine;

public class BombAbilityCharge : MonoBehaviour
{
    //===========================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetType().ToString() == Tags.BOXCOLLIDER2D)
        {
            Player.Instance.GetComponentInChildren<BombAbility>().UpdateBombCharge(1);

            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
