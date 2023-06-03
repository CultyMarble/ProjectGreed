using UnityEngine;
public enum AbilityStatusEffect
{
    Poison,
    Rot,
    none,
}
public abstract class CoreAbility : MonoBehaviour
{
    [Header("Ability Settings:")]
    [SerializeField] protected float damage;

    [SerializeField] protected float pushPower;
    [SerializeField] protected float pushRadius;

    [SerializeField] protected float cooldown;
    [SerializeField] public AbilityStatusEffect abilityStatusEffect;
    protected float cooldownTimer = default;

    public float CooldownTimer { get => cooldownTimer; private set { } }

    //===========================================================================
    protected virtual void Update()
    {
        AbilityCooldown();
    }

    //===========================================================================
    private void AbilityCooldown()
    {
        if (cooldownTimer == 0.0f)
            return;

        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0.0f)
            cooldownTimer = 0.0f;
    }
}
