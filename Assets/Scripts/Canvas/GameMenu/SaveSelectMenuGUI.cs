using UnityEngine;
using UnityEngine.UI;

public class SaveSelectMenuGUI : SingletonMonobehaviour<SaveSelectMenuGUI>
{
    [Header("Content")]
    [SerializeField] private GameObject content = default;

    [Header("Menu Content:")]
    [SerializeField] private Button ss_save01Button = default;
    [SerializeField] private Button ss_save02Button = default;
    [SerializeField] private Button ss_save03Button = default;
    [SerializeField] private Button ss_backButton = default;

    //===========================================================================
    private void OnEnable()
    {
        ss_save01Button.onClick.AddListener(() =>
        {
            SaveDataManager.Instance.LoadPlayerDataToRuntimeData(SaveDataSlot.save01);
            SceneControlManager.Instance.LoadStartingSceneWrapper();

            SetActive(false);
        });

        ss_save02Button.onClick.AddListener(() =>
        {
            SaveDataManager.Instance.LoadPlayerDataToRuntimeData(SaveDataSlot.save02);
            SceneControlManager.Instance.LoadStartingSceneWrapper();

            SetActive(false);
        });

        ss_save03Button.onClick.AddListener(() =>
        {
            SaveDataManager.Instance.LoadPlayerDataToRuntimeData(SaveDataSlot.save03);
            SceneControlManager.Instance.LoadStartingSceneWrapper();

            SetActive(false);
        });

        ss_backButton.onClick.AddListener(() =>
        {
            this.SetActive(false);

            MainMenuGUI.Instance.SetActive(true);
        });
    }

    //===========================================================================
    public void SetActive(bool active)
    {
        content.SetActive(active);
    }
}