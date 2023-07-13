using UnityEngine;
using TMPro;

public class DisplayPlayerCurrency : SingletonMonobehaviour<DisplayPlayerCurrency>
{
    [SerializeField] private TextMeshProUGUI tempCurrencyText;
    [SerializeField] private TextMeshProUGUI permCurrencyText;

    //===========================================================================
    public void UpdateTempCurrencyText(int amount)
    {
        tempCurrencyText.SetText(amount.ToString());
    }

    public void UpdatePermCurrencyText(int amount)
    {
        permCurrencyText.SetText(amount.ToString());
    }
}
