using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SaveSelectMenuGUI : SingletonMonobehaviour<SaveSelectMenuGUI>
{
    [SerializeField] private Button ss_save01Button = default;
    [SerializeField] private Button ss_save02Button = default;
    [SerializeField] private Button ss_save03Button = default;

    //===========================================================================
    protected override void Awake()
    {
        base.Awake();

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
    }

    //===========================================================================
    public void SetActive(bool newBool)
    {
        ss_save01Button.gameObject.SetActive(newBool);
        ss_save02Button.gameObject.SetActive(newBool);
        ss_save03Button.gameObject.SetActive(newBool);
    }
}
