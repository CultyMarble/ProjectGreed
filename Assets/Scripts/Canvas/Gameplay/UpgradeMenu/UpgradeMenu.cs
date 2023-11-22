using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : SingletonMonobehaviour<UpgradeMenu>
{
    [SerializeField] private GameObject um_BGImage = default;

    [Header("Tier 1 Upgrade")]
    [SerializeField] private Button um_Tier1UpgradeLeftButton = default;
    [SerializeField] private Button um_Tier1UpgradeMiddleButton = default;
    [SerializeField] private Button um_Tier1UpgradeRightButton = default;

    [Header("Tier 2 Upgrade")]
    [SerializeField] private Button um_Tier2UpgradeLeftButton = default;
    [SerializeField] private Button um_Tier2UpgradeMiddleButton = default;
    [SerializeField] private Button um_Tier2UpgradeRightButton = default;

    private UpgradePath currentUpgradePathTier1 = UpgradePath.none;
    private UpgradePath currentUpgradePathTier2 = UpgradePath.none;

    //===========================================================================
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Home))
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
        // Tier 1
        um_Tier1UpgradeLeftButton.onClick.AddListener(() =>
        {
            RemoveCurrentUpgradePath_Tier1Effect();
            um_Tier1UpgradeLeftButton.GetComponent<UpgradeMenuButton>().AppliedEffect();
            um_Tier1UpgradeLeftButton.GetComponent<Image>().enabled = true;
            currentUpgradePathTier1 = UpgradePath.Left;
        });

        um_Tier1UpgradeMiddleButton.onClick.AddListener(() =>
        {
            RemoveCurrentUpgradePath_Tier1Effect();
            um_Tier1UpgradeMiddleButton.GetComponent<UpgradeMenuButton>().AppliedEffect();
            um_Tier1UpgradeMiddleButton.GetComponent<Image>().enabled = true;
            currentUpgradePathTier1 = UpgradePath.Middle;
        });

        um_Tier1UpgradeRightButton.onClick.AddListener(() =>
        {
            RemoveCurrentUpgradePath_Tier1Effect();
            um_Tier1UpgradeRightButton.GetComponent<UpgradeMenuButton>().AppliedEffect();
            um_Tier1UpgradeRightButton.GetComponent<Image>().enabled = true;
            currentUpgradePathTier1 = UpgradePath.Right;
        });

        // Tier 2
        um_Tier2UpgradeLeftButton.onClick.AddListener(() =>
        {
            RemoveCurrentUpgradePath_Tier2Effect();
            um_Tier2UpgradeLeftButton.GetComponent<UpgradeMenuButton>().AppliedEffect();
            um_Tier2UpgradeLeftButton.GetComponent<Image>().enabled = true;
            currentUpgradePathTier2 = UpgradePath.Left;
        });

        um_Tier2UpgradeMiddleButton.onClick.AddListener(() =>
        {
            RemoveCurrentUpgradePath_Tier2Effect();
            um_Tier2UpgradeMiddleButton.GetComponent<UpgradeMenuButton>().AppliedEffect();
            um_Tier2UpgradeMiddleButton.GetComponent<Image>().enabled = true;
            currentUpgradePathTier2 = UpgradePath.Middle;
        });

        um_Tier2UpgradeRightButton.onClick.AddListener(() =>
        {
            RemoveCurrentUpgradePath_Tier2Effect();
            um_Tier2UpgradeRightButton.GetComponent<UpgradeMenuButton>().AppliedEffect();
            um_Tier2UpgradeRightButton.GetComponent<Image>().enabled = true;
            currentUpgradePathTier2 = UpgradePath.Right;
        });
    }

    //===========================================================================
    private void RemoveCurrentUpgradePath_Tier1Effect()
    {
        switch (currentUpgradePathTier1)
        {
            case UpgradePath.Left:
                um_Tier1UpgradeLeftButton.GetComponent<UpgradeMenuButton>().RemoveEffect();
                um_Tier1UpgradeLeftButton.GetComponent<Image>().enabled = false;
                currentUpgradePathTier1 = UpgradePath.none;
                break;
            case UpgradePath.Middle:
                um_Tier1UpgradeMiddleButton.GetComponent<UpgradeMenuButton>().RemoveEffect();
                um_Tier1UpgradeMiddleButton.GetComponent<Image>().enabled = false;
                currentUpgradePathTier1 = UpgradePath.none;
                break;
            case UpgradePath.Right:
                um_Tier1UpgradeRightButton.GetComponent<UpgradeMenuButton>().RemoveEffect();
                um_Tier1UpgradeRightButton.GetComponent<Image>().enabled = false;
                currentUpgradePathTier1 = UpgradePath.none;
                break;
            case UpgradePath.none:
                break;
        }
    }

    private void RemoveCurrentUpgradePath_Tier2Effect()
    {
        switch (currentUpgradePathTier2)
        {
            case UpgradePath.Left:
                um_Tier2UpgradeLeftButton.GetComponent<UpgradeMenuButton>().RemoveEffect();
                um_Tier2UpgradeLeftButton.GetComponent<Image>().enabled = false;
                currentUpgradePathTier2 = UpgradePath.none;
                break;
            case UpgradePath.Middle:
                um_Tier2UpgradeMiddleButton.GetComponent<UpgradeMenuButton>().RemoveEffect();
                um_Tier2UpgradeMiddleButton.GetComponent<Image>().enabled = false;
                currentUpgradePathTier2 = UpgradePath.none;
                break;
            case UpgradePath.Right:
                um_Tier2UpgradeRightButton.GetComponent<UpgradeMenuButton>().RemoveEffect();
                um_Tier2UpgradeRightButton.GetComponent<Image>().enabled = false;
                currentUpgradePathTier2 = UpgradePath.none;
                break;
            case UpgradePath.none:
                break;
        }
    }

    //===========================================================================
    public void SetActive(bool newBool)
    {
        um_BGImage.SetActive(newBool);
    }
}