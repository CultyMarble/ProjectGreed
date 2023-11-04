using UnityEngine;
using UnityEngine.UI;

public class OptionMenuGUI : SingletonMonobehaviour<OptionMenuGUI>
{
    [SerializeField] private GameObject content = default;

    [Header("Menu Content:")]
    [SerializeField] private Button om_BackButton = default;

    //===========================================================================
    void OnEnable()
    {
        om_BackButton.onClick.AddListener(() =>
        {
            SceneControlManager.Instance.GameState = GameState.PauseMenu;
            SetActive(false);
        });
    }

    private void Update()
    {
        if (SceneControlManager.Instance.IsLoadingScreenActive)
            return;

        if (SceneControlManager.Instance.GameState == GameState.MainMenu)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneControlManager.Instance.GameState != GameState.OptionMenu)
                return;

            SetActive(false);
        }
    }

    //===========================================================================
    public void SetActive(bool active)
    {
        content.SetActive(active);

        if (active)
        {
            SceneControlManager.Instance.GameState = GameState.OptionMenu;
        }
        else
        {
            SceneControlManager.Instance.GameState = GameState.PauseMenu;
        }

    }
}