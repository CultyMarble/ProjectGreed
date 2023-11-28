using UnityEngine;
using UnityEngine.InputSystem;

public class BombAbility : MonoBehaviour
{
    public struct OnChargeChangedEventArgs { public float charge; }
    public event System.EventHandler<OnChargeChangedEventArgs> OnChargeChangedEvent;

    [SerializeField] private Transform bombAbilityBombPool = default;
    [SerializeField] private Transform pfBombAbilityBomb = default;

    [SerializeField] public bool throwBomb = false;
    [HideInInspector] public bool infAmmo = false;

    private int currentCharge = default;
    private float damage = default;
    private float radius = default;
    private float delayTime = default;
    private readonly float inputDelayDuration = 0.5f;
    private float inputDelayTimer = default;

    // Ability Upgrade
    private bool bombManualTrigger = default;

    // NEW INPUT SYSTEM
    private PlayerInput playerInput = default;

    //===========================================================================
    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    private void Update()
    {
        if (SceneControlManager.Instance.CurrentGameplayState == GameplayState.Pause)
            return;

        UpdateInputDelay();

        if (Player.Instance.actionState == PlayerActionState.none ||
            Player.Instance.actionState == PlayerActionState.IsDashing)
        {
            InputHandler();
        }
    }

    //===========================================================================
    private void UpdateInputDelay()
    {
        if (inputDelayTimer <= 0)
            return;

        inputDelayTimer -= Time.deltaTime;
    }

    private void InputHandler()
    {
        if (inputDelayTimer > 0)
            return;

        if (playerInput.actions["Bomb"].triggered && currentCharge != 0)
        {
            PlaceBomb();

            inputDelayTimer = inputDelayDuration;
        }
    }

    private void PlaceBomb()
    {
        BombAbilityBomb _bomb = Instantiate(pfBombAbilityBomb, bombAbilityBombPool).GetComponent<BombAbilityBomb>();

        if (bombManualTrigger == true)
        {
            _bomb.SetManualTrigger(bombManualTrigger);

            _bomb.SetDamage(damage * 2);
            _bomb.SetRadius(radius * 2);

            _bomb.transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);
        }
        else
        {
            _bomb.SetDamage(damage);
            _bomb.SetRadius(radius);
            _bomb.SetDelayTime(delayTime);
        }

        _bomb.gameObject.SetActive(true);

        if (throwBomb)
        {
            Vector3 mouseDir = transform.parent.GetComponent<PlayerMovement>().GetMouseDirection().normalized;
            _bomb.transform.position = transform.position + 1.5f * (mouseDir);
            Vector2 velocity = mouseDir * 10f;
            _bomb.gameObject.GetComponent<Rigidbody2D>().velocity = velocity;
        }
        else
        {
            _bomb.transform.position = transform.position;
        }

        if (!infAmmo)
            UpdateBombCharge(-1);
    }

    //===========================================================================
    public void UpdateAbilityParameters()
    {
        currentCharge = PlayerDataManager.Instance.PlayerDataRuntime.Bomb_baseCharge;
        damage = PlayerDataManager.Instance.PlayerDataRuntime.Bomb_baseDamage;
        radius = PlayerDataManager.Instance.PlayerDataRuntime.Bomb_baseRadius;
        delayTime = PlayerDataManager.Instance.PlayerDataRuntime.Bomb_baseDelayTime;

        UpdateBombCharge(3);
    }

    public void UpdateBombCharge(int amount)
    {
        currentCharge += amount;
        if (currentCharge < 0)
            currentCharge = 0;

        // Invoke Event
        OnChargeChangedEvent?.Invoke(this, new OnChargeChangedEventArgs { charge = currentCharge });
    }

    public void SetBombManualTrigger(bool active)
    {
        bombManualTrigger = active;
    }
}