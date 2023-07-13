using UnityEngine;

public class GameplayInfoUIControl : SingletonMonobehaviour<GameplayInfoUIControl>
{
    [SerializeField] private PlayerHealth playerHealth;

    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject playerCurrencies;
    [SerializeField] private GameObject playerAbilityCoolDown;

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
        playerUI.SetActive(newBool);
        playerCurrencies.SetActive(newBool);
        playerAbilityCoolDown.SetActive(newBool);
    }
}
