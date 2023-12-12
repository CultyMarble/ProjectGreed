using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : SingletonMonobehaviour<UpgradeMenu>
{
    [SerializeField] private GameObject um_BGImage = default;
    [SerializeField] private Button um_CloseButton = default;

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

    [Header("Ability Info Panel")]
    [SerializeField] private Transform upgradeInfoPanel = default;
    [SerializeField] private TextMeshProUGUI upgradeNameText = default;
    [SerializeField] private TextMeshProUGUI upgradeDescriptionText = default;

    //===========================================================================
    private void OnEnable()
    {
        um_CloseButton.onClick.AddListener(() =>
        {
            um_BGImage.SetActive(false);
            SceneControlManager.Instance.CurrentGameplayState = GameplayState.Ongoing;
        });

        // Tier 1
        um_Tier1UpgradeLeftButton.onClick.AddListener(() =>
        {
            if (currentUpgradePathTier1 == UpgradePath.Left)
            {
                RemoveCurrentUpgradePath_Tier1Effect(true);
                PlayerCurrencies.Instance.UpdatePermCurrencyAmount(1);
                return;
            }

            if (PlayerCurrencies.Instance.PermCurrencyAmount >= 1)
            {
                if (currentUpgradePathTier1 == UpgradePath.none)
                    PlayerCurrencies.Instance.UpdatePermCurrencyAmount(-1);

                AppliedTier1LeftUpgrade();
            }
        });

        um_Tier1UpgradeMiddleButton.onClick.AddListener(() =>
        {
            if (currentUpgradePathTier1 == UpgradePath.Middle)
            {
                RemoveCurrentUpgradePath_Tier1Effect(true);
                PlayerCurrencies.Instance.UpdatePermCurrencyAmount(1);
                return;
            }

            if (PlayerCurrencies.Instance.PermCurrencyAmount >= 1)
            {
                if (currentUpgradePathTier1 == UpgradePath.none)
                    PlayerCurrencies.Instance.UpdatePermCurrencyAmount(-1);

                AppliedTier1MiddleUpgrade();
            }
        });

        um_Tier1UpgradeRightButton.onClick.AddListener(() =>
        {
            if (currentUpgradePathTier1 == UpgradePath.Right)
            {
                RemoveCurrentUpgradePath_Tier1Effect(true);
                PlayerCurrencies.Instance.UpdatePermCurrencyAmount(1);
                return;
            }

            if (PlayerCurrencies.Instance.PermCurrencyAmount >= 1)
            {
                if (currentUpgradePathTier1 == UpgradePath.none)
                    PlayerCurrencies.Instance.UpdatePermCurrencyAmount(-1);

                AppliedTier1RightUpgrade();
            }
        });

        // Tier 2
        um_Tier2UpgradeLeftButton.onClick.AddListener(() =>
        {
            if (currentUpgradePathTier2 == UpgradePath.Left)
            {
                RemoveCurrentUpgradePath_Tier2Effect(true);
                PlayerCurrencies.Instance.UpdatePermCurrencyAmount(2);
                return;
            }

            if (PlayerCurrencies.Instance.PermCurrencyAmount >= 2)
            {
                if (currentUpgradePathTier2 == UpgradePath.none)
                    PlayerCurrencies.Instance.UpdatePermCurrencyAmount(-2);

                AppliedTier2LeftUpgrade();
            }
        });

        um_Tier2UpgradeMiddleButton.onClick.AddListener(() =>
        {
            if (currentUpgradePathTier2 == UpgradePath.Middle)
            {
                RemoveCurrentUpgradePath_Tier2Effect(true);
                PlayerCurrencies.Instance.UpdatePermCurrencyAmount(2);
                return;
            }

            if (PlayerCurrencies.Instance.PermCurrencyAmount >= 2)
            {
                if (currentUpgradePathTier2 == UpgradePath.none)
                    PlayerCurrencies.Instance.UpdatePermCurrencyAmount(-2);

                AppliedTier2MiddleUpgrade();
            }
        });

        um_Tier2UpgradeRightButton.onClick.AddListener(() =>
        {
            if (currentUpgradePathTier2 == UpgradePath.Right)
            {
                RemoveCurrentUpgradePath_Tier2Effect(true);
                PlayerCurrencies.Instance.UpdatePermCurrencyAmount(2);
                return;
            }

            if (PlayerCurrencies.Instance.PermCurrencyAmount >= 2)
            {
                if (currentUpgradePathTier2 == UpgradePath.none)
                    PlayerCurrencies.Instance.UpdatePermCurrencyAmount(-2);

                AppliedTier2RightUpgrade();
            }
        });
    }

    private void Update()
    {
        if(um_BGImage.activeSelf == false)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetContentActive(false);
            SceneControlManager.Instance.CurrentGameplayState = GameplayState.Ongoing;
        }
    }

    //===========================================================================
    public void AppliedTier1LeftUpgrade()
    {
        um_Tier1UpgradeLeftButton.GetComponent<UpgradeMenuButton>().AppliedEffect();
        um_Tier1UpgradeLeftButton.GetComponent<Image>().enabled = true;
        currentUpgradePathTier1 = UpgradePath.Left;
        SaveDataManager.Instance.UpgradeData01.SetTier1Choice(UpgradeChoice.Left);
    }

    public void AppliedTier1MiddleUpgrade()
    {
        um_Tier1UpgradeMiddleButton.GetComponent<UpgradeMenuButton>().AppliedEffect();
        um_Tier1UpgradeMiddleButton.GetComponent<Image>().enabled = true;
        currentUpgradePathTier1 = UpgradePath.Middle;
        SaveDataManager.Instance.UpgradeData01.SetTier1Choice(UpgradeChoice.Middle);
    }

    public void AppliedTier1RightUpgrade()
    {
        um_Tier1UpgradeRightButton.GetComponent<UpgradeMenuButton>().AppliedEffect();
        um_Tier1UpgradeRightButton.GetComponent<Image>().enabled = true;
        currentUpgradePathTier1 = UpgradePath.Right;
        SaveDataManager.Instance.UpgradeData01.SetTier1Choice(UpgradeChoice.Right);
    }

    public void AppliedTier2LeftUpgrade()
    {
        um_Tier2UpgradeLeftButton.GetComponent<UpgradeMenuButton>().AppliedEffect();
        um_Tier2UpgradeLeftButton.GetComponent<Image>().enabled = true;
        currentUpgradePathTier2 = UpgradePath.Left;
        SaveDataManager.Instance.UpgradeData01.SetTier2Choice(UpgradeChoice.Left);
    }

    public void AppliedTier2MiddleUpgrade()
    {
        um_Tier2UpgradeMiddleButton.GetComponent<UpgradeMenuButton>().AppliedEffect();
        um_Tier2UpgradeMiddleButton.GetComponent<Image>().enabled = true;
        currentUpgradePathTier2 = UpgradePath.Middle;
        SaveDataManager.Instance.UpgradeData01.SetTier2Choice(UpgradeChoice.Middle);
    }

    public void AppliedTier2RightUpgrade()
    {
        um_Tier2UpgradeRightButton.GetComponent<UpgradeMenuButton>().AppliedEffect();
        um_Tier2UpgradeRightButton.GetComponent<Image>().enabled = true;
        currentUpgradePathTier2 = UpgradePath.Right;
        SaveDataManager.Instance.UpgradeData01.SetTier2Choice(UpgradeChoice.Right);
    }

    public void RemoveCurrentUpgradePath_Tier1Effect(bool removeSave)
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

        if (removeSave)
            SaveDataManager.Instance.UpgradeData01.SetTier1Choice(UpgradeChoice.None);
    }

    public void RemoveCurrentUpgradePath_Tier2Effect(bool removeSave)
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

        if (removeSave)
            SaveDataManager.Instance.UpgradeData01.SetTier2Choice(UpgradeChoice.None);
    }

    //===========================================================================
    public void SetContentActive(bool newBool)
    {
        um_BGImage.SetActive(newBool);
    }

    public void SetUpgradeInfoPanelActive(bool active)
    {
        upgradeInfoPanel.gameObject.SetActive(active);
    }

    public void SetUpgradeNameText(string text)
    {
        upgradeNameText.SetText(text);
    }

    public void SetUpgradeDescriptionText(string text)
    {
        upgradeDescriptionText.SetText(text);
    }
}