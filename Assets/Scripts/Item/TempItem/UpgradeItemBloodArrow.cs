using UnityEngine;

public class UpgradeItemBloodArrow : UpgradeItem
{
    public int increaseMaxRangeAbilityCharge = default;

    //===========================================================================
    protected override void AddItemEffect()
    {
        Player.Instance.GetComponentInChildren<RangeAbility>().UpdateCurrentMaxCharge(increaseMaxRangeAbilityCharge);
    }

    protected override void RemoveItemEffect()
    {
        Player.Instance.GetComponentInChildren<RangeAbility>().UpdateCurrentMaxCharge(-increaseMaxRangeAbilityCharge);
    }
}
