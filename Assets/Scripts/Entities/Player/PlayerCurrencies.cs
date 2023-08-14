using System.Collections;
using UnityEngine;

public class PlayerCurrencies : SingletonMonobehaviour<PlayerCurrencies>
{
    [SerializeField] private DisplayPlayerCurrency displayPlayerCurrency;

    private int tempCurrencyAmount = 1500;
    private int permCurrencyAmount = 50;

    public int TempCurrencyAmount => tempCurrencyAmount;
    public int PermCurrencyAmount => tempCurrencyAmount;

    //===========================================================================
    private void OnEnable()
    {
        UpdatePermCurrencyAmount();
        UpdateTempCurrencyAmount();
    }

    //===========================================================================
    public void UpdateTempCurrencyAmount(int amount = 0)
    {
        tempCurrencyAmount += amount;
        tempCurrencyAmount = Mathf.Clamp(tempCurrencyAmount, 0, 10000);

        displayPlayerCurrency.UpdateTempCurrencyText(tempCurrencyAmount);
    }

    public void UpdatePermCurrencyAmount(int amount = 0)
    {
        permCurrencyAmount += amount;
        permCurrencyAmount = Mathf.Clamp(permCurrencyAmount, 0, 10000);

        displayPlayerCurrency.UpdatePermCurrencyText(permCurrencyAmount);
    }
}
