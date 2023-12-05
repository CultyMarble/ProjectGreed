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

    private int tempCurrencyAmount = default;
    private int permCurrencyAmount = default;

    public bool hasSilverKey = false;
    public bool hasGoldKey = false;

    public int TempCurrencyAmount => tempCurrencyAmount;
    public int PermCurrencyAmount => permCurrencyAmount;

    //===========================================================================
    private void OnEnable()
    {
        SetPermCurrencyAmount(SaveDataManager.Instance.UpgradeData01.PermCurrencyAmount);
    }

    private void Start()
    {
        UpdatePermCurrencyAmount();
        UpdateTempCurrencyAmount();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.PageUp))
            UpdatePermCurrencyAmount(1);

        if (Input.GetKeyDown(KeyCode.PageDown))
            UpdatePermCurrencyAmount(-1);
    }

    //===========================================================================
    public void UpdateTempCurrencyAmount(int amount = 0)
    {
        tempCurrencyAmount += amount;
        tempCurrencyAmount = Mathf.Clamp(tempCurrencyAmount, 0, int.MaxValue);

        PlayerInfoController.Instance.DisplayPlayerCurrency.UpdateTempCurrencyText(tempCurrencyAmount);
    }

    public void UpdatePermCurrencyAmount(int amount = 0)
    {
        permCurrencyAmount += amount;
        permCurrencyAmount = Mathf.Clamp(permCurrencyAmount, 0, int.MaxValue);

        PlayerInfoController.Instance.DisplayPlayerCurrency.UpdatePermCurrencyText(permCurrencyAmount);
        SaveDataManager.Instance.SaveCurrencyData();
    }
    public void SetPermCurrencyAmount(int amount) { permCurrencyAmount = amount; }

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
}