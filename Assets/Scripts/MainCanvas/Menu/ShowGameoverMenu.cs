using UnityEngine;

public class ShowGameoverMenu : MonoBehaviour
{
    [SerializeField] private PlayerHeartManager playerHeart;
    [SerializeField] private GameObject gameOverMenu;

    //===========================================================================
    private void OnEnable()
    {
        playerHeart.OnDespawnEvent += DisplayGameOverUI_OnDespawnEventHandler; ;
    }

    private void OnDisable()
    {
        playerHeart.OnDespawnEvent -= DisplayGameOverUI_OnDespawnEventHandler; ;
    }

    //===========================================================================
    private void DisplayGameOverUI_OnDespawnEventHandler(object sender, System.EventArgs e)
    {
        if (gameOverMenu.activeSelf)
            return;

        gameOverMenu.SetActive(true);
    }
}