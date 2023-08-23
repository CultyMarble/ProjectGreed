using UnityEngine;

public class UpgradeItemBookOfHaste : UpgradeItem
{
    public float newDashTime = default;
    public float newDashSpeed = default;

    //===========================================================================
    protected override void AddItemEffect()
    {
        Player.Instance.GetComponent<PlayerMovement>().SetDashParameter(newDashTime, newDashSpeed);
    }

    protected override void RemoveItemEffect()
    {
        Player.Instance.GetComponent<PlayerMovement>().SetDashParameter(
            Player.Instance.PlayerData.baseDashTime, 
            Player.Instance.PlayerData.baseDashSpeed);
    }
}