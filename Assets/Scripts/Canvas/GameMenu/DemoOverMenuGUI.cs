using UnityEngine;
using UnityEngine.UI;

public class DemoOverMenuGUI : SingletonMonobehaviour<DemoOverMenuGUI>
{
    [SerializeField] private GameObject content;
    [SerializeField] private Button do_ReturnButton = default;
    private BossCheck bossCheck;

    //===========================================================================
    private void OnEnable()
    {
        bossCheck = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossCheck>();
        bossCheck.OnDespawnBossEvent += DisplayDemoOverUI_OnDespawnEventHandler;
        do_ReturnButton.onClick.AddListener(() =>
        {
            SetMenuActive(false);
            SceneControlManager.Instance.RespawnPlayerAtHub();
        });
    }

    //===========================================================================
    public void SetMenuActive(bool newBool)
    {
        if (content.activeSelf)
            return;
        content.SetActive(true);
        do_ReturnButton.gameObject.SetActive(newBool);
    }
    private void DisplayDemoOverUI_OnDespawnEventHandler(object sender, System.EventArgs e)
    {
        if (content.activeSelf)
            return;

        SetMenuActive(true);
    }
    
}