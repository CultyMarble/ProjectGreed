using UnityEngine;

public class BasicAbility : MonoBehaviour
{
    [Header("Particle Pool:")]
    [SerializeField] private Transform basicAbilityProjectilePool;
    [SerializeField] private Transform pfSprayParticleProjectile;
    [SerializeField] private int particlePoolAmount;

    [Header("Particle Settings:")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float lifeTime;

    [SerializeField] private float timeUntilChangeDirectionMax;
    [SerializeField] private float timeUntilChangeDirectionMin;
    [SerializeField] private float swingMagtitude;

    [Header("Ability Settings:")]
    [SerializeField] private float coolDown;
    [SerializeField] public int damage;
    [SerializeField] private float pushPower;

    private float cooldownTimer = default;

    //===========================================================================
    private void Awake()
    {
        // Create Pool of Particle Effect
        for (int i = 0; i < particlePoolAmount; i++)
        {
            Transform _particle = Instantiate(pfSprayParticleProjectile, basicAbilityProjectilePool);
            _particle.localPosition = Vector2.zero;
            _particle.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        BasicAbilityCooldown();

        BasicAbilityInputHandler();
    }

    private void FixedUpdate()
    {
        CultyMarbleHelper.RotateGameObjectToMouseDirection(this.transform);
    }

    //===========================================================================
    private void BasicAbilityCooldown()
    {
        if (cooldownTimer > 0.0f)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer < 0.0f)
                cooldownTimer = 0.0f;
        }
    }

    private void BasicAbilityInputHandler()
    {
        if (Input.GetMouseButton(0) && cooldownTimer == 0)
        {
            SpawnParticle();
            cooldownTimer = coolDown;
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