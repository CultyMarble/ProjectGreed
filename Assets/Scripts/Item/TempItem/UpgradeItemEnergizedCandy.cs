using UnityEngine;

public class UpgradeItemEnergizedCandy : UpgradeItem
{
    public float increaseRechargeRate = default;

    //===========================================================================
    protected override void AddItemEffect()
    {
        Player.Instance.GetComponentInChildren<BasicAbility>().UpdateFuelRechargeRate(increaseRechargeRate);
    }

    protected override void RemoveItemEffect()
    {
        Player.Instance.GetComponentInChildren<BasicAbility>().UpdateFuelRechargeRate(-increaseRechargeRate);
    }
}