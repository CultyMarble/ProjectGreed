using UnityEngine;
using UnityEngine.InputSystem;

public class BombAbility : CoreAbility
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

    private GreedControls input = null;
    private bool bombButtonCheck = false;
    private bool bombPlaced = false;

    private void Awake()
    {
        input = new GreedControls();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Bomb.performed += ActionPerformed;
        input.Player.Bomb.canceled += ActionCanceled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Bomb.performed -= ActionPerformed;
        input.Player.Bomb.canceled -= ActionCanceled;
    }

    private void ActionPerformed(InputAction.CallbackContext obj)
    {
        bombButtonCheck = true;
    }

    private void ActionCanceled(InputAction.CallbackContext obj)
    {
        bombButtonCheck = false;
    }

    //===========================================================================
    protected override void Update()
    {
        base.Update();

        if (canUseAbility == false)
            return;

        switch (Player.Instance.playerActionState)
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
        if (bombButtonCheck && !bombPlaced && cooldownTimer == 0)
        {
            Player.Instance.playerActionState = PlayerActionState.IsUsingBombAbility;
            bombPlaced = true;
        }
        else if (!bombButtonCheck)
        {
            Player.Instance.playerActionState = PlayerActionState.none;
            bombPlaced = false;
        }
    }

    private void PlaceBomb()
    {
        if (placeTime <= 0)
        {
            Transform bomb = Instantiate(pfBomb, transform.position, Quaternion.identity).transform;
            bomb.GetComponent<BombObject>().SetBombConfig(damage, pushPower, pushRadius, fuseTime, abilityStatusEffect);
            Player.Instance.playerActionState = PlayerActionState.none;
            placeTime = 0.1f;
        }
        else
        {
            placeTime -= Time.deltaTime;
        }
    }
}
