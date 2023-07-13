using UnityEngine;

public class BombAbility : CoreAbility
{
    [Header("Effect Settings:")]
    [SerializeField] private GameObject pfBomb;

    [Header("Ability Settings:")]
    [SerializeField] private int rotStackApply;
    [SerializeField] private float fuseTime;
    private float placeTime = 0.1f;

    [HideInInspector] public bool canUseAbility = default;

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
        if (Input.GetKeyDown(KeyCode.G) && cooldownTimer == 0)
        {
            Player.Instance.playerActionState = PlayerActionState.IsUsingBombAbility;
        }
        else
        {
            Player.Instance.playerActionState = PlayerActionState.none;
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
        else {
            placeTime -= Time.deltaTime;
        }
    }
}
