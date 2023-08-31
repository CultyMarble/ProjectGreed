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
    private void OnDisable()
    {
        playerDataSave01.ClearData();
    }

    //===========================================================================
    public void LoadPlayerDataToRuntimeData(SaveDataSlot saveData)
    {
        playerDataSave01.TransferData(playerDataDefault);

        switch (saveData)
        {
            case SaveDataSlot.save01:
                PlayerDataManager.Instance.TransferData(playerDataSave01);
                break;
            case SaveDataSlot.save02:
                PlayerDataManager.Instance.TransferData(playerDataSave02);
                break;
            case SaveDataSlot.save03:
                PlayerDataManager.Instance.TransferData(playerDataSave03);
                break;
            default:
                break;
        }
    }
}