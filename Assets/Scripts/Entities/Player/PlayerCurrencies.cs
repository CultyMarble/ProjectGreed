using UnityEngine;

public class PlayerCurrencies : SingletonMonobehaviour<PlayerCurrencies>
{
    public enum KeyType
    {
        Silver,
        Gold
    }

    [SerializeField] private DisplayPlayerCurrency displayPlayerCurrency;

    private int tempCurrencyAmount = 0;
    public bool hasSilverKey = false;
    public bool hasGoldKey = false;

    //private int permCurrencyAmount = 50;

    public int TempCurrencyAmount => tempCurrencyAmount;
    public int PermCurrencyAmount => tempCurrencyAmount;

    //===========================================================================
    private void Start()
    {
        //UpdatePermCurrencyAmount();
        UpdateTempCurrencyAmount();
    }

    //===========================================================================
    public void UpdateTempCurrencyAmount(int amount = 0)
    {
        tempCurrencyAmount += amount;
        tempCurrencyAmount = Mathf.Clamp(tempCurrencyAmount, 0, int.MaxValue);

        PlayerInfoController.Instance.DisplayPlayerCurrency.UpdateTempCurrencyText(tempCurrencyAmount);
    }
    public void UpdateKeys()
    {
        PlayerInfoController.Instance.DisplayPlayerCurrency.UpdateSilverKeyIcon(hasSilverKey);
        PlayerInfoController.Instance.DisplayPlayerCurrency.UpdateGoldKeyIcon(hasGoldKey);
    }
    public void AddSilverKey()
    {
        hasSilverKey = true;
        UpdateKeys();
    }
    public void AddGoldKey()
    {
        hasGoldKey = true;
        UpdateKeys();
    }


    //public void UpdatePermCurrencyAmount(int amount = 0)
    //{
    //    permCurrencyAmount += amount;
    //    permCurrencyAmount = Mathf.Clamp(permCurrencyAmount, 0, int.MaxValue);

    //    PlayerInfoController.Instance.DisplayPlayerCurrency.UpdatePermCurrencyText(permCurrencyAmount);
    //}
}