using UnityEngine;

public class ItemIncreaseDashRange : TempItem
{
    [SerializeField] private float increaseDashTime = default;

    //===========================================================================
    protected override void AddItemEffect()
    {
        Player.Instance.GetComponent<PlayerController>().UpdateDashTime(increaseDashTime);
    }
}