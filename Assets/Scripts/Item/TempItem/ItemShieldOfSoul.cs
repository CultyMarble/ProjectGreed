using UnityEngine;

public class ItemShieldOfSoul : TempItem
{
    [SerializeField] private int increaseMaxFuel = default;

    //======================================================================
    protected override void AddItemEffect()
    {
        Player.Instance.GetComponentInChildren<BasicAbility>().UpdateMaxFuel(increaseMaxFuel);
    }
}