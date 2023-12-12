using UnityEngine;
using UnityEngine.InputSystem;

public class RangeAbility : MonoBehaviour
{
    public struct OnMaxChargeChangedEventArgs { public int maxCharge; }
    public event System.EventHandler<OnMaxChargeChangedEventArgs> OnMaxChargeChangedEvent;

    public struct OnCurrentChargeChangedEventArgs { public int currentCharge; }
    public event System.EventHandler<OnCurrentChargeChangedEventArgs> OnCurrentChargeChangedEvent;

    private EnemyHealth enemyHealth;

    [Header("Effect Settings:")]
    [SerializeField] private SpriteRenderer aimIndicator;

    private int currentMaxCharge = default;
    private int currentCharge = default;
    private int maxRecharge = default;
    private int currentRecharge = default;

    private float projectileSpeed = default;
    private float projectileDamage = default;

    private float channelTimer = default;

    // Pooling
    [Header("Pooling Settings:")]
    [SerializeField] private Transform rangeAbilityProjectilePool = default;
    [SerializeField] private Transform pfRangeAbilityProjectile = default;
    private readonly int poolSize = 100;

    // NEW INPUT SYSTEM
    private PlayerInput playerInput;
    private bool rightButtonCheck = false;

    PlayerHeart playerHeartManager;

    private bool canCreatePoisonPool = default;

    //===========================================================================
    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        playerHeartManager = GetComponentInParent<PlayerHeart>();
    }

    private void OnEnable()
    {
        playerInput.actions["RightClick"].started += ActionPerformed;
        playerInput.actions["RightClick"].canceled += ActionCanceled;
        playerHeartManager.OnDespawnPlayerEvent += EmptyPool;

        PopulatePool();
    }

    private void Update()
    {
        if (SceneControlManager.Instance.CurrentGameplayState == GameplayState.Pause)
        {
            Player.Instance.actionState = PlayerActionState.none;
            rightButtonCheck = false;
        }

        if (Player.Instance.actionState == PlayerActionState.none ||
            Player.Instance.actionState == PlayerActionState.IsUsingRangeAbility)
            InputHandler();

        if (channelTimer > 0)
            Player.Instance.actionState = PlayerActionState.IsUsingRangeAbility;

        if (Input.GetKeyDown(KeyCode.Numlock))
        {
            UpdateCurrentRecharge(100);
        }
        if (Player.Instance.actionState == PlayerActionState.IsUsingRangeAbility
            && Input.GetMouseButton(1) == false)
        {
            Player.Instance.actionState = PlayerActionState.none;
        }
    }

    private void FixedUpdate()
    {
        CultyMarbleHelper.RotateGameObjectToMouseDirection(this.transform);
    }

    private void OnDisable()
    {
        playerInput.actions["RightClick"].started -= ActionPerformed;
        playerInput.actions["RightClick"].canceled -= ActionCanceled;
        playerHeartManager.OnDespawnPlayerEvent -= EmptyPool;
    }

    //===========================================================================
    private void ActionPerformed(InputAction.CallbackContext obj)
    {
        rightButtonCheck = true;
    }

    private void ActionCanceled(InputAction.CallbackContext obj)
    {
        rightButtonCheck = false;
    }

    //===========================================================================
    private void PopulatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Instantiate(pfRangeAbilityProjectile, rangeAbilityProjectilePool).gameObject.SetActive(false);
        }
    }

    private void InputHandler()
    {
        if (rightButtonCheck)
        {
            aimIndicator.gameObject.SetActive(true);
            ChannelHandler();
        }
        else
        {
            ShootHandler();

            aimIndicator.gameObject.SetActive(false);
            channelTimer = 0;

            SetPlayerMovementSpeed();
        }
    }

    private void UpdateIndicatorColor()
    {
        if (channelTimer >= PlayerDataManager.Instance.PlayerDataRuntime.Ra_baseMaxChargeTime)
        {
            aimIndicator.color = Color.green;
        }
        else if (channelTimer >= PlayerDataManager.Instance.PlayerDataRuntime.Ra_baseMidChargeTime)
        {
            aimIndicator.color = Color.cyan;
        }
        else if (channelTimer >= PlayerDataManager.Instance.PlayerDataRuntime.Ra_baseMinChargeTime)
        {
            aimIndicator.color = Color.yellow;
        }
        else
        {
            aimIndicator.color = Color.red;
        }
    }

    private void SetProjectileSpeed()
    {
        if (channelTimer >= PlayerDataManager.Instance.PlayerDataRuntime.Ra_baseMaxChargeTime)
        {
            projectileSpeed = PlayerDataManager.Instance.PlayerDataRuntime.Ra_baseMaxSpeed;
        }
        else if (channelTimer >= PlayerDataManager.Instance.PlayerDataRuntime.Ra_baseMidChargeTime)
        {
            projectileSpeed = PlayerDataManager.Instance.PlayerDataRuntime.Ra_baseMidSpeed;
        }
        else if (channelTimer >= PlayerDataManager.Instance.PlayerDataRuntime.Ra_baseMinChargeTime)
        {
            projectileSpeed = PlayerDataManager.Instance.PlayerDataRuntime.Ra_baseMinSpeed;
        }
        else
        {
            projectileSpeed = 0.0f;
        }
    }

    private void SetProjectileDamage()
    {
        if (channelTimer >= PlayerDataManager.Instance.PlayerDataRuntime.Ra_baseMaxChargeTime)
        {
            projectileDamage = PlayerDataManager.Instance.PlayerDataRuntime.Ra_baseMaxDamage;
        }
        else if (channelTimer >= PlayerDataManager.Instance.PlayerDataRuntime.Ra_baseMidChargeTime)
        {
            projectileDamage = PlayerDataManager.Instance.PlayerDataRuntime.Ra_baseMidDamage;
        }
        else if (channelTimer >= PlayerDataManager.Instance.PlayerDataRuntime.Ra_baseMinChargeTime)
        {
            projectileDamage = PlayerDataManager.Instance.PlayerDataRuntime.Ra_baseMinDamage;
        }
        else
        {
            projectileDamage = 0.0f;
        }
    }

    private void SetPlayerMovementSpeed()
    {
        if (channelTimer >= PlayerDataManager.Instance.PlayerDataRuntime.Ra_baseMaxChargeTime)
        {
            Player.Instance.PlayerMovement.SetMoveSpeed(PlayerDataManager.Instance.PlayerDataRuntime.Ra_basePlayerMinSpeed);
        }
        else if (channelTimer >= PlayerDataManager.Instance.PlayerDataRuntime.Ra_baseMidChargeTime)
        {
            Player.Instance.PlayerMovement.SetMoveSpeed(PlayerDataManager.Instance.PlayerDataRuntime.Ra_basePlayerMidSpeed);
        }
        else if (channelTimer >= PlayerDataManager.Instance.PlayerDataRuntime.Ra_baseMinChargeTime)
        {
            Player.Instance.PlayerMovement.SetMoveSpeed(PlayerDataManager.Instance.PlayerDataRuntime.Ra_basePlayerMaxSpeed);
        }
        else
        {
            Player.Instance.PlayerMovement.SetMoveSpeed(PlayerDataManager.Instance.PlayerDataRuntime.BaseMoveSpeed);
        }
    }

    private void ChannelHandler()
    {
        if (currentCharge != 0)
        {
            channelTimer += Time.deltaTime;

            SetPlayerMovementSpeed();
            UpdateIndicatorColor();
        }
        else
        {
            aimIndicator.color = Color.red;
        }
    }

    private void ShootHandler()
    {
        if (Player.Instance.actionState == PlayerActionState.none)
            return;

        if (channelTimer < PlayerDataManager.Instance.PlayerDataRuntime.Ra_baseMinChargeTime)
            return;

        SetProjectileSpeed();
        SetProjectileDamage();

        foreach (Transform projectile in rangeAbilityProjectilePool)
        {
            if (projectile.gameObject.activeInHierarchy == false)
            {
                AudioManager.Instance.playSFXClip(AudioManager.SFXSound.projectile);
                RangeAbilityProjectile _projectile = projectile.GetComponent<RangeAbilityProjectile>();
                _projectile.ProjectileConfig(projectileSpeed, transform, projectileDamage);
                _projectile.CanCreatePoisonPool(canCreatePoisonPool);

                projectile.gameObject.SetActive(true);
                break;
            }
        }

        Player.Instance.actionState = PlayerActionState.none;

        currentCharge--;

        // Invoke Event
        OnCurrentChargeChangedEvent?.Invoke(this, new OnCurrentChargeChangedEventArgs { currentCharge = currentCharge });
    }

    //===========================================================================
    public void UpdateAbilityParameters()
    {
        currentMaxCharge = PlayerDataManager.Instance.PlayerDataRuntime.Ra_baseMaxCharge;
        currentCharge = currentMaxCharge;
        maxRecharge = PlayerDataManager.Instance.PlayerDataRuntime.Ra_baseMaxRecharge;

        UpdateCurrentMaxCharge();
        UpdateCurrentCharge();
    }

    public void ResetAbilityCharge()
    {
        currentCharge = currentMaxCharge;

        //Invoke Event
        OnMaxChargeChangedEvent?.Invoke(this, new OnMaxChargeChangedEventArgs { maxCharge = currentMaxCharge });

        //Invoke Event
        OnCurrentChargeChangedEvent?.Invoke(this, new OnCurrentChargeChangedEventArgs { currentCharge = currentCharge });
    }

    public void UpdateCurrentMaxCharge(int amount = 0)
    {
        currentMaxCharge += amount;

        if (currentMaxCharge <= 0)
            currentMaxCharge = 0;

        if (currentCharge > currentMaxCharge)
            currentCharge = currentMaxCharge;

        //Invoke Event
        OnMaxChargeChangedEvent?.Invoke(this, new OnMaxChargeChangedEventArgs { maxCharge = currentMaxCharge });

        //Invoke Event
        OnCurrentChargeChangedEvent?.Invoke(this, new OnCurrentChargeChangedEventArgs { currentCharge = currentCharge });
    }

    public void UpdateCurrentCharge(int amount = 0)
    {
        currentCharge += amount;

        if (currentCharge <= 0)
        {
            currentCharge = 0;
        }
        else if (currentCharge > currentMaxCharge)
        {
            currentCharge = currentMaxCharge;
        }

        //Invoke Event
        OnCurrentChargeChangedEvent?.Invoke(this, new OnCurrentChargeChangedEventArgs { currentCharge = currentCharge });
    }

    public void UpdateCurrentRecharge(int amount)
    {
        currentRecharge += amount;
        if (currentRecharge > maxRecharge)
        {
            currentRecharge -= maxRecharge;
            UpdateCurrentCharge(1);
        }
    }

    public void CanCreatePoisonPool(bool active)
    {
        canCreatePoisonPool = active;
    }

    public void EmptyPool(object sender, System.EventArgs e)
    {
        foreach (Transform projectile in rangeAbilityProjectilePool)
        {
            if (projectile.gameObject.activeSelf == true)
                projectile.GetComponent<BasicAbilityBubble>().Despawn();
        }
    }
}