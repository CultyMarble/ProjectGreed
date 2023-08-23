using UnityEngine;

public class UpgradeItemShieldOfFaith : UpgradeItem
{
    [SerializeField] private int increaseMaxHeart = default;

    //===========================================================================
    protected override void AddItemEffect()
    {
        Player.Instance.GetComponent<PlayerHeart>().UpdateCurrentMaxHeart(increaseMaxHeart);
    }

    protected override void RemoveItemEffect()
    {
        Player.Instance.GetComponent<PlayerHeart>().UpdateCurrentMaxHeart(-increaseMaxHeart);
    }
}