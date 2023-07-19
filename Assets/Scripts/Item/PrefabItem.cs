using UnityEngine;

public class PrefabItem : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private float IncreaseDamage;
    [SerializeField] private float IncreaseMaxHealth;
    [SerializeField] private float IncreaseMaxFuel;
    [SerializeField] private float reduceDashCD;

    private bool canPickedUp;

    //===========================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.Instance.ShowFPromtText();
            canPickedUp = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.Instance.HideFPromtText();
            canPickedUp = false;
        }
    }

    //===========================================================================
    private void Update()
    {
        if (canPickedUp == false)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (GetComponent<ItemCost>() != null)
            {
                if (PlayerCurrencies.Instance.TempCurrencyAmount >= GetComponent<ItemCost>().itemCost)
                {
                    PlayerCurrencies.Instance.UpdateTempCurrencyAmount(-(GetComponent<ItemCost>().itemCost));

                    transform.parent.GetComponentInParent<ShopKeeper>().SetItemPurchase(transform.parent.GetSiblingIndex());
                }
                else
                {
                    return;
                }
            }

            GameObject player = FindObjectOfType<Player>().gameObject;

            // Item Effect
            player.GetComponentInChildren<BasicAbility>().UpdateDamage(IncreaseDamage);

            player.GetComponent<PlayerHealth>().UpdateCurrentMaxHealth(IncreaseMaxHealth);

            player.GetComponent<PlayerController>().dashCD -= reduceDashCD;
            if (player.GetComponent<PlayerController>().dashCD <= 0)
                player.GetComponent<PlayerController>().dashCD = 0;

            player.GetComponentInChildren<BasicAbility>().UpdateMaxFuel(IncreaseMaxFuel);
            //

            Player.Instance.HideFPromtText();

            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }
}