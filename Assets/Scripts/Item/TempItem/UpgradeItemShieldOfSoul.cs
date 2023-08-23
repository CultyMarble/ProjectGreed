using UnityEngine;

public class UpgradeItemShieldOfSoul : UpgradeItem
{
    [SerializeField] private int increaseMaxFuel = default;

    //======================================================================
    protected override void AddItemEffect()
    {
        Player.Instance.GetComponentInChildren<BasicAbility>().UpdateMaxFuel(increaseMaxFuel);
    }

    protected override void RemoveItemEffect()
    {
        Player.Instance.GetComponentInChildren<BasicAbility>().UpdateMaxFuel(-increaseMaxFuel);
    }
}