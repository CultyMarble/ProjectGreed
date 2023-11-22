using UnityEngine;

public class TierTwoLeftButton : UpgradeMenuButton
{
    public override void AppliedEffect()
    {
        Player.Instance.GetComponentInChildren<BombAbility>().SetBombManualTrigger(true);
    }

    public override void RemoveEffect()
    {
        Player.Instance.GetComponentInChildren<BombAbility>().SetBombManualTrigger(false);
    }
}
