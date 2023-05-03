using UnityEngine;

public class RangeAbility : MonoBehaviour
{
    [Header("Effect Settings:")]
    [SerializeField] private GameObject pfRangeAbilityProjectile;

    [Header("Ability Settings:")]
    [SerializeField] private float rangeCD;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private int rotStackApply;

    private float rangeCDcounter;

    //===========================================================================
    void Update()
    {
        AbilityCooldown();

        switch (Player.Instance.playerActionState)
        {
            case PlayerActionState.none:
                InputHandler();
                CultyMarbleHelper.RotateGameObjectToMouseDirection(this.transform);
                break;

            default:
                break;
        }
    }

    //===========================================================================
    private void InputHandler()
    {
        if (Input.GetKeyDown(KeyCode.E) && rangeCDcounter == 0)
        {
            rangeCDcounter = rangeCD;

            Transform projectile = Instantiate(pfRangeAbilityProjectile, this.transform.position, Quaternion.identity).transform;
            projectile.GetComponent<RangeAbilityProjectile>().ProjectileConfig(rotStackApply, projectileSpeed, this.transform);
        }
    }

    private void AbilityCooldown()
    {
        if (rangeCDcounter > 0)
        {
            rangeCDcounter -= Time.deltaTime;

            if (rangeCDcounter < 0)
                rangeCDcounter = 0;
        }
    }
}