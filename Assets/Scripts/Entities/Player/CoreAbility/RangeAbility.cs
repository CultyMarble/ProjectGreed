using UnityEngine;
using UnityEngine.InputSystem;

public class RangeAbility : PlayerAbility
{
    [Header("Effect Settings:")]
    [SerializeField] private GameObject pfRangeAbilityProjectile;
    [SerializeField] private GameObject aimIndicator;

    [Header("Ability Settings:")]
    [SerializeField] private int rotStackApply;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float aimChargeTime;

    private float aimCharge;

    //===========================================================================
    // NEW INPUT SYSTEM

    private PlayerInput playerInput;
    private bool rightButtonCheck = false;

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    private void OnEnable()
    {
        playerInput.actions["RightClick"].started += ActionPerformed;
        playerInput.actions["RightClick"].canceled += ActionCanceled;
    }

    private void OnDisable()
    {
        playerInput.actions["RightClick"].started -= ActionPerformed;
        playerInput.actions["RightClick"].canceled -= ActionCanceled;
    }

    private void ActionPerformed(InputAction.CallbackContext obj)
    {
        rightButtonCheck = true;
    }

    private void ActionCanceled(InputAction.CallbackContext obj)
    {
        rightButtonCheck = false;
    }

    //===========================================================================

    protected override void Update()
    {
        base.Update();

        switch (Player.Instance.playerActionState)
        {
            case PlayerActionState.none:
                aimIndicator.SetActive(false);
                InputHandler();
                break;
            case PlayerActionState.IsUsingRangeAbility:
                aimIndicator.SetActive(true);
                Shoot();
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
        if (rightButtonCheck)
        {
            Player.Instance.playerActionState = PlayerActionState.IsUsingRangeAbility;
            aimCharge += Time.deltaTime;
        }
        else
        {
            Player.Instance.playerActionState = PlayerActionState.none;
            aimCharge = 0;
        }
    }

    private void Shoot()
    {
        if (cooldownTimer == 0)
        {
            if (aimCharge + Time.deltaTime <= aimChargeTime)
            {
                aimCharge += Time.deltaTime;
            }
            else
            {
                aimCharge = aimChargeTime;
            }

            aimIndicator.GetComponent<SpriteRenderer>().color = Color.yellow;

            if (aimCharge >= aimChargeTime)
            {
                aimIndicator.GetComponent<SpriteRenderer>().color = Color.green;
            }
            if (!rightButtonCheck)
            {
                float scaledSpeed = Mathf.Clamp((aimCharge / aimChargeTime) * projectileSpeed, projectileSpeed / 2, projectileSpeed);
                // int scaledDamage = (int)Mathf.Clamp((aimCharge / aimChargeTime) * damage, damage / 2, damage);

                cooldownTimer = cooldown;

                Transform projectile = Instantiate(pfRangeAbilityProjectile, this.transform.position, Quaternion.identity).transform;
                Player.Instance.playerActionState = PlayerActionState.none;
                aimCharge = 0;
            }
        }
        else
        {
            aimIndicator.GetComponent<SpriteRenderer>().color = Color.red;
            if (!rightButtonCheck)
            {
                Player.Instance.playerActionState = PlayerActionState.none;
                aimCharge = 0;
            }
        }
    }

}