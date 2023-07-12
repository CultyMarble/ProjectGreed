using UnityEngine;

[RequireComponent(typeof(EnemyStatusEffect))]
public abstract class StatusEffect : MonoBehaviour
{
    protected EnemyHealth enemyHealth;
    [SerializeField] protected Sprite effectIcon;

    protected EnemyStatusEffect enemyStatusEffect = default;

    private bool active = default;
    protected int stackAmount = default;

    protected float triggerInterval;
    private float triggerIntervalTimer = default;

    protected float statusDuration;
    private float statusDurationTimer = default;

    //===========================================================================
    protected virtual void Awake()
    {
        enemyHealth = transform.parent.GetComponentInParent<EnemyHealth>();

        enemyStatusEffect = GetComponent<EnemyStatusEffect>();
        enemyHealth.OnDespawnEvent += EnemyHealth_OnDespawnEvent;
    }

    protected virtual void Update()
    {
        if (active == false)
            return;

        UpdateTriggerInterval();
        UpdateStatusDuration();
    }

    private void OnDisable()
    {
        if (enemyHealth != null)
            enemyHealth.OnDespawnEvent -= EnemyHealth_OnDespawnEvent;
    }

    //===========================================================================
    private void EnemyHealth_OnDespawnEvent(object sender, System.EventArgs e)
    {
        Deactivate();
    }

    //===========================================================================
    private void UpdateTriggerInterval()
    {
        triggerIntervalTimer -= Time.deltaTime;
        if (triggerIntervalTimer <= 0)
        {
            triggerIntervalTimer += triggerInterval;
            TriggerHandler();
        }
    }

    private void UpdateStatusDuration()
    {
        statusDurationTimer -= Time.deltaTime;
        if (statusDurationTimer <= 0)
        {
            active = false;
            stackAmount = 0;
            enemyStatusEffect.EffectVFX.sprite = null;
        }
    }

    //===========================================================================
    protected abstract void TriggerHandler();

    protected abstract void OverstackHandler();

    //===========================================================================
    public void Activate(float newTriggerInterval = 0.01f, float newStatusDuration = 3.0f, int _stackAmount = 1)
    {
        active = true;

        triggerInterval = newTriggerInterval;
        triggerIntervalTimer = triggerInterval;

        statusDuration = newStatusDuration;
        statusDurationTimer = statusDuration;

        enemyStatusEffect.EffectVFX.sprite = effectIcon;
        stackAmount+= _stackAmount;
    }

    public bool CheckActive()
    {
        if (active)
        {
            return true;
        }
        return false;
    }

    public void Deactivate()
    {
        enemyStatusEffect.EffectVFX.sprite = null;

        active = false;
        stackAmount = 0;

        triggerInterval = 0.0f;
        triggerIntervalTimer = 0.0f;

        statusDuration = 0.0f;
        statusDurationTimer = 0.0f;
    }
}