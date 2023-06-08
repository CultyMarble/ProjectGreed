using UnityEngine;

public class Poison : StatusEffect
{
    [Header("Poison Config:")]
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private float spreadRadius;

    //===========================================================================
    protected override void TriggerHandler()
    {
        enemyHealth.UpdateCurrentHealth(-stackAmount * 0.025f);
    }

    protected override void OverstackHandler()
    {
        return;
    }
}