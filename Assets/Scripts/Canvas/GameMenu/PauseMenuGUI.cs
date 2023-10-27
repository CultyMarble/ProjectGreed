using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuGUI : MonoBehaviour
{
    [Header("Pause Menu")]
    [SerializeField] private GameObject pm_BGIMage = default;
    [SerializeField] private GameObject pm_Text = default;
    [SerializeField] private Button pm_AbandonRunButton = default;
    [SerializeField] private Button pm_MainMenuButton = default;

    private GameState pm_priorGameState = default;

    //===========================================================================
    private void OnEnable()
    {
        // Pause Menu
        pm_AbandonRunButton.onClick.AddListener(() => SceneControlManager.Instance.RespawnPlayerAtHub());
        pm_MainMenuButton.onClick.AddListener(() => SceneControlManager.Instance.BackToMainMenuWrapper());
    }

    private void Update()
    {
        if (SceneControlManager.Instance.IsLoadingScreenActive)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (SceneControlManager.Instance.CurrentGameState)
            {
                case GameState.GameplayHub:
                    SetPauseMenuActive(true);
                    pm_priorGameState = SceneControlManager.Instance.CurrentGameState;
                    SceneControlManager.Instance.CurrentGameState = GameState.GameplayMenu;
                    break;
                case GameState.GameplayDungeon:
                    SetPauseMenuActive(true);
                    pm_priorGameState = SceneControlManager.Instance.CurrentGameState;
                    SceneControlManager.Instance.CurrentGameState = GameState.GameplayMenu;
                    break;
                case GameState.GameplayMenu:
                    SetPauseMenuActive(false);
                    SceneControlManager.Instance.CurrentGameState = GameState.GameplayHub;
                    break;
                default:
                    break;
            }
        }
    }

    //===========================================================================
    public void SetPauseMenuActive(bool newBool)
    {
        if (newBool)
        {
            pm_Text.SetActive(newBool);
            pm_BGIMage.SetActive(newBool);

            if (SceneControlManager.Instance.CurrentGameState == GameState.GameplayDungeon)
                pm_AbandonRunButton.gameObject.SetActive(newBool);

            pm_MainMenuButton.gameObject.SetActive(newBool);
        }
        else
        {
            SceneControlManager.Instance.CurrentGameState = pm_priorGameState;

            pm_Text.SetActive(newBool);
            pm_BGIMage.SetActive(newBool);
            pm_AbandonRunButton.gameObject.SetActive(newBool);
            pm_MainMenuButton.gameObject.SetActive(newBool);
        }
    }
}