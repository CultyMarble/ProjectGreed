using UnityEngine;

public class SaveDataManager : SingletonMonobehaviour<SaveDataManager>
{
    [SerializeField] private SOGameData save01;
    public SOGameData SAVE01 => save01;
}
