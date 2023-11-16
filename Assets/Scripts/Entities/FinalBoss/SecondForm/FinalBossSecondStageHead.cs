using UnityEngine;

public class FinalBossSecondStageHead : MonoBehaviour
{
    [SerializeField] private Transform pfBulletSphere = default;
    [SerializeField] private Transform parent = default;

    private readonly float cooldownMin = 7.5f;
    private readonly float cooldownMax = 10.0f;

    private float cooldownTimer = default;

    //===========================================================================
    private void Start()
    {
        cooldownTimer = Random.Range(cooldownMin, cooldownMax);
    }

    private void Update()
    {
        if (SceneControlManager.Instance.CurrentGameplayState == GameplayState.Pause)
            return;

        // Cooldown
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0.0f)
        {
            cooldownTimer = Random.Range(cooldownMin, cooldownMax);
            CreateBulletSphere();
        }
    }

    //===========================================================================
    private void CreateBulletSphere()
    {
        Transform _projectile = Instantiate(pfBulletSphere, parent);
        _projectile.position = transform.position;
    }
}