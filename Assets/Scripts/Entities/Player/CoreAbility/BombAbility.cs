using UnityEngine;
using UnityEngine.InputSystem;

public class BombAbility : PlayerAbility
{
    [Header("Effect Settings:")]
    [SerializeField] private GameObject pfBomb;

    [Header("Ability Settings:")]
    [SerializeField] private int rotStackApply;
    [SerializeField] private float fuseTime;
    private float placeTime = 0.1f;

    public bool canUseAbility = default;

    //===========================================================================
    // NEW INPUT SYSTEM

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    //===========================================================================
    protected override void Update()
    {
        base.Update();

        if (canUseAbility == false)
            return;

        switch (Player.Instance.actionState)
        {
            case PlayerActionState.none:
                InputHandler();
                break;
            case PlayerActionState.IsUsingBombAbility:
                PlaceBomb();
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        CultyMarbleHelper.RotateGameObjectToMouseDirection(this.transform);
    }

    //===========================================================================
    private void InputHandler()
    {
        if (playerInput.actions["Bomb"].triggered && cooldownTimer == 0)
        {
            Player.Instance.actionState = PlayerActionState.IsUsingBombAbility;
        }
        else
        {
            Player.Instance.actionState = PlayerActionState.none;
        }
    }

    private void PlaceBomb()
    {
        if (placeTime <= 0)
        {
            Transform bomb = Instantiate(pfBomb, transform.position, Quaternion.identity).transform;
            // bomb.GetComponent<BombObject>().SetBombConfig(damage, pushPower, pushRadius, fuseTime, abilityStatusEffect);
            Player.Instance.actionState = PlayerActionState.none;
            placeTime = 0.1f;
        }
        else
        {
            placeTime -= Time.deltaTime;
        }
    }
}