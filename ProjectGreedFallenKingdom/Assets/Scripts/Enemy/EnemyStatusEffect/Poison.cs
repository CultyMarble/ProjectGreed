using UnityEngine;

public class Poison : StatusEffect
{
    [Header("Poison Config:")]
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private float spreadRadius;

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.O))
        {
            Activate(0.025f);
        }
    }

    //===========================================================================
    protected override void TriggerHandler()
    {
        enemyHealth.UpdateCurrentHealth(-1);
    }

    protected override void OverstackHandler()
    {
        return;
    }
}