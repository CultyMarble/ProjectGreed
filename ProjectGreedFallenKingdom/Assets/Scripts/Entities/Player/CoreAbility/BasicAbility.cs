using UnityEngine;

public class BasicAbility : CoreAbility
{
    [Header("Particle Pool:")]
    [SerializeField] private Transform basicAbilityProjectilePool;

    [Header("Particle Settings:")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float lifeTime;
    [SerializeField] private float maxFuel;
    [SerializeField] private float fuelDrainRate;
    [SerializeField] private float refuelRate;
    [SerializeField] private float refuelDelay;

    [SerializeField] private float timeUntilChangeDirectionMax;
    [SerializeField] private float timeUntilChangeDirectionMin;
    [SerializeField] private float swingMagtitude;
    [SerializeField] private float growthRate;
    [SerializeField] private float size;

    private float refuelCounter;
    private float currentFuel = default;

    public float CurrentFuel { get => currentFuel; private set { } }
    public float MaxFuel { get => maxFuel; private set { } }

    //===========================================================================
    private void Start()
    {
        currentFuel = maxFuel;
    }

    protected override void Update()
    {
        base.Update();

        switch (Player.Instance.playerActionState)
        {
            case PlayerActionState.none:
                InputHandler();
                break;
            case PlayerActionState.IsUsingBasicAbility:
                InputHandler();
                break;
            default:
                break;
        }


        InputHandler();
        Fuel();
    }

    private void FixedUpdate()
    {
        CultyMarbleHelper.RotateGameObjectToMouseDirection(this.transform);
    }

    //===========================================================================
    private void InputHandler()
    {
        if (Input.GetMouseButton(0) && cooldownTimer == 0 && currentFuel > 0)
        {
            Player.Instance.playerActionState = PlayerActionState.IsUsingBasicAbility;
            SpawnParticle();
            if (currentFuel < fuelDrainRate)
            {
                currentFuel = 0;
            }
            else
            {
                currentFuel -= fuelDrainRate;
            }
            cooldownTimer = cooldown;
        }
        else if (!Input.GetMouseButton(0) && Player.Instance.playerActionState == PlayerActionState.IsUsingBasicAbility)
        {
            Player.Instance.playerActionState = PlayerActionState.none;
        }
    }

    private void SpawnParticle()
    {
        Vector3 mouseDir = (CultyMarbleHelper.GetMouseToWorldPosition() - this.transform.position).normalized;
        foreach (Transform particle in basicAbilityProjectilePool)
        {
            if (particle.gameObject.activeInHierarchy == false)
            {
                particle.GetComponent<SprayParticleProjectile>().ConfigParticleMovementSpeed(mouseDir, moveSpeed + this.GetComponentInParent<Rigidbody2D>().velocity.magnitude, lifeTime);
                particle.GetComponent<SprayParticleProjectile>().
                    ConfigParticleMovementPattern(timeUntilChangeDirectionMax, timeUntilChangeDirectionMin, swingMagtitude);
                particle.GetComponent<SprayParticleProjectile>().ConfigParticleSizeAndGrowth(size,growthRate);
                particle.GetComponent<SprayParticleProjectile>().ConfigParticleDamage(damage, pushPower, abilityStatusEffect);

                particle.position = this.transform.position + 0.5f * mouseDir;
                particle.gameObject.SetActive(true);
                break; 
            }
        }
    }

    void Fuel()
    {
        refuelCounter += Time.deltaTime;

        if (currentFuel < maxFuel && refuelCounter >= refuelDelay && !Input.GetMouseButton(0))
        {
            currentFuel += refuelRate;
            refuelCounter = 0;
        }
    }
}