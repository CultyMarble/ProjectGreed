using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerHeartManager;

public class RangeAbility : MonoBehaviour
{
    public struct OnMaxChargeChangedEventArgs { public int maxCharge; }
    public event System.EventHandler<OnMaxChargeChangedEventArgs> OnMaxChargeChangedEvent;

    public struct OnCurrentChargeChangedEventArgs { public int currentCharge; }
    public event System.EventHandler<OnCurrentChargeChangedEventArgs> OnCurrentChargeChangedEvent;

    [Header("Effect Settings:")]
    [SerializeField] private GameObject pfRangeAbilityProjectile;
    [SerializeField] private SpriteRenderer aimIndicator;

    private int currentMaxCharge = default;
    private int currentCharge = default;

    private float projectileSpeed = default;
    private float projectileDamage = default;

    private float channelTimer = default;

    // NEW INPUT SYSTEM
    private PlayerInput playerInput;
    private bool rightButtonCheck = false;

    //===========================================================================
    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    private void OnEnable()
    {
        playerInput.actions["RightClick"].started += ActionPerformed;
        playerInput.actions["RightClick"].canceled += ActionCanceled;

        EventManager.AfterSceneLoadEvent += EventManager_AfterSceneLoadEvent;
    }

    private void Update()
    {
        if (Player.Instance.actionState == PlayerActionState.none ||
            Player.Instance.actionState == PlayerActionState.IsUsingRangeAbility)
        {
            InputHandler();
        }

        if (channelTimer > 0)
            Player.Instance.actionState = PlayerActionState.IsUsingRangeAbility;

        if (Input.GetKeyDown(KeyCode.Numlock))
        {
            UpdateCurrentCharge(1);
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

        EventManager.AfterSceneLoadEvent -= EventManager_AfterSceneLoadEvent;
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

    private void EventManager_AfterSceneLoadEvent()
    {
        currentMaxCharge = Player.Instance.PlayerData.ra_baseCharge;
        currentCharge = currentMaxCharge;

        UpdateCurrentMaxCharge();
        UpdateCurrentCharge();
    }

    //===========================================================================
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
        if (channelTimer >= Player.Instance.PlayerData.ra_baseMaxChargeTime)
        {
            aimIndicator.color = Color.green;
        }
        else if (channelTimer >= Player.Instance.PlayerData.ra_baseMidChargeTime)
        {
            aimIndicator.color = Color.cyan;
        }
        else if (channelTimer >= Player.Instance.PlayerData.ra_baseMinChargeTime)
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
        if (channelTimer >= Player.Instance.PlayerData.ra_baseMaxChargeTime)
        {
            projectileSpeed = Player.Instance.PlayerData.ra_baseMaxSpeed;
        }
        else if (channelTimer >= Player.Instance.PlayerData.ra_baseMidChargeTime)
        {
            projectileSpeed = Player.Instance.PlayerData.ra_baseMidSpeed;
        }
        else if (channelTimer >= Player.Instance.PlayerData.ra_baseMinChargeTime)
        {
            projectileSpeed = Player.Instance.PlayerData.ra_baseMinSpeed;
        }
        else
        {
            projectileSpeed = 0.0f;
        }
    }

    private void SetProjectileDamage()
    {
        if (channelTimer >= Player.Instance.PlayerData.ra_baseMaxChargeTime)
        {
            projectileDamage = Player.Instance.PlayerData.ra_baseMaxDamage;
        }
        else if (channelTimer >= Player.Instance.PlayerData.ra_baseMidChargeTime)
        {
            projectileDamage = Player.Instance.PlayerData.ra_baseMidDamage;
        }
        else if (channelTimer >= Player.Instance.PlayerData.ra_baseMinChargeTime)
        {
            projectileDamage = Player.Instance.PlayerData.ra_baseMinDamage;
        }
        else
        {
            projectileDamage = 0.0f;
        }
    }

    private void SetPlayerMovementSpeed()
    {
        if (channelTimer >= Player.Instance.PlayerData.ra_baseMaxChargeTime)
        {
            Player.Instance.PlayerMovement.SetMoveSpeed(Player.Instance.PlayerData.ra_basePlayerMinSpeed);
        }
        else if (channelTimer >= Player.Instance.PlayerData.ra_baseMidChargeTime)
        {
            Player.Instance.PlayerMovement.SetMoveSpeed(Player.Instance.PlayerData.ra_basePlayerMidSpeed);
        }
        else if (channelTimer >= Player.Instance.PlayerData.ra_baseMinChargeTime)
        {
            Player.Instance.PlayerMovement.SetMoveSpeed(Player.Instance.PlayerData.ra_basePlayerMaxSpeed);
        }
        else
        {
            Player.Instance.PlayerMovement.SetMoveSpeed(6.0f);
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
        if (channelTimer < Player.Instance.PlayerData.ra_baseMinChargeTime)
            return;

        SetProjectileSpeed();
        SetProjectileDamage();

        RangeAbilityProjectile projectile = Instantiate(pfRangeAbilityProjectile, this.transform.position, Quaternion.identity).
            GetComponent<RangeAbilityProjectile>();

        projectile.ProjectileConfig(projectileSpeed, this.transform, projectileDamage);

        Player.Instance.actionState = PlayerActionState.none;

        currentCharge--;
        // Invoke Event
        OnCurrentChargeChangedEvent?.Invoke(this, new OnCurrentChargeChangedEventArgs { currentCharge = currentCharge });
    }

    //===========================================================================
    public void ResetAbilityCharge()
    {
        currentMaxCharge = Player.Instance.PlayerData.ra_baseCharge;
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
}