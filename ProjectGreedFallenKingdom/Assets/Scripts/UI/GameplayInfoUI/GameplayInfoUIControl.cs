using UnityEngine;

public class GameplayInfoUIControl : SingletonMonobehaviour<GameplayInfoUIControl>
{
    [SerializeField] private GameObject playerHealthUI;
    [SerializeField] private GameObject playerCurrencies;
    [SerializeField] private GameObject playerAbilityCoolDown;

    //===========================================================================
    public void SetGameplayInfoUIActive(bool newBool)
    {
        playerHealthUI.SetActive(newBool);
        playerCurrencies.SetActive(newBool);
        playerAbilityCoolDown.SetActive(newBool);
    }
}
