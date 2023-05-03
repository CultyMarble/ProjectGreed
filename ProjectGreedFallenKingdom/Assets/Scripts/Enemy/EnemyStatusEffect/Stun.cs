using UnityEngine;

public class Stun : StatusEffect
{
    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.K))
            Activate();
    }

    //======================================================================
    protected override void TriggerHandler()
    {
        enemyStatusEffect.HostEntity.GetComponent<TargetingAI>().currentTargetTransform = null;
    }

    protected override void OverstackHandler()
    {
        return;
    }
}