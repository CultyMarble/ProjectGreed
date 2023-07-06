using UnityEngine;

public class ShowGameoverMenu : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject gameOverMenu;

    //===========================================================================
    private void OnEnable()
    {
        playerHealth.OnDespawnEvent += DisplayGameOverUI_OnDespawnEventHandler; ;
    }

    private void OnDisable()
    {
        playerHealth.OnDespawnEvent -= DisplayGameOverUI_OnDespawnEventHandler; ;
    }

    //===========================================================================
    private void DisplayGameOverUI_OnDespawnEventHandler(object sender, System.EventArgs e)
    {
        if (gameOverMenu.activeSelf)
            return;

        gameOverMenu.SetActive(true);
    }
}