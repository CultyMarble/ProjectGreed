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
    [SerializeField] private SOPlayerData playerDataSave02 = default;
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
                CheckIfNewSaveSlot(playerDataSave02);
                PlayerDataManager.Instance.TransferData(playerDataSave02);
                PlayerDataManager.Instance.SetActiveSlot(SaveDataSlot.save02);
                break;
            case SaveDataSlot.save03:
                CheckIfNewSaveSlot(playerDataSave03);
                PlayerDataManager.Instance.TransferData(playerDataSave03);
                PlayerDataManager.Instance.SetActiveSlot(SaveDataSlot.save03);
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
}