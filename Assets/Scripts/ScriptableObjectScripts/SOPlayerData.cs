using UnityEngine;

[CreateAssetMenu(menuName = "SOData/NewSOPlayerData", fileName = "SOPlayerData")]
public class SOPlayerData : ScriptableObject
{
    [Header("Basic Data:")]
    [SerializeField] private int baseMaxHealth = default;
    public int BaseMaxHealth => baseMaxHealth;

    [Header("Movement Data:")]
    [SerializeField] private float baseMoveSpeed = default;
    [SerializeField] private float baseDashPenalty = default;
    [SerializeField] private float baseDashCooldown = default;
    [SerializeField] private float baseDashTime = default;
    [SerializeField] private float baseDashSpeed = default;

    public float BaseMoveSpeed => baseMoveSpeed;
    public float BaseDashPenalty => baseDashPenalty;
    public float BaseDashCooldown => baseDashCooldown;
    public float BaseDashTime => baseDashTime;
    public float BaseDashSpeed => baseDashSpeed;

    [Header("Basic Ability Data:")]
    [SerializeField] private float ba_baseCooldown = default;
    [SerializeField] private float ba_baseMaxFuel = default;
    [SerializeField] private float ba_fuelConsumePerTrigger = default;
    [SerializeField] private float ba_baseRechargeRate = default;
    [SerializeField] private float ba_baseDamage = default;
    [SerializeField] private float ba_basePlayerSpeedPenalty = default;

    public float Ba_baseCooldown => ba_baseCooldown;
    public float Ba_baseMaxFuel => ba_baseMaxFuel;
    public float Ba_fuelConsumePerTrigger => ba_fuelConsumePerTrigger;
    public float Ba_baseRechargeRate => ba_baseRechargeRate;
    public float Ba_baseDamage => ba_baseDamage;
    public float Ba_basePlayerSpeedPenalty => ba_basePlayerSpeedPenalty;

    [Header("Range Ability Data")]
    [SerializeField] private int ra_baseMaxCharge = default;
    [SerializeField] private int ra_baseMaxRecharge = default;

    public int Ra_baseMaxCharge => ra_baseMaxCharge;
    public int Ra_baseMaxRecharge => ra_baseMaxRecharge;

    [SerializeField] private float ra_baseMinChargeTime = default;
    [SerializeField] private float ra_baseMidChargeTime = default;
    [SerializeField] private float ra_baseMaxChargeTime = default;

    public float Ra_baseMinChargeTime => ra_baseMinChargeTime;
    public float Ra_baseMidChargeTime => ra_baseMidChargeTime;
    public float Ra_baseMaxChargeTime => ra_baseMaxChargeTime;

    [SerializeField] private float ra_baseMinDamage = default;
    [SerializeField] private float ra_baseMidDamage = default;
    [SerializeField] private float ra_baseMaxDamage = default;

    public float Ra_baseMinDamage => ra_baseMinDamage;
    public float Ra_baseMidDamage => ra_baseMidDamage;
    public float Ra_baseMaxDamage => ra_baseMaxDamage;

    [SerializeField] private float ra_baseMinSpeed = default;
    [SerializeField] private float ra_baseMidSpeed = default;
    [SerializeField] private float ra_baseMaxSpeed = default;

    public float Ra_baseMinSpeed => ra_baseMinSpeed;
    public float Ra_baseMidSpeed => ra_baseMidSpeed;
    public float Ra_baseMaxSpeed => ra_baseMaxSpeed;

    [SerializeField] private float ra_basePlayerMinSpeed = default;
    [SerializeField] private float ra_basePlayerMidSpeed = default;
    [SerializeField] private float ra_basePlayerMaxSpeed = default;

    public float Ra_basePlayerMinSpeed => ra_basePlayerMinSpeed;
    public float Ra_basePlayerMidSpeed => ra_basePlayerMidSpeed;
    public float Ra_basePlayerMaxSpeed => ra_basePlayerMaxSpeed;

    [Header("Bomb Ability Data")]
    [SerializeField] private int bomb_baseCharge = default;

    [SerializeField] private float bomb_baseDamage = default;
    [SerializeField] private float bomb_baseDelayTime = default;
    [SerializeField] private float bomb_baseRadius = default;

    public int Bomb_baseCharge => bomb_baseCharge;
    public float Bomb_baseDamage => bomb_baseDamage;
    public float Bomb_baseDelayTime => bomb_baseDelayTime;
    public float Bomb_baseRadius => bomb_baseRadius;

    //===========================================================================
    public void TransferData(SOPlayerData saveData)
    {
        baseMaxHealth = saveData.baseMaxHealth;

        baseMoveSpeed = saveData.baseMoveSpeed;
        baseDashPenalty = saveData.baseDashPenalty;
        baseDashCooldown = saveData.baseDashCooldown;
        baseDashTime = saveData.baseDashTime;
        baseDashSpeed = saveData.baseDashSpeed;

        ba_baseCooldown = saveData.ba_baseCooldown;
        ba_baseMaxFuel = saveData.ba_baseMaxFuel;
        ba_fuelConsumePerTrigger = saveData.ba_fuelConsumePerTrigger;
        ba_baseRechargeRate = saveData.ba_baseRechargeRate;
        ba_baseDamage = saveData.ba_baseDamage;
        ba_basePlayerSpeedPenalty = saveData.ba_basePlayerSpeedPenalty;

        ra_baseMaxCharge = saveData.ra_baseMaxCharge;
        ra_baseMaxRecharge = saveData.ra_baseMaxRecharge;

        ra_baseMinChargeTime = saveData.ra_baseMinChargeTime;
        ra_baseMidChargeTime = saveData.ra_baseMidChargeTime;
        ra_baseMaxChargeTime = saveData.ra_baseMaxChargeTime;

        ra_baseMinDamage = saveData.ra_baseMinDamage;
        ra_baseMidDamage = saveData.ra_baseMidDamage;
        ra_baseMaxDamage = saveData.ra_baseMaxDamage;

        ra_baseMinSpeed = saveData.ra_baseMinSpeed;
        ra_baseMidSpeed = saveData.ra_baseMidSpeed;
        ra_baseMaxSpeed = saveData.ra_baseMaxSpeed;

        ra_basePlayerMinSpeed = saveData.ra_basePlayerMinSpeed;
        ra_basePlayerMidSpeed = saveData.ra_basePlayerMidSpeed;
        ra_basePlayerMaxSpeed = saveData.ra_basePlayerMaxSpeed;

        bomb_baseCharge = saveData.bomb_baseCharge;
        bomb_baseDamage = saveData.bomb_baseDamage;
        bomb_baseDelayTime = saveData.bomb_baseDelayTime;
        bomb_baseRadius = saveData.bomb_baseRadius;
    }

    public void ClearData()
    {
        baseMaxHealth = default;

        baseMoveSpeed = default;
        baseDashPenalty = default;
        baseDashCooldown = default;
        baseDashTime = default;
        baseDashSpeed = default;

        ba_baseCooldown = default;
        ba_baseMaxFuel = default;
        ba_fuelConsumePerTrigger = default;
        ba_baseRechargeRate = default;
        ba_baseDamage = default;
        ba_basePlayerSpeedPenalty = default;

        ra_baseMaxCharge = default;
        ra_baseMaxRecharge = default;

        ra_baseMinChargeTime = default;
        ra_baseMidChargeTime = default;
        ra_baseMaxChargeTime = default;

        ra_baseMinDamage = default;
        ra_baseMidDamage = default;
        ra_baseMaxDamage = default;

        ra_baseMinSpeed = default;
        ra_baseMidSpeed = default;
        ra_baseMaxSpeed = default;

        ra_basePlayerMinSpeed = default;
        ra_basePlayerMidSpeed = default;
        ra_basePlayerMaxSpeed = default;

        bomb_baseCharge = default;
        bomb_baseDamage = default;
        bomb_baseDelayTime = default;
        bomb_baseRadius = default;
    }
}