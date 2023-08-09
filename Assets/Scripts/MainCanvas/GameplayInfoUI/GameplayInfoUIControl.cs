using UnityEngine;

public class GameplayInfoUIControl : SingletonMonobehaviour<GameplayInfoUIControl>
{
    [SerializeField] private PlayerHealth playerHealth;

    [SerializeField] private GameObject playerHeartUI;
    [SerializeField] private GameObject playerFuelUI;
    [SerializeField] private GameObject playerCurrenciesUI;
    [SerializeField] private GameObject playerAbilityCoolDownUI;

    //===========================================================================
    private void OnEnable()
    {
        playerHealth.OnDespawnEvent += PlayerHealth_OnDespawnEventHandler;
    }

    private void OnDisable()
    {
        playerHealth.OnDespawnEvent -= PlayerHealth_OnDespawnEventHandler;
    }

    //===========================================================================
    private void PlayerHealth_OnDespawnEventHandler(object sender, System.EventArgs e)
    {
        SetGameplayInfoUIActive(false);
    }

    //===========================================================================
    public void SetGameplayInfoUIActive(bool newBool)
    {
        playerHeartUI.SetActive(newBool);
        playerFuelUI.SetActive(newBool);
        playerCurrenciesUI.SetActive(newBool);
        playerAbilityCoolDownUI.SetActive(newBool);
    }
}
