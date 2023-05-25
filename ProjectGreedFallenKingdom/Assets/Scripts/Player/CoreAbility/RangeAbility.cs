using UnityEngine;

public class RangeAbility : CoreAbility
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
        if (Input.GetMouseButtonDown(1) && cooldownTimer == 0)
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
        aimCharge += Time.deltaTime;
        aimIndicator.GetComponent<SpriteRenderer>().color = Color.yellow;
        if (Input.GetMouseButtonUp(1) && aimCharge < aimChargeTime)
        {
            Player.Instance.playerActionState = PlayerActionState.none;
            aimCharge = 0;
            return;
        }
        if (aimCharge > aimChargeTime)
        {
            aimIndicator.GetComponent<SpriteRenderer>().color = Color.green;
        }
        if (Input.GetMouseButtonUp(1))
        {
            cooldownTimer = cooldown;

            Transform projectile = Instantiate(pfRangeAbilityProjectile, this.transform.position, Quaternion.identity).transform;
            projectile.GetComponent<RangeAbilityProjectile>().ProjectileConfig(rotStackApply, projectileSpeed, this.transform, damage);
            Player.Instance.playerActionState = PlayerActionState.none;
            aimCharge = 0;
        }
    }
}