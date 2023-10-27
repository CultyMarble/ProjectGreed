using UnityEngine;

public class CurrencyShopTrigger : MonoBehaviour
{
    private PlayerCurrencies playerCurrencies;

    private void Awake()
    {
        playerCurrencies = FindObjectOfType<PlayerCurrencies>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.GetType().ToString() != Tags.CAPSULECOLLIDER2D)
        {
            // playerCurrencies.Open();
            Debug.Log("Show Currencies");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.GetType().ToString() != Tags.CAPSULECOLLIDER2D)
        {
            // playerCurrencies.Close();
            Debug.Log("Hide Currencies");
        }
    }

}
