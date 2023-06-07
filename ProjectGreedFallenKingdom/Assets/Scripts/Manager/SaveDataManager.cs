using UnityEngine;

public class SaveDataManager : SingletonMonobehaviour<SaveDataManager>
{
    [SerializeField] private SOGameData save01;

    public SOGameData SAVE01 => save01;

    //===========================================================================

    protected override void Awake()
    {
        base.Awake();

        save01.SaveCheckPointData(SceneName.Scene03_HubArea, Vector3.zero);
    }
}