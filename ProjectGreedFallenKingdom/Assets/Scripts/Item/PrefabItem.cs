using UnityEngine;

public class PrefabItem : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private int addDamage;
    [SerializeField] private int addHealth;
    [SerializeField] private float reduceDashCD;

    private bool pickedUp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Player.Instance.ShowFPromtText();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKey(KeyCode.F) && pickedUp == false)
        {
            pickedUp = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Player.Instance.HideFPromtText();
    }

    private void Update()
    {
        if (pickedUp)
        {
            if (GetComponent<ItemCost>() != null)
            {// item has price
                if (PlayerCurrencies.Instance.GetTempCurrencyAmount() >= GetComponent<ItemCost>().itemCost)
                {// player can afford item
                    PlayerCurrencies.Instance.IncreaseTempCurrencyAmount(-(GetComponent<ItemCost>().itemCost));
                }
                else
                {
                    return;
                }
            }

            // PlayerOffenseControl.Instance.meleeDamage += addDamage;
            FindObjectOfType<Player>().GetComponent<EnemyHealth>().maxHealth += addHealth;
            //PlayerMovement.Instance.dashCD -= reduceDashCD;

            Player.Instance.HideFPromtText();

            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }
}