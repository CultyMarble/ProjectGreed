using UnityEngine;
using TMPro;

public class DisplayPlayerCurrency : SingletonMonobehaviour<DisplayPlayerCurrency>
{
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI tempCurrencyText;
    [SerializeField] private TextMeshProUGUI permCurrencyText;

    //===========================================================================
    private void OnEnable()
    {
        EventManager.BeforeSceneUnloadEvent += EventManager_BeforeSceneUnloadEventHandler;

        EventManager.AfterSceneLoadEvent += EventManager_AfterSceneLoadEventHandler;

        player.GetComponent<PlayerCurrencies>().OnTempCurrencyAmountChanged += Instance_OnTempCurrencyAmountChangedHandler;
        player.GetComponent<PlayerCurrencies>().OnPermCurrencyAmountChanged += Instance_OnPermCurrencyAmountChangedHandler;
    }

    private void OnDisable()
    {
        EventManager.BeforeSceneUnloadEvent -= EventManager_BeforeSceneUnloadEventHandler;

        EventManager.AfterSceneLoadEvent -= EventManager_AfterSceneLoadEventHandler;
    }

    //===========================================================================
    private void EventManager_BeforeSceneUnloadEventHandler()
    {
        return;
    }

    private void EventManager_AfterSceneLoadEventHandler()
    {
        UpdateTempCurrencyText();
        UpdatePermCurrencyText();
    }

    private void Instance_OnTempCurrencyAmountChangedHandler(object sender, PlayerCurrencies.OnTempCurrencyAmountChangedEventArgs e)
    {
        UpdateTempCurrencyText();
    }

    private void Instance_OnPermCurrencyAmountChangedHandler(object sender, PlayerCurrencies.OnPermCurrencyAmountChangedEventArgs e)
    {
        UpdatePermCurrencyText();
    }

    public void UpdateTempCurrencyText()
    {
        tempCurrencyText.SetText(player.GetComponent<PlayerCurrencies>().GetTempCurrencyAmount().ToString());
    }

    public void UpdatePermCurrencyText()
    {
        permCurrencyText.SetText(player.GetComponent<PlayerCurrencies>().GetPermCurrencyAmount().ToString());
    }
}
