using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public struct OnHealthChangedEvenArgs { public float healthRatio; }
    public event EventHandler<OnHealthChangedEvenArgs> OnHealthChanged;

    public event EventHandler OnDespawnEvent;

    [SerializeField] public float baseMaxHealth;

    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentMaxHealth;

    private float feedbackDamageTime = 0.10f;
    private float feedbackDamageTimer = default;
    private float damageAudioDelayTime = 0.10f;
    private float damageAudioDelayTimer = default;
    private SpawnCurrency spawnCurrency;

    //======================================================================
    private void Awake()
    {
        currentMaxHealth = baseMaxHealth;
        currentHealth = currentMaxHealth;
        spawnCurrency = GetComponent<SpawnCurrency>();

        UpdateCurrentHealth();
    }

    private void Update()
    {
        if (feedbackDamageTimer > 0)
        {
            feedbackDamageTimer -= Time.deltaTime;
            if (feedbackDamageTimer <= 0)
            {
                feedbackDamageTimer = 0;
                this.gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 255, 255);
            }
        }
        if (damageAudioDelayTimer > 0)
        {
            damageAudioDelayTimer -= Time.deltaTime;
            if (damageAudioDelayTimer <= 0)
            {
                damageAudioDelayTimer = 0;
            }
        }
    }

    //======================================================================
    private void DamageFeedBack()
    {
        // Health Feedback
        gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 0, 0);
        feedbackDamageTimer = feedbackDamageTime;
        if(damageAudioDelayTimer <= 0)
        {
            AudioManager.Instance.playSFXClip(AudioManager.SFXSound.enemyDamage);
            damageAudioDelayTimer = damageAudioDelayTime;
        }
    }

    public void Despawn()
    {
        // Call OnDestroy Event
        OnDespawnEvent?.Invoke(this, EventArgs.Empty);

        // Reset Parameters
        currentMaxHealth = baseMaxHealth;
        currentHealth = currentMaxHealth;
        
        OnHealthChanged?.Invoke(this, new OnHealthChangedEvenArgs { healthRatio = currentHealth / currentMaxHealth });
        gameObject.SetActive(false);

        if(spawnCurrency != null)
        {
            spawnCurrency.SpewOutCurrency();
        }

        Player.Instance.GetComponentInChildren<RangeAbility>().UpdateCurrentRecharge(100);
    }

    //======================================================================
    public void UpdateCurrentHealth(float amount = 0)
    {
        if (amount != 0)
        {
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0.0f, currentMaxHealth);

            if (amount < 0)
                DamageFeedBack();

            // Call OnHitPointChanged Event
            OnHealthChanged?.Invoke(this, new OnHealthChangedEvenArgs { healthRatio = currentHealth / currentMaxHealth });

            if (currentHealth <= 0)
            {
                GetComponent<Enemy>().ResetStatusEffects();
                Despawn();
            }
        }
    }

    public float GetHealthPercentage()
    {
        return (currentHealth / currentMaxHealth) * 100.0f;
    }
    public float GetCurrenHealth()
    {
        return currentHealth;
    }
}