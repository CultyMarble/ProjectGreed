using UnityEngine;
using UnityEngine.UI;

public class OptionMenuGUI : SingletonMonobehaviour<OptionMenuGUI>
{
    [SerializeField] private GameObject content = default;
    public GameObject Content => content;

    [Header("Menu Content:")]
    [SerializeField] private Button om_BackButton = default;

    //===========================================================================
    void OnEnable()
    {
        om_BackButton.onClick.AddListener(() =>
        {
            SetContentActive(false);
            PauseMenuGUI.Instance.SetContentActive(true);
        });
    }

    private void Update()
    {
        if (SceneControlManager.Instance.IsLoadingScreenActive)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetContentActive(false);
        }
    }

    //===========================================================================
    public void SetContentActive(bool active)
    {
        content.SetActive(active);
    }
}