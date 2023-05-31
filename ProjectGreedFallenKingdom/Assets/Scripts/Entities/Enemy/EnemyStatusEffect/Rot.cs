using UnityEngine;

public class Rot : StatusEffect
{
    [Header("Rot Config:")]
    [SerializeField] private int killThreshold;

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