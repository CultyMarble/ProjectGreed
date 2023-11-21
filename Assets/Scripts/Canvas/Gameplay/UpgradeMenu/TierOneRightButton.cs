using UnityEngine;

public class TierOneRightButton : UpgradeMenuButton
{
    public override void AppliedEffect()
    {
        Player.Instance.GetComponentInChildren<RangeAbility>().UpdateCurrentMaxCharge(1);
        Player.Instance.GetComponentInChildren<RangeAbility>().UpdateCurrentCharge(1);
    }

    public override void RemoveEffect()
    {
        Player.Instance.GetComponentInChildren<RangeAbility>().UpdateCurrentMaxCharge(-1);
    }
}