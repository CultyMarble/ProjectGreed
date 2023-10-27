using UnityEngine;

public enum UpgradeTier
{
    Tier1,
    Tier2,
    Tier3,
}

public enum UpgradePath
{
    Left,
    Middle,
    Right,
    none,
}

public abstract class UpgradeMenuButton : MonoBehaviour
{
    public abstract void AppliedEffect();
    public abstract void RemoveEffect();
}
