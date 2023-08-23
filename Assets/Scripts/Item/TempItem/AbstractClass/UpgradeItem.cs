using UnityEngine;

public abstract class UpgradeItem : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;

    private bool canPickedUp;

    //===========================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetType().ToString() == Tags.BOXCOLLIDER2D)
        {
            RunInfoController.Instance.SetItemInfoPanelActive(true);
            RunInfoController.Instance.SetItemNameText(itemName);
            RunInfoController.Instance.SetItemDescriptionText(itemDescription);

            Player.Instance.SetInteractPromtTextActive(true);
            canPickedUp = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetType().ToString() == Tags.BOXCOLLIDER2D)
        {
            RunInfoController.Instance.SetItemNameText("");
            RunInfoController.Instance.SetItemDescriptionText("");
            RunInfoController.Instance.SetItemInfoPanelActive(false);

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
            return true;
        }

        return false;
    }

    //===========================================================================
    protected abstract void AddItemEffect();
    protected abstract void RemoveItemEffect();
}