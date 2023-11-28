using UnityEngine;

public class BombAbilityBomb : MonoBehaviour
{
    private float damage;
    private float radius;
    private float delayTime;
    private Animator animator;

    private bool manualTrigger = default;

    //===========================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (manualTrigger == false)
            return;

        if (collision.gameObject.TryGetComponent(out RangeAbilityProjectile projectile))
        {
            projectile.Despawn();
            SetExplode();
        }
    }

    //===========================================================================
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EventManager.BeforeSceneUnloadEvent += EventManager_BeforeSceneUnloadEvent;
    }

    private void Update()
    {
        if (SceneControlManager.Instance.CurrentGameplayState == GameplayState.Pause)
            return;

        if (manualTrigger == true)
            return;

        UpdateDelayTime();

        if (delayTime <= 0)
            animator.SetBool("isExploding", true);
    }

    private void OnDisable()
    {
        EventManager.BeforeSceneUnloadEvent -= EventManager_BeforeSceneUnloadEvent;
    }

    //===========================================================================
    private void EventManager_BeforeSceneUnloadEvent()
    {
        Destroy();
    }

    //===========================================================================
    private void UpdateDelayTime()
    {
        if (delayTime <= 0)
            return;

        delayTime -= Time.deltaTime;
    }

    private void TriggerBombEffect()
    {
        GetComponent<Rigidbody2D>().isKinematic = true;

        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D collider2D in collider2DArray)
        {
            if (collider2D.CompareTag("Enemy"))
                collider2D.GetComponent<EnemyHealth>().UpdateCurrentHealth(-damage);

            if (collider2D.CompareTag("FinalBoss"))
                collider2D.transform.parent.GetComponent<EnemyHealth>().UpdateCurrentHealth(-damage);
        }
    }

    private void Destroy()
    {
        animator.SetBool("isExploding", false);

        gameObject.SetActive(false);
        gameObject.transform.position = Vector3.zero;
    }

    //===========================================================================
    public void SetDamage(float newDamage) { damage = newDamage; }

    public void SetRadius(float newRadius) { radius = newRadius; }

    public void SetDelayTime(float newDelay) { delayTime = newDelay; }

    public void SetManualTrigger(bool active) { manualTrigger = active; }

    public void SetExplode() { animator.SetBool("isExploding", true); }
}