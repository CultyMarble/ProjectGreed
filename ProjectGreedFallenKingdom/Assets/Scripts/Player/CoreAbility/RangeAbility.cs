using UnityEngine;

public class RangeAbility : CoreAbility
{
    [Header("Effect Settings:")]
    [SerializeField] private GameObject pfRangeAbilityProjectile;

    [Header("Ability Settings:")]
    [SerializeField] private int rotStackApply;
    [SerializeField] private float projectileSpeed;

    //===========================================================================
    protected override void Update()
    {
        base.Update();

        switch (Player.Instance.playerActionState)
        {
            case PlayerActionState.none:
                InputHandler();
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
        if (Input.GetKeyDown(KeyCode.E) && cooldownTimer == 0)
        {
            cooldownTimer = cooldown;

            Transform projectile = Instantiate(pfRangeAbilityProjectile, this.transform.position, Quaternion.identity).transform;
            projectile.GetComponent<RangeAbilityProjectile>().ProjectileConfig(rotStackApply, projectileSpeed, this.transform);
        }
    }
}