using UnityEngine;

public class TierOneMiddleButton : UpgradeMenuButton
{
    public override void AppliedEffect()
    {
        Player.Instance.GetComponentInChildren<BasicAbility>().UpdateMaxFuel(25);
    }

    public override void RemoveEffect()
    {
        Player.Instance.GetComponentInChildren<BasicAbility>().UpdateMaxFuel(-25);
    }
}