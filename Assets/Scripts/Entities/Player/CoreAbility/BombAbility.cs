using UnityEngine;
using UnityEngine.InputSystem;

public class BombAbility : MonoBehaviour
{
    public struct OnChargeChangedEventArgs { public float charge; }
    public event System.EventHandler<OnChargeChangedEventArgs> OnChargeChangedEvent;
    [SerializeField] public bool throwBomb = false;
    private int currentCharge = default;
    private float damage = default;
    private float radius = default;
    private float delayTime = default;
    private readonly float inputDelayDuration = 0.5f;
    private float inputDelayTimer = default;

    // Pooling
    [Header("Pooling Settings:")]
    [SerializeField] private Transform bombAbilityBombPool = default;
    [SerializeField] private Transform pfBombAbilityBomb = default;
    private readonly int poolSize = 10;

    // NEW INPUT SYSTEM
    private PlayerInput playerInput = default;

    //===========================================================================
    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        PopulatePool();
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
    private void PopulatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Instantiate(pfBombAbilityBomb, bombAbilityBombPool).gameObject.SetActive(false);
        }
    }

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
        foreach (Transform bomb in bombAbilityBombPool)
        {
            if (bomb.gameObject.activeInHierarchy == false)
            {
                BombAbilityBomb _bomb = bomb.GetComponent<BombAbilityBomb>();
                _bomb.SetDamage(damage);
                _bomb.SetRadius(radius);
                _bomb.SetDelayTime(delayTime);

                bomb.gameObject.SetActive(true);

                if (throwBomb)
                {
                    Vector3 mouseDir = transform.parent.GetComponent<PlayerMovement>().GetMouseDirection().normalized;
                    bomb.position = transform.position + 1.5f * (mouseDir);
                    Vector2 velocity = mouseDir * 10f;
                    bomb.gameObject.GetComponent<Rigidbody2D>().velocity = velocity;
                }
                else
                {
                    bomb.position = transform.position;
                }

                UpdateBombCharge(-1);

                break;
            }
        }
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
}