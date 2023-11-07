using UnityEngine;

public class FinalBossTornadoBulletAI : FinalBossAttackAI
{
    [SerializeField] private int triggerTime = default;
    private int triggerTimeCounter = default;

    [Header("Tornado Behaviour")]
    [SerializeField] private Transform pfProjectile = default;
    [SerializeField] private Transform projectileParent = default;
    [SerializeField] private Transform shootingPoint = default;
    [SerializeField] private int projectileAmount = default;
    [SerializeField] private float bulletSpeed = default;

    //======================================================================
    private void OnEnable()
    {
        triggerTimeCounter = triggerTime;
    }

    public override void TriggerAttack()
    {
        triggerTimeCounter = triggerTime;
        animator.SetBool("TornadoBulletAttack", true);
    }

    public void SetTriggerTime(int amount)
    {
        triggerTime = amount;
    }

    //======================================================================
    public void AnimEvent_TonadoBulletAttack()
    {
        // Rotate shooting point to player
        Vector3 _toPlayerDirection = (Player.Instance.transform.position - transform.position).normalized;
        float _zEulerAngle = CultyMarbleHelper.GetAngleFromVector(_toPlayerDirection);
        shootingPoint.eulerAngles = new Vector3(0.0f, 0.0f, _zEulerAngle);

        for (int i = 0; i < projectileAmount; i++)
        {
            Transform _projectile = Instantiate(pfProjectile, projectileParent);
            _projectile.position = transform.position;

            Vector2 _moveDirection = (shootingPoint.GetChild(i).transform.position - shootingPoint.transform.position).normalized;
            _projectile.GetComponent<EnemyProjectile>().SetMoveDirectionAndSpeed(_moveDirection, bulletSpeed);
        }

        triggerTimeCounter--;
        if (triggerTimeCounter == 0)
        {
            animator.SetBool("TornadoBulletAttack", false);
            GetComponent<FinalBossFirstForm>().SetCooldownTimer();
        }
    }
}