using UnityEngine;

public class UpgradeItemBookOfHaste : UpgradeItem
{
    public float increaseDashTime = default;
    public float increaseDashSpeed = default;

    //===========================================================================
    protected override void AddItemEffect()
    {
        Player.Instance.GetComponent<PlayerMovement>().IncreaseDashParameter(increaseDashTime, increaseDashSpeed);
    }

    protected override void RemoveItemEffect()
    {

    }
}