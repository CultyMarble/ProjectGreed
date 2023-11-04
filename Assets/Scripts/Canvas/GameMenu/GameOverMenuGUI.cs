using UnityEngine;

public class GameOverMenuGUI : SingletonMonobehaviour<GameOverMenuGUI>
{
    [SerializeField] private PlayerHeart playerHeartManager;
    [SerializeField] private GameObject content;


    //===========================================================================
    private void OnEnable()
    {
        playerHeartManager.OnDespawnPlayerEvent += DisplayGameOverUI_OnDespawnEventHandler;
    }

    private void OnDisable()
    {
        playerHeartManager.OnDespawnPlayerEvent -= DisplayGameOverUI_OnDespawnEventHandler;
    }

    //===========================================================================
    private void DisplayGameOverUI_OnDespawnEventHandler(object sender, System.EventArgs e)
    {
        if (content.activeSelf)
            return;

        SetActive(true);
    }

    public void SetActive(bool active)
    {
        content.SetActive(active);
    }
}