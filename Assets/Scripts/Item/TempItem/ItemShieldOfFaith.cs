using UnityEngine;

public class ItemShieldOfFaith : TempItem
{
    [SerializeField] private int increaseMaxHeart = default;

    //===========================================================================
    protected override void AddItemEffect()
    {
        Player.Instance.GetComponent<PlayerHealth>().UpdateCurrentMaxHealth(increaseMaxHeart);
    }
}