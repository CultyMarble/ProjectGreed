using UnityEngine;

public class Regeneration : StatusEffect
{
    [Header("Regeneration Config:")]
    [SerializeField] private int regenAmount;

    //===========================================================================
    protected override void TriggerHandler()
    {
        return;
    }

    protected override void OverstackHandler()
    {
        return;
    }
}