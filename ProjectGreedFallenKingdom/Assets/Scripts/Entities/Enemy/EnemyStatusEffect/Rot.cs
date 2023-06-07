using UnityEngine;

public class Rot : StatusEffect
{
    [SerializeField] private EnemyHealth enemyHealth = default;
    [SerializeField] private Transform rotBar;

    [Header("Rot Config:")]
    [SerializeField] private int killThreshold;

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.T))
        {
            Activate(0.5f, 10.5f, 25);
        }

        rotBar.localScale = new Vector3(stackAmount * 0.01f, 1.0f, 1.0f);
    }

    //===========================================================================
    protected override void TriggerHandler()
    {
        if (enemyHealth.GetHealthPercentage() <= stackAmount)
        {
            enemyHealth.UpdateCurrentHealth(-99999);
        }
        else
        {
            stackAmount--;
        }
    }

    protected override void OverstackHandler()
    {
        return;
    }
}