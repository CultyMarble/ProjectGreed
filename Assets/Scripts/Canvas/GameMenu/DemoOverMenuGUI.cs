using UnityEngine;
using UnityEngine.UI;

public class DemoOverMenuGUI : SingletonMonobehaviour<DemoOverMenuGUI>
{
    [SerializeField] private GameObject content = default;

    [Header("Menu Content")]
    [SerializeField] private Button do_ReturnHubButton = default;
    [SerializeField] private Button do_MainMenuButton = default;

    //===========================================================================
    private void OnEnable()
    {
        do_ReturnHubButton.onClick.AddListener(() =>
        {
            UpgradeMenu.Instance.RemoveCurrentUpgradePath_Tier1Effect(true);
            UpgradeMenu.Instance.RemoveCurrentUpgradePath_Tier2Effect(true);

            DisplayItemUpgradeIcon.Instance.Clear();

            MapGenerator.Instance.ClearMap();

            SceneControlManager.Instance.RespawnPlayerAtHub();
        });

        do_MainMenuButton.onClick.AddListener(() =>
        {
            UpgradeMenu.Instance.RemoveCurrentUpgradePath_Tier1Effect(true);
            UpgradeMenu.Instance.RemoveCurrentUpgradePath_Tier2Effect(true);

            DisplayItemUpgradeIcon.Instance.Clear();

            MapGenerator.Instance.ClearMap();

            SceneControlManager.Instance.LoadMainMenuWrapper();
        });
    }

    //===========================================================================
    public void SetContentActive(bool active)
    {
        content.SetActive(active);

        if (active)
            SceneControlManager.Instance.CurrentGameplayState = GameplayState.Pause;
    }
}