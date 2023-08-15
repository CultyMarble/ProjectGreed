using UnityEngine;

public class PlayerData : MonoBehaviour
{
    // Basic Stat
    public readonly int baseMaxHealth = 3;

    // Movement
    public readonly float baseMoveSpeed = 6.0f;

    public readonly float baseDashPenalty = 0.1f;
    public readonly float baseDashCooldown = 1.0f;
    public readonly float baseDashTime = 0.25f;
    public readonly float baseDashSpeed = 20.0f;

    // Basic Ability Stat
    public readonly float ba_baseCooldown = 0.05f;
    public readonly float ba_baseMaxFuel = 100.0f;
    public readonly float ba_fuelConsumePerTrigger = 1.0f;
    public readonly float ba_baseRechargeRate = 100.0f;
    public readonly float ba_baseDamage = 3.0f;

    // Range Ability Stat
    public readonly int ra_baseCharge = 2;

    public readonly float ra_baseMinChargeTime = 0.5f;
    public readonly float ra_baseMidChargeTime = 1.0f;
    public readonly float ra_baseMaxChargeTime = 2.0f;

    public readonly float ra_baseMinDamage = 10.0f;
    public readonly float ra_baseMidDamage = 30.0f;
    public readonly float ra_baseMaxDamage = 60.0f;

    public readonly float ra_baseMinSpeed = 15.0f;
    public readonly float ra_baseMidSpeed = 20.0f;
    public readonly float ra_baseMaxSpeed = 30.0f;

    public readonly float ra_basePlayerMinSpeed = 1.0f;
    public readonly float ra_basePlayerMidSpeed = 3.0f;
    public readonly float ra_basePlayerMaxSpeed = 5.0f;

    // Bomb Ability Stat
    public readonly int bomb_baseCharge = 0;

    public readonly float bomb_baseDamage = 50.0f;
    public readonly float bomb_baseDelayTime = 1.5f;
    public readonly float bomb_baseRadius = 3.0f;
}