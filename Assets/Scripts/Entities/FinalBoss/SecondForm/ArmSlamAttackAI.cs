using UnityEngine;

public class ArmSlamAttackAI : FinalBossAttackAI
{
    [SerializeField] private Transform parent = default;

    [Header("Slam")]
    [SerializeField] private Transform pfShockWave = default;
    private bool triggerShockwave = default;

    [Header("Bullet Wave")]
    [SerializeField] private Transform pfProjectile = default;
    [SerializeField] private Transform shootingPoint = default;
    [SerializeField] private int projectileAmount = default;
    [SerializeField] private float bulletSpeed = default;
    private bool triggerBulletWave = default;

    [Header("Trap Wave")]
    [SerializeField] private Transform pfBossTrap = default;
    [SerializeField] private Transform trapSpawnPoint = default;
    [SerializeField] private int trapAmount = default;
    private bool triggerTrapWave = default;

    //======================================================================
    public override void TriggerAttack()
    {
        animator.SetBool("SlamAttack", true);
    }

    //======================================================================
    public void SetTriggerShockWave(bool active) { triggerShockwave = active; }
    public void SetTriggerBulletWave(bool active) { triggerBulletWave = active; }
    public void SetTriggerTrapWave(bool active) { triggerTrapWave = active; }

    public void SetProjectileAmount(int amount) { projectileAmount = amount; }
    public void SetTrapAmount(int amount) { trapAmount = amount; }

    //======================================================================
    public void AnimEvent_SlamAttack()
    {
        // Get Direction with player
        Vector3 _direction = (Player.Instance.transform.position - transform.position).normalized;

        if (triggerShockwave)
        {
            Transform _shockwave = Instantiate(pfShockWave, parent);
            _shockwave.transform.position = transform.position;
            _shockwave.GetComponent<PrefabShockWave>().SetMoveDirection();
        }

        if (triggerBulletWave)
        {
            // Rotate shooting point to player
            Vector3 _toPlayerDirection = (Player.Instance.transform.position - transform.position).normalized;
            float _zEulerAngle = CultyMarbleHelper.GetAngleFromVector(_toPlayerDirection);
            shootingPoint.eulerAngles = new Vector3(0.0f, 0.0f, _zEulerAngle);

            for (int i = 0; i < projectileAmount; i++)
            {
                Transform _projectile = Instantiate(pfProjectile, parent);
                _projectile.position = transform.position;

                Vector2 _moveDirection = (shootingPoint.GetChild(i).transform.position - shootingPoint.transform.position).normalized;
                _projectile.GetComponent<EnemyProjectile>().SetMoveDirectionAndSpeed(_moveDirection, bulletSpeed);
            }
        }

        if (triggerTrapWave)
        {
            for (int i = 0; i < trapAmount; i++)
            {
                int _index = Random.Range(0, trapSpawnPoint.childCount);

                Transform _trap = Instantiate(pfBossTrap, parent);
                _trap.transform.position = trapSpawnPoint.GetChild(_index).transform.position;
            }
        }

        animator.SetBool("SlamAttack", false);
    }
}