using UnityEngine;

public abstract class TempItem : MonoBehaviour
{
    [SerializeField] private string itemName;

    private bool canPickedUp;

    //===========================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.Instance.SetInteractPromtTextActive(true);
            canPickedUp = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.Instance.SetInteractPromtTextActive(false);
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
            if (ItemCostCheck() == false)
                return;

            GameObject player = FindObjectOfType<Player>().gameObject;

            AddItemEffect();

            Player.Instance.SetInteractPromtTextActive(false);

            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }

    //===========================================================================
    private bool ItemCostCheck()
    {
        if (GetComponent<ItemCost>() == null)
            return true;

        if (PlayerCurrencies.Instance.TempCurrencyAmount >= GetComponent<ItemCost>().itemCost)
        {
            PlayerCurrencies.Instance.UpdateTempCurrencyAmount(-(GetComponent<ItemCost>().itemCost));

            transform.parent.GetComponentInParent<ShopKeeper>().SetItemPurchase(transform.parent.GetSiblingIndex());

            return true;
        }

        return false;
    }

    //===========================================================================
    protected abstract void AddItemEffect();
}