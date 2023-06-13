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
            aimIndicator.GetComponent<SpriteRenderer>().color = Color.grey;
            aimCharge = 0;
        }
    }

    private void Shoot()
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
        if (Input.GetMouseButtonUp(1))
        {
            float scaledSpeed = Mathf.Clamp((aimCharge / aimChargeTime) * projectileSpeed, projectileSpeed / 2, projectileSpeed);
            int scaledDamage = (int)Mathf.Clamp((aimCharge / aimChargeTime) * damage, damage / 2, damage);

            cooldownTimer = cooldown;

            Transform projectile = Instantiate(pfRangeAbilityProjectile, this.transform.position, Quaternion.identity).transform;
            projectile.GetComponent<RangeAbilityProjectile>().ProjectileConfig(rotStackApply, scaledSpeed, this.transform, scaledDamage, abilityStatusEffect);
            Player.Instance.playerActionState = PlayerActionState.none;
            aimCharge = 0;
        }
    }
}