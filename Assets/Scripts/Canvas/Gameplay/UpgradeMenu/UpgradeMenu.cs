using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : SingletonMonobehaviour<UpgradeMenu>
{
    [SerializeField] private GameObject um_BGImage = default;

    [SerializeField] private Button um_Tier1UpgradeLeftButton = default;
    [SerializeField] private Button um_Tier1UpgradeMiddleButton = default;
    [SerializeField] private Button um_Tier1UpgradeRightButton = default;
    private UpgradePath currentUpgradePath = UpgradePath.none;

    //===========================================================================
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SetActive(true);
            SceneControlManager.Instance.CurrentGameplayState = GameplayState.Pause;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetActive(false);
            SceneControlManager.Instance.CurrentGameplayState = GameplayState.Ongoing;
        }
    }

    //===========================================================================
    private void OnEnable()
    {
        um_Tier1UpgradeLeftButton.onClick.AddListener(() =>
        {
            RemoveCurrentUpgradePathEffect();
            um_Tier1UpgradeLeftButton.GetComponent<UpgradeMenuButton>().AppliedEffect();
            um_Tier1UpgradeLeftButton.GetComponent<Image>().enabled = true;
            currentUpgradePath = UpgradePath.Left;
        });

        um_Tier1UpgradeMiddleButton.onClick.AddListener(() =>
        {
            RemoveCurrentUpgradePathEffect();
            um_Tier1UpgradeMiddleButton.GetComponent<UpgradeMenuButton>().AppliedEffect();
            um_Tier1UpgradeMiddleButton.GetComponent<Image>().enabled = true;
            currentUpgradePath = UpgradePath.Middle;
        });

        um_Tier1UpgradeRightButton.onClick.AddListener(() =>
        {
            RemoveCurrentUpgradePathEffect();
            um_Tier1UpgradeRightButton.GetComponent<UpgradeMenuButton>().AppliedEffect();
            um_Tier1UpgradeRightButton.GetComponent<Image>().enabled = true;
            currentUpgradePath = UpgradePath.Right;
        });
    }

    //===========================================================================
    private void RemoveCurrentUpgradePathEffect()
    {
        switch (currentUpgradePath)
        {
            case UpgradePath.Left:
                um_Tier1UpgradeLeftButton.GetComponent<UpgradeMenuButton>().RemoveEffect();
                um_Tier1UpgradeLeftButton.GetComponent<Image>().enabled = false;
                currentUpgradePath = UpgradePath.none;
                break;
            case UpgradePath.Middle:
                um_Tier1UpgradeMiddleButton.GetComponent<UpgradeMenuButton>().RemoveEffect();
                um_Tier1UpgradeMiddleButton.GetComponent<Image>().enabled = false;
                currentUpgradePath = UpgradePath.none;
                break;
            case UpgradePath.Right:
                um_Tier1UpgradeRightButton.GetComponent<UpgradeMenuButton>().RemoveEffect();
                um_Tier1UpgradeRightButton.GetComponent<Image>().enabled = false;
                currentUpgradePath = UpgradePath.none;
                break;
            case UpgradePath.none:
                break;
        }
    }

    //===========================================================================
    public void SetActive(bool newBool)
    {
        um_BGImage.SetActive(newBool);

        um_Tier1UpgradeLeftButton.gameObject.SetActive(newBool);
        um_Tier1UpgradeMiddleButton.gameObject.SetActive(newBool);
        um_Tier1UpgradeRightButton.gameObject.SetActive(newBool);
    }
}