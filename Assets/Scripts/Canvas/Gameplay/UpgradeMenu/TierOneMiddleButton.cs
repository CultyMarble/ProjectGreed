using UnityEngine;

public class TierOneMiddleButton : UpgradeMenuButton
{
    public override void AppliedEffect()
    {
        Debug.Log("Applied Tier 01 Middle Effect");
    }

    public override void RemoveEffect()
    {
        Debug.Log("Removed Tier 01 Middle Effect");
    }
}
