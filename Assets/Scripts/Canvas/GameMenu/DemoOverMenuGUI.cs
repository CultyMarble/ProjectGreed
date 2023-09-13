using UnityEngine;
using UnityEngine.UI;

public class DemoOverMenuGUI : SingletonMonobehaviour<DemoOverMenuGUI>
{
    [SerializeField] private GameObject do_BGImage = default;
    [SerializeField] private GameObject do_Text = default;
    [SerializeField] private GameObject do_ContentText = default;
    [SerializeField] private Button do_ReturnButton = default;

    //===========================================================================
    private void OnEnable()
    {
        do_ReturnButton.onClick.AddListener(() =>
        {
            SetMenuActive(false);
            SceneControlManager.Instance.RespawnPlayerAtHub();
        });
    }

    //===========================================================================
    public void SetMenuActive(bool newBool)
    {
        do_BGImage.SetActive(newBool);
        do_Text.SetActive(newBool);
        do_ContentText.SetActive(newBool);
        do_ReturnButton.gameObject.SetActive(newBool);
    }
}