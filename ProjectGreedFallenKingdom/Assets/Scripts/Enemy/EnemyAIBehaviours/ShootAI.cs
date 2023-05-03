using UnityEngine;

[RequireComponent(typeof(TargetingAI))]
public class ShootAI : MonoBehaviour
{
    [SerializeField] private Transform pfEnemyBullet;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int shootTimes;
    [SerializeField] private float shootDelayInterval;
    [SerializeField] private float shootCoolDown;

    private TargetingAI targetingAI;
    private float shootCoolDownTimer;
    private float shootDelayIntervalTimer;
    private int shootTimeCounter;
    private Vector3 bulletDirection;
    private Vector3 recordedBulletDirection;
    private bool isShooting;

    //===========================================================================
    private void Awake()
    {
        targetingAI = GetComponent<TargetingAI>();
    }

    private void Start()
    {
        isShooting = false;

        shootCoolDownTimer = shootCoolDown;

        shootTimeCounter = 0;
    }

    private void Update()
    {
        ShootCoolDown();
        Shoot();
    }

    //===========================================================================
    private void ShootCoolDown()
    {
        if (targetingAI.currentTargetTransform == null || isShooting == true)
            return;

        shootCoolDownTimer -= Time.deltaTime;
        if (shootCoolDownTimer <= 0.0f)
        {
            isShooting = true;
            shootDelayIntervalTimer = shootDelayInterval;
            shootCoolDownTimer = shootCoolDown;
            shootTimeCounter = shootTimes;
        }
    }

    private void Shoot()
    {
        if (shootTimeCounter == 0)
            return;

        shootDelayIntervalTimer -= Time.deltaTime;
        if (shootDelayIntervalTimer <= 0.0f)
        {
            shootDelayIntervalTimer += shootDelayInterval;

            SpawnBullet();

            --shootTimeCounter;
            if (shootTimeCounter == 0)
            {
                isShooting = false;
            }
        }
    }

    private void SpawnBullet()
    {
        if (targetingAI.currentTargetTransform != null)
        {
            bulletDirection = (targetingAI.currentTargetTransform.position - transform.position).normalized;
            recordedBulletDirection = bulletDirection;
        }

        Transform prefabBullet = Instantiate(pfEnemyBullet, transform);

        prefabBullet.GetComponent<PrefabEnemyBullet>().SetMoveDirectionAndSpeed(recordedBulletDirection, bulletSpeed);

        Destroy(prefabBullet.gameObject, 1.5f);
    }
}
