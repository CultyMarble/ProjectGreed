using UnityEngine;

public class Regeneration : StatusEffect
{
    [Header("Regeneration Config:")]
    [SerializeField] private int regenAmount;

    //===========================================================================
    protected override void TriggerHandler()
    {
        enemyStatusEffect.Host.GetComponent<EnemyHealth>().UpdateCurrentHealth(regenAmount);
    }

    protected override void OverstackHandler()
    {

    }
}