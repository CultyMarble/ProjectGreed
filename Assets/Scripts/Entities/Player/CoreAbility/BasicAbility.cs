using UnityEngine;
using UnityEngine.InputSystem;

public class BasicAbility : PlayerAbility
{
    // Utility
    public readonly float moveSpeed = 2.0f;
    public readonly float lifeTime = 0.75f;
    public readonly float rechargeDelay = 1.0f;

    public readonly float timeUntilChangeDirectionMax = 0.2f;
    public readonly float timeUntilChangeDirectionMin = 0.1f;
    public readonly float swingMagtitude = 0.8f;
    public readonly float growthRate = 0.75f;
    public readonly float size = 0.18f;

    // Ability Stats
    private float cooldown = default;

    private float maxFuel = default;
    private float fuelConsumePerTrigger = default;
    private float rechargeRate = default;

    private float rechargeTimer = default;
    private float currentFuel = default;

    private float damage = default;

    public float CurrentFuel => currentFuel;
    public float MaxFuel => maxFuel;

    // Pooling System
    [SerializeField] private Transform basicAbilityProjectilePool;

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
        cooldown = Player.Instance.PlayerData.ba_baseCooldown;

        maxFuel = Player.Instance.PlayerData.ba_baseMaxFuel;
        fuelConsumePerTrigger = Player.Instance.PlayerData.ba_fuelConsumePerTrigger;
        rechargeRate = Player.Instance.PlayerData.ba_baseRechargeRate;
        damage = Player.Instance.PlayerData.ba_baseDamage;

        currentFuel = maxFuel;
    }

    protected override void Update()
    {
        base.Update();

        UpdateRechargeTimer();

        RechargeFuel();

        if (Player.Instance.actionState == PlayerActionState.none ||
            Player.Instance.actionState == PlayerActionState.IsUsingBasicAbility)
        {
            InputHandler();
        }
    }

    private void FixedUpdate()
    {
        CultyMarbleHelper.RotateGameObjectToMouseDirection(this.transform);
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
    private void InputHandler()
    {
        if (leftClickButtonCheck)
        {
            if (cooldownTimer <= 0 && currentFuel > 0)
            {
                Player.Instance.actionState = PlayerActionState.IsUsingBasicAbility;
                SpawnParticle();

                currentFuel -= fuelConsumePerTrigger;

                rechargeTimer = rechargeDelay;
                cooldownTimer = cooldown;
            }
        }
        else
        {
            if (Player.Instance.actionState == PlayerActionState.IsUsingBasicAbility)
            {
                Player.Instance.actionState = PlayerActionState.none;
            }
        }
    }

    private void SpawnParticle()
    {
        Vector3 mouseDir = (CultyMarbleHelper.GetMouseToWorldPosition() - transform.position).normalized;
        foreach (Transform particle in basicAbilityProjectilePool)
        {
            if (particle.gameObject.activeInHierarchy == false)
            {
                BasicAbilityBubble _bubble = particle.GetComponent<BasicAbilityBubble>();

                _bubble.SetMovementSpeed(mouseDir, moveSpeed + GetComponentInParent<Rigidbody2D>().velocity.magnitude, lifeTime);
                _bubble.SetMovementPattern(timeUntilChangeDirectionMax, timeUntilChangeDirectionMin, swingMagtitude);
                _bubble.SetSizeAndGrowth(size, growthRate);
                _bubble.SetDamage(damage);

                particle.position = transform.position + (0.5f * mouseDir);
                particle.gameObject.SetActive(true);
                break;
            }
        }
    }

    private void UpdateRechargeTimer()
    {
        if (rechargeTimer <= 0)
            return;

        rechargeTimer -= Time.deltaTime;
    }

    private void RechargeFuel()
    {
        if (currentFuel == maxFuel || rechargeTimer > 0)
            return;

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