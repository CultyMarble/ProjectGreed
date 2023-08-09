using UnityEngine;

public class StatusEffectPoison : EnemyStatusEffectNew
{
    // Status Effect Config
    private readonly float baseDuration = 3.0f;
    private readonly float triggerInterval = 0.01f;

    private float duration = default;
    private float triggerTimeCounter = default;

    // Burning Damage Config
    private readonly float baseDamageFactor = 0.01f;
    private float damageFactor = default;
    private float damagePerTrigger = default;

    //======================================================================
    private void Update()
    {
        DurationCheck();

        TriggerDamage();
    }

    //===========================================================================
    private void DurationCheck()
    {
        duration -= Time.deltaTime;

        if (duration > 0)
            return;

        RemoveResetEffect();
    }

    private void TriggerDamage()
    {
        triggerTimeCounter += Time.deltaTime;
        if (triggerTimeCounter >= triggerInterval)
        {
            triggerTimeCounter -= triggerInterval;

            enemyHealth.UpdateCurrentHealth(-damagePerTrigger);
        }
    }

    private void RemoveResetEffect()
    {
        this.enabled = false;

        damageFactor = default;
        damagePerTrigger = default;
    }

    //===========================================================================
    public void SetDamageFactor(float modifier)
    {
        damageFactor = baseDamageFactor * (1.0f + (modifier * 0.01f));
    }

    public void SetAppliedDamage(float appliedDamage)
    {
        float testDamage = damageFactor * appliedDamage;

        if (testDamage < damagePerTrigger)
            return;

        damagePerTrigger = testDamage;
    }

    public void SetDuration(float modifier = 1.0f)
    {
        duration = baseDuration * modifier;
    }
}