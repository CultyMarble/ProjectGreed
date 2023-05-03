using UnityEngine;

public class Stun : StatusEffect
{
    //===========================================================================
    protected override void TriggerHandler()
    {
        entityStatusEffect.HostEntity.GetComponent<ChasingAI>().StopMovement(triggerInterval);
    }

    protected override void OverstackHandler()
    {
        return;
    }
}
