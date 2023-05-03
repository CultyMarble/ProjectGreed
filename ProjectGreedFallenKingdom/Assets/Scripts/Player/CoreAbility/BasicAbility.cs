using UnityEngine;

public class BasicAbility : CoreAbility
{
    [Header("Particle Pool:")]
    [SerializeField] private Transform basicAbilityProjectilePool;

    [Header("Particle Settings:")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float lifeTime;

    [SerializeField] private float timeUntilChangeDirectionMax;
    [SerializeField] private float timeUntilChangeDirectionMin;
    [SerializeField] private float swingMagtitude;

    //===========================================================================

    protected override void Update()
    {
        base.Update();

        BasicAbilityInputHandler();
    }

    private void FixedUpdate()
    {
        CultyMarbleHelper.RotateGameObjectToMouseDirection(this.transform);
    }

    //===========================================================================
    private void BasicAbilityInputHandler()
    {
        if (Input.GetMouseButton(0) && cooldownTimer == 0)
        {
            SpawnParticle();
            cooldownTimer = cooldown;
        }
    }

    private void SpawnParticle()
    {
        foreach (Transform particle in basicAbilityProjectilePool)
        {
            if (particle.gameObject.activeInHierarchy == false)
            {
                particle.GetComponent<SprayParticleProjectile>().ConfigParticleMovementSpeed(moveSpeed, lifeTime);
                particle.GetComponent<SprayParticleProjectile>().
                    ConfigParticleMovementPattern(timeUntilChangeDirectionMax, timeUntilChangeDirectionMin, swingMagtitude);

                particle.position = this.transform.position;
                particle.gameObject.SetActive(true);
                break;
            }
        }
    }
}