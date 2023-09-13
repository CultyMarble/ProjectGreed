using UnityEngine;

public class TierOneRightButton : UpgradeMenuButton
{
    public override void AppliedEffect()
    {
        Debug.Log("Applied Tier 01 Right Effect");
    }

    public override void RemoveEffect()
    {
        Debug.Log("Removed Tier 01 Right Effect");
    }
}
