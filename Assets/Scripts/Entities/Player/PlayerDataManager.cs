using UnityEngine;

public class PlayerDataManager : SingletonMonobehaviour<PlayerDataManager>
{
    private SOPlayerData playerDataRuntime = default;
    public SOPlayerData PlayerDataRuntime => playerDataRuntime;

    //===========================================================================
    protected override void Awake()
    {
        base.Awake();

        playerDataRuntime = ScriptableObject.CreateInstance<SOPlayerData>();
    }

    //===========================================================================
    public void TransferData(SOPlayerData saveData)
    {
        playerDataRuntime.TransferData(saveData);

        Player.Instance.GetComponent<PlayerMovement>().UpdateMovementParameters();
        Player.Instance.GetComponentInChildren<BasicAbility>().UpdateAbilityParameters();
        Player.Instance.GetComponentInChildren<RangeAbility>().UpdateAbilityParameters();
        Player.Instance.GetComponentInChildren<BombAbility>().UpdateAbilityParameters();
    }
}