using UnityEngine;

public enum SaveDataSlot
{
    save01,
    save02,
    save03,
}

public class SaveDataManager : SingletonMonobehaviour<SaveDataManager>
{
    [Header("Default Data:")]
    [SerializeField] private SOPlayerData playerDataDefault = default;

    [Header("Data Save 01:")]
    [SerializeField] private SOPlayerData playerDataSave01 = default;
    [SerializeField] private SOPlayerUpgradeData upgradeData01 = default;
    public SOPlayerUpgradeData UpgradeData01 => upgradeData01;

    [Header("Data Save 02:")]
    [SerializeField] private SOPlayerData playerDataSave02 = default;

    [Header("Data Save 03:")]
    [SerializeField] private SOPlayerData playerDataSave03 = default;

    //===========================================================================
    private void CheckIfNewSaveSlot(SOPlayerData saveData)
    {
        if (saveData.BaseMaxHealth == 0)
            saveData.TransferData(playerDataDefault);
    }

    //===========================================================================
    public void LoadPlayerDataToRuntimeData(SaveDataSlot saveData)
    {
        switch (saveData)
        {
            case SaveDataSlot.save01:
                CheckIfNewSaveSlot(playerDataSave01);
                PlayerDataManager.Instance.TransferData(playerDataSave01);
                PlayerDataManager.Instance.SetActiveSlot(SaveDataSlot.save01);
                break;
            case SaveDataSlot.save02:
                break;
            case SaveDataSlot.save03:
                break;
        }
    }

    public void SaveRuntimeDataToPlayerDataSlot(SaveDataSlot activeSlot, SOPlayerData runtimeData)
    {
        switch (activeSlot)
        {
            case SaveDataSlot.save01:
                playerDataSave01.TransferData(runtimeData);
                break;
            case SaveDataSlot.save02:
                playerDataSave02.TransferData(runtimeData);
                break;
            case SaveDataSlot.save03:
                playerDataSave03.TransferData(runtimeData);
                break;
        }
    }

    public void SaveCurrencyData() { upgradeData01.UpdateCurrencyAmount(); }

    public void LoadUpgradeSaveData()
    {
        switch (upgradeData01.Tier1Choice)
        {
            case UpgradeChoice.Left:
                UpgradeMenu.Instance.AppliedTier1LeftUpgrade();
                break;
            case UpgradeChoice.Middle:
                UpgradeMenu.Instance.AppliedTier1MiddleUpgrade();
                break;
            case UpgradeChoice.Right:
                UpgradeMenu.Instance.AppliedTier1RightUpgrade();
                break;
            case UpgradeChoice.None:
                break;
        }

        switch (upgradeData01.Tier2Choice)
        {
            case UpgradeChoice.Left:
                UpgradeMenu.Instance.AppliedTier2LeftUpgrade();
                break;
            case UpgradeChoice.Middle:
                UpgradeMenu.Instance.AppliedTier2MiddleUpgrade();
                break;
            case UpgradeChoice.Right:
                UpgradeMenu.Instance.AppliedTier2RightUpgrade();
                break;
            case UpgradeChoice.None:
                break;
        }
    }
}