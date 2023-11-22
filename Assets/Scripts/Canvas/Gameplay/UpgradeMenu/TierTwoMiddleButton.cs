using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TierTwoMiddleButton : UpgradeMenuButton
{
    public override void AppliedEffect()
    {
        Debug.Log("Add Tier 2 Middle Upgrade");
    }

    public override void RemoveEffect()
    {
        Debug.Log("Remove Tier 2 Middle Upgrade");
    }
}
