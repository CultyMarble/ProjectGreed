using UnityEngine;

public class GameplayInfoController : SingletonMonobehaviour<GameplayInfoController>
{
    [SerializeField] private PlayerHeart playerHeartManager;

    [SerializeField] private GameObject playerHeartUI;
    [SerializeField] private GameObject playerFuelUI;
    [SerializeField] private GameObject playerRangeAbilityChargeUI;
    [SerializeField] private GameObject playerBombAbilityChargeUI;
    [SerializeField] private GameObject playerCurrencyUI;

    [SerializeField] private DisplayPlayerHeart displayPlayerHeart = default;
    [SerializeField] private DisplayPlayerFuel displayPlayerFuel = default;
    [SerializeField] private DisplayPlayerRangeAbilityCharge displayPlayerRangeAbilityCharge = default;
    [SerializeField] private DisplayPlayerBombAbilityCharge displayPlayerBombAbilityCharge = default;
    [SerializeField] private DisplayPlayerCurrency displayPlayerCurrency = default;

    public DisplayPlayerHeart DisplayPlayerHeart => displayPlayerHeart;
    public DisplayPlayerFuel DisplayPlayerFuel => displayPlayerFuel;
    public DisplayPlayerRangeAbilityCharge DisplayPlayerRangeAbilityCharge => displayPlayerRangeAbilityCharge;
    public DisplayPlayerBombAbilityCharge DisplayPlayerBombAbilityCharge => displayPlayerBombAbilityCharge;
    public DisplayPlayerCurrency DisplayPlayerCurrency => displayPlayerCurrency;

    //===========================================================================
    private void OnEnable()
    {
        playerHeartManager.OnDespawnPlayerEvent += PlayerHealth_OnDespawnEventHandler;

        EventManager.BeforeSceneUnloadEvent += EventManager_BeforeSceneUnloadEvent;
        EventManager.AfterSceneLoadEvent += EventManager_AfterSceneLoadEvent;
    }

    private void OnDisable()
    {
        playerHeartManager.OnDespawnPlayerEvent -= PlayerHealth_OnDespawnEventHandler;

        EventManager.BeforeSceneUnloadEvent -= EventManager_BeforeSceneUnloadEvent;
        EventManager.AfterSceneLoadEvent -= EventManager_AfterSceneLoadEvent;
    }

    //===========================================================================
    private void PlayerHealth_OnDespawnEventHandler(object sender, System.EventArgs e)
    {
        SetGameplayInfoUIActive(false);
    }

    private void EventManager_BeforeSceneUnloadEvent()
    {
        SetGameplayInfoUIActive(false);
    }

    private void EventManager_AfterSceneLoadEvent()
    {
        SetGameplayInfoUIActive(true);
    }

    //===========================================================================
    public void SetGameplayInfoUIActive(bool newBool)
    {
        playerHeartUI.SetActive(newBool);
        playerFuelUI.SetActive(newBool);
        playerRangeAbilityChargeUI.SetActive(newBool);
        playerBombAbilityChargeUI.SetActive(newBool);
        playerCurrencyUI.SetActive(newBool);
    }
}