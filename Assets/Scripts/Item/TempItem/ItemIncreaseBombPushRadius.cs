using UnityEngine;

public class ItemIncreaseBombPushRadius : TempItem
{
    [SerializeField] private float increasePushRadius = default;

    //===========================================================================
    protected override void AddItemEffect()
    {
        Player.Instance.GetComponent<BombAbility>().UpdateRadius(increasePushRadius);
    }
}