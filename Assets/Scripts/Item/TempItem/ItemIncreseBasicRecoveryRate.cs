using UnityEngine;

public class ItemIncreseBasicRecoveryRate : TempItem
{
    [SerializeField] private int increaseBasicRecoveryRate = default;

    //===========================================================================
    protected override void AddItemEffect()
    {
        Player.Instance.GetComponentInChildren<BasicAbility>().UpdateRefuelRate(increaseBasicRecoveryRate);
    }
}