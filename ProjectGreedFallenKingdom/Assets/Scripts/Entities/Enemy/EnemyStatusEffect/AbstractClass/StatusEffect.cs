using UnityEngine;

[RequireComponent(typeof(EnemyStatusEffect))]
public abstract class StatusEffect : MonoBehaviour
{
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
        enemyStatusEffect = GetComponent<EnemyStatusEffect>();
    }

    protected virtual void Update()
    {
        if (active == false)
            return;

        UpdateTriggerInterval();
        UpdateStatusDuration();
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
        active = false;
        enemyStatusEffect.EffectVFX.sprite = null;
        stackAmount = 0;
    }
}