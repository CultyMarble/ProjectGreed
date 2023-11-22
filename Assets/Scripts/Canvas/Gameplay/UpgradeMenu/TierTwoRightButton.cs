using UnityEngine;

public class TierTwoRightButton : UpgradeMenuButton
{
    public override void AppliedEffect()
    {
        Debug.Log("Add Tier 2 Left Upgrade");
    }

    public override void RemoveEffect()
    {
        Debug.Log("Remove Tier 2 Left Upgrade");
    }
}
