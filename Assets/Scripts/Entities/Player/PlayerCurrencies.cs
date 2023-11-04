using UnityEngine;

public class PlayerCurrencies : SingletonMonobehaviour<PlayerCurrencies>
{
    public enum KeyType
    {
        Silver,
        Gold
    }

    [SerializeField] private DisplayPlayerCurrency displayPlayerCurrency;
    [SerializeField] private PlayerHeart playerHeartManager;
    private int tempCurrencyAmount = 0;
    public bool hasSilverKey = false;
    public bool hasGoldKey = false;

    //private int permCurrencyAmount = 50;

    public int TempCurrencyAmount => tempCurrencyAmount;
    public int PermCurrencyAmount => tempCurrencyAmount;

    //===========================================================================
    private void OnEnable()
    {
        playerHeartManager.OnDespawnPlayerEvent += OnDespawnPlayer_ResetCurrencyWrapper;
    }
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
    public void ResetCurrency()
    {
        tempCurrencyAmount = 0;
        hasSilverKey = false;
        hasGoldKey = false;
        PlayerInfoController.Instance.DisplayPlayerCurrency.UpdateTempCurrencyText(0);
        PlayerInfoController.Instance.DisplayPlayerCurrency.UpdateSilverKeyIcon(false);
        PlayerInfoController.Instance.DisplayPlayerCurrency.UpdateGoldKeyIcon(false);
    }
    public void OnDespawnPlayer_ResetCurrencyWrapper(object sender, System.EventArgs e)
    {
        ResetCurrency();
        PlayerInfoController.Instance.DisplayPlayerCurrency.UpdateTempCurrencyText(tempCurrencyAmount);
        PlayerInfoController.Instance.DisplayPlayerCurrency.UpdateSilverKeyIcon(hasSilverKey);
        PlayerInfoController.Instance.DisplayPlayerCurrency.UpdateGoldKeyIcon(hasGoldKey);
    }

    //public void UpdatePermCurrencyAmount(int amount = 0)
    //{
    //    permCurrencyAmount += amount;
    //    permCurrencyAmount = Mathf.Clamp(permCurrencyAmount, 0, int.MaxValue);

    //    PlayerInfoController.Instance.DisplayPlayerCurrency.UpdatePermCurrencyText(permCurrencyAmount);
    //}
}