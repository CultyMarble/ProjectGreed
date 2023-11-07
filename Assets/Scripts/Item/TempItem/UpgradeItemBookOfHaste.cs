
public class UpgradeItemBookOfHaste : UpgradeItem
{
    public float newDashTime = default;
    public float newDashSpeed = default;

    //===========================================================================
    protected override void AddItemEffect()
    {
<<<<<<< Updated upstream
        Player.Instance.GetComponent<PlayerMovement>().SetDashParameter(newDashTime, newDashSpeed);
=======
        Player.Instance.GetComponent<PlayerMovement>().UpdateDashParameter(increaseDashTime, increaseDashSpeed);
>>>>>>> Stashed changes
    }

    protected override void RemoveItemEffect()
    {
        Player.Instance.GetComponent<PlayerMovement>().UpdateDashParameter(-increaseDashTime, -increaseDashSpeed);
    }
}