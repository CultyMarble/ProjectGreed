using UnityEngine;

public class ItemIncreaseBombDamage : TempItem
{
    [SerializeField] private float increaseDamage = default;

    //===========================================================================
    protected override void AddItemEffect()
    {
        Player.Instance.GetComponentInChildren<BombAbility>().UpdateDamage(increaseDamage);
    }
}