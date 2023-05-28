using UnityEngine;

public class PlayerCurrencies : SingletonMonobehaviour<PlayerCurrencies>
{
    public class OnTempCurrencyAmountChangedEventArgs { public int amount;}
    public event System.EventHandler<OnTempCurrencyAmountChangedEventArgs> OnTempCurrencyAmountChanged;
    public class OnPermCurrencyAmountChangedEventArgs { public int amount; }
    public event System.EventHandler<OnPermCurrencyAmountChangedEventArgs> OnPermCurrencyAmountChanged;

    private int tempCurrencyAmount = 1500;
    private int permCurrencyAmount = 50;

    //===========================================================================
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            IncreaseTempCurrencyAmount(20);
        }

        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            IncreasePermCurrencyAmount(1);
        }
    }

    //===========================================================================
    public int GetTempCurrencyAmount()
    {
        return tempCurrencyAmount;
    }

    public int GetPermCurrencyAmount()
    {
        return permCurrencyAmount;
    }

    public void IncreaseTempCurrencyAmount(int amount)
    {
        tempCurrencyAmount += amount;

        OnTempCurrencyAmountChanged?.Invoke(this, new OnTempCurrencyAmountChangedEventArgs { amount = tempCurrencyAmount });
    }

    public void IncreasePermCurrencyAmount(int amount)
    {
        permCurrencyAmount += amount;

        OnPermCurrencyAmountChanged?.Invoke(this, new OnPermCurrencyAmountChangedEventArgs { amount = permCurrencyAmount });
    }
}
