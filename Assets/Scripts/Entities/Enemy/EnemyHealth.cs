using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public struct OnHealthChangedEvenArgs { public float healthRatio; }
    public event EventHandler<OnHealthChangedEvenArgs> OnHealthChanged;

    public event EventHandler OnDespawnEvent;

    [SerializeField] private float maxHealth;

    public float currentHealth;

    private float feedbackDamageTime = 0.10f;
    private float feedbackDamageTimer = default;
    private SpawnCurrency spawnCurrency;

    //======================================================================
    private void Awake()
    {
        currentHealth = maxHealth;
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
    }

    //======================================================================
    private void DamageFeedBack()
    {
        // Health Feedback
        this.gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 0, 0);
        feedbackDamageTimer = feedbackDamageTime;
    }

    public void Despawn()
    {
        // Call OnDestroy Event
        OnDespawnEvent?.Invoke(this, EventArgs.Empty);

        // Reset Parameters
        currentHealth = maxHealth;
        
        OnHealthChanged?.Invoke(this, new OnHealthChangedEvenArgs { healthRatio = currentHealth / maxHealth });
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
            currentHealth = Mathf.Clamp(currentHealth, 0.0f, maxHealth);

            if (amount < 0)
                DamageFeedBack();

            // Call OnHitPointChanged Event
            OnHealthChanged?.Invoke(this, new OnHealthChangedEvenArgs { healthRatio = currentHealth / maxHealth });

            if (currentHealth <= 0)
            {
                GetComponent<Enemy>().ResetStatusEffects();
                Despawn();
            }
        }
    }

    public float GetHealthPercentage()
    {
        return (currentHealth / maxHealth) * 100.0f;
    }
}