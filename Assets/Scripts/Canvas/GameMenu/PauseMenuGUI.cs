using UnityEngine;
using UnityEngine.UI;

public class PauseMenuGUI : SingletonMonobehaviour<PauseMenuGUI>
{
    [SerializeField] private GameObject content = default;

    [Header("Menu Content:")]
    [SerializeField] private Button pm_OptionsMenuButton = default;
    [SerializeField] private Button pm_AbandonRunButton = default;
    [SerializeField] private Button pm_MainMenuButton = default;

    //===========================================================================
    private void OnEnable()
    {
        pm_OptionsMenuButton.onClick.AddListener(() => OptionMenuGUI.Instance.SetContentActive(true));
        pm_AbandonRunButton.onClick.AddListener(() => SceneControlManager.Instance.RespawnPlayerAtHub());
        pm_MainMenuButton.onClick.AddListener(() => SceneControlManager.Instance.LoadMainMenuWrapper());
    }

    private void Update()
    {
        if (SceneControlManager.Instance.IsLoadingScreenActive)
            return;

        if (SceneControlManager.Instance.CurrentActiveScene == SceneName.MainMenu)
            return;

        if (GameOverMenuGUI.Instance.Content.activeInHierarchy)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (OptionMenuGUI.Instance.Content.activeInHierarchy)
                return;

            SetContentActive(!content.gameObject.activeInHierarchy);

            if (content.gameObject.activeInHierarchy == true)
            {
                SceneControlManager.Instance.CurrentGameplayState = GameplayState.Pause;
            }
            else
            {
                SceneControlManager.Instance.CurrentGameplayState = GameplayState.Ongoing;
            }
        }
    }

    //===========================================================================
    public void SetContentActive(bool active)
    {
        content.SetActive(active);

        if (active)
        {
            if (SceneControlManager.Instance.CurrentActiveScene == SceneName.DemoSceneHub)
            {
                pm_AbandonRunButton.gameObject.SetActive(false);
            }
            else if (SceneControlManager.Instance.CurrentActiveScene == SceneName.DemoSceneDungeon ||
                SceneControlManager.Instance.CurrentActiveScene == SceneName.DemoSceneBossRoom)
            {
                pm_AbandonRunButton.gameObject.SetActive(true);
            }
        }
    }
}