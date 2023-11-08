using UnityEngine;

public class UpgradeItemSpeedCandy : UpgradeItem
{
    [SerializeField] private int increaSpeed = default;

    //======================================================================
    protected override void AddItemEffect()
    {
        Player.Instance.GetComponentInChildren<PlayerMovement>().AddMoveSpeed(increaSpeed);
    }

    protected override void RemoveItemEffect()
    {
        Player.Instance.GetComponentInChildren<PlayerMovement>().AddMoveSpeed(-increaSpeed);
    }
}
