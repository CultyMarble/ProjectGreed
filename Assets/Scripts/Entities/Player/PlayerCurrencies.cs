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
        tempCurrencyAmount = Mathf.Clamp(tempCurrencyAmount, 0, int.MaxValue);

        GameplayInfoController.Instance.DisplayPlayerCurrency.UpdateTempCurrencyText(tempCurrencyAmount);
    }

    public void UpdatePermCurrencyAmount(int amount = 0)
    {
        permCurrencyAmount += amount;
        permCurrencyAmount = Mathf.Clamp(permCurrencyAmount, 0, int.MaxValue);

        GameplayInfoController.Instance.DisplayPlayerCurrency.UpdatePermCurrencyText(permCurrencyAmount);
    }
}