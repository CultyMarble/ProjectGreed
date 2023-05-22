using UnityEngine;

public class Stun : StatusEffect
{
    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.K))
        {
            Activate();
        }
    }

    //======================================================================
    protected override void TriggerHandler()
    {
        if (enemyStatusEffect.Host.GetComponent<TargetingAI>())
            enemyStatusEffect.Host.GetComponent<TargetingAI>().ClearTarget();
        else if (enemyStatusEffect.Host.GetComponent<TargetingAIBasic>())
            enemyStatusEffect.Host.GetComponent<TargetingAIBasic>().currentTargetTransform = null;
    }

    protected override void OverstackHandler()
    {
        return;
    }
}