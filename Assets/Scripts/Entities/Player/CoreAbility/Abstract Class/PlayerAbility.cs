using UnityEngine;

public abstract class PlayerAbility : MonoBehaviour
{
    [Header("Ability Settings:")]
    [SerializeField] protected float cooldown;

    protected float cooldownTimer = default;
    public float CooldownTimer => cooldownTimer;

    //===========================================================================
    protected virtual void Update()
    {
        AbilityCooldown();
    }

    //===========================================================================
    private void AbilityCooldown()
    {
        if (cooldownTimer <= 0.0f)
            return;

        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0.0f)
            cooldownTimer = 0.0f;
    }
}