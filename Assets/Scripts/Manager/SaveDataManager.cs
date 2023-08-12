using UnityEngine;

public class SaveDataManager : SingletonMonobehaviour<SaveDataManager>
{
    [Header("SaveFile")]
    [SerializeField] private SOGameData save01 = default;
    public SOGameData SAVE01 => save01;

    [Header("Gameplay Runtime Data:")]
    [SerializeField] private SOListInt generatedItemForSale = default;

    //===========================================================================
    protected override void Awake()
    {
        base.Awake();

        save01.SaveCheckPointData(SceneName.Scene03_HubArea, Vector3.zero);
    }

    private void OnEnable()
    {
        SceneControlManager.Instance.OnUnloadRuntimeDataEvent += Instance_OnUnloadRuntimeDataEvent;
    }

    private void OnDisable()
    {
        ClearGameplayRuntimeData();

        SceneControlManager.Instance.OnUnloadRuntimeDataEvent -= Instance_OnUnloadRuntimeDataEvent;
    }

    //===========================================================================
    private void Instance_OnUnloadRuntimeDataEvent(object sender, System.EventArgs e)
    {
        ClearGameplayRuntimeData();
    }

    private void ClearGameplayRuntimeData()
    {
        generatedItemForSale.itemList.Clear();
    }
}