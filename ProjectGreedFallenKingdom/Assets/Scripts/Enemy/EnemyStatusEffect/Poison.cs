using UnityEngine;

public class Poison : StatusEffect
{
    [Header("Poison Config:")]
    [SerializeField] private float spreadRadius;

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