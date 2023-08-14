using UnityEngine;
using UnityEngine.InputSystem;

public class BasicAbility : PlayerAbility
{
    // Utility
    public readonly float moveSpeed = 3.0f;
    public readonly float lifeTime = 0.75f;
    public readonly float rechargeDelay = 1.0f;

    public readonly float timeUntilChangeDirectionMax = 0.2f;
    public readonly float timeUntilChangeDirectionMin = 0.1f;
    public readonly float swingMagtitude = 0.8f;
    public readonly float growthRate = 0.75f;
    public readonly float size = 0.18f;

    // Pooling System
    [SerializeField] private Transform basicAbilityProjectilePool;

    // Ability Stats
    private float maxFuel = default;
    private float drainRate = default;
    private float rechargeRate = default;

    private float rechargeTimer = default;
    private float currentFuel = default;

    private float damage = default;

    public float CurrentFuel { get => currentFuel; private set { } }
    public float MaxFuel { get => maxFuel; private set { } }

    //===========================================================================
    // NEW INPUT SYSTEM
    private PlayerInput playerInput;
    private bool leftClickButtonCheck = false;

    //===========================================================================
    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    private void OnEnable()
    {
        playerInput.actions["LeftClick"].started += ActionPerformed;
        playerInput.actions["LeftClick"].canceled += ActionCanceled;
    }

    private void Start()
    {
        maxFuel = Player.Instance.PlayerStat.ba_baseMaxFuel;
        drainRate = Player.Instance.PlayerStat.ba_baseDrainRate;
        rechargeRate = Player.Instance.PlayerStat.ba_baseRechargeRate;
        damage = Player.Instance.PlayerStat.ba_baseDamage;

        currentFuel = maxFuel;
    }

    private void OnDisable()
    {
        playerInput.actions["LeftClick"].started -= ActionPerformed;
        playerInput.actions["LeftClick"].canceled -= ActionCanceled;
    }

    //===========================================================================
    private void ActionPerformed(InputAction.CallbackContext obj)
    {
        leftClickButtonCheck = true;
    }

    private void ActionCanceled(InputAction.CallbackContext obj)
    {
        leftClickButtonCheck = false;
    }

    //===========================================================================
    protected override void Update()
    {
        base.Update();

        //Debug.Log(cooldownTimer);

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

        UpdateRechargeTimerCheck();

        if (currentFuel != maxFuel && rechargeTimer <= 0)
            RechargeFuel();
    }

    private void FixedUpdate()
    {
        CultyMarbleHelper.RotateGameObjectToMouseDirection(this.transform);
    }

    //===========================================================================
    private void InputHandler()
    {
        if (leftClickButtonCheck && cooldownTimer <= 0 && currentFuel > 0)
        {
            Player.Instance.playerActionState = PlayerActionState.IsUsingBasicAbility;
            SpawnParticle();

            currentFuel -= drainRate * Time.deltaTime;

            rechargeTimer = rechargeDelay;
            cooldownTimer = cooldown;
        }
        else if (!leftClickButtonCheck && Player.Instance.playerActionState == PlayerActionState.IsUsingBasicAbility)
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
                particle.GetComponent<SprayParticleProjectile>().ConfigParticleSizeAndGrowth(size, growthRate);
                particle.GetComponent<SprayParticleProjectile>().ConfigParticleDamage(damage);

                particle.position = this.transform.position + 0.5f * mouseDir;
                particle.gameObject.SetActive(true);
                break;
            }
        }
    }

    private void UpdateRechargeTimerCheck()
    {
        if (rechargeTimer <= 0)
            return;

        rechargeTimer -= Time.deltaTime;
    }

    private void RechargeFuel()
    {
        currentFuel += rechargeRate * Time.deltaTime;
        currentFuel = Mathf.Clamp(currentFuel, 0.0f, maxFuel);
    }

    //===========================================================================
    public void UpdateMaxFuel(float amount)
    {
        maxFuel += amount;
    }

    public void ResetMaxFuel()
    {
        maxFuel = 100.0f;
    }

    public void UpdateRefuelRate(float amount)
    {
        rechargeRate += amount;
    }
}