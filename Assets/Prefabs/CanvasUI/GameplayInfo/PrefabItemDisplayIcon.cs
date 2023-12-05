using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrefabItemDisplayIcon : MonoBehaviour
{
    public Image itemDisplayIcon = default;
    public int iconID = default;
    [SerializeField] private TextMeshProUGUI amountText = default;

    private int amount = default;

    private void OnEnable()
    {
        itemDisplayIcon.sprite = null;
        iconID = 0;
        amount = 0;
    }

    public void ResetAmount() { amount = 0; }

    public void UpdateAmount()
    {
        amount++;
        UpdateTextAmount();
    }

    private void UpdateTextAmount()
    {
        amountText.SetText(amount.ToString());
    }
}