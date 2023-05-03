using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public float maxHealth;

    public struct OnHitPointChangedEvenArgs
    {
        public float currentHealth;
    }
    public event EventHandler<OnHitPointChangedEvenArgs> OnHealthChanged;

    public event EventHandler OnDespawnEvent;

    private float currentHealth;

    private float feedbackDamageTime = 0.10f;
    private float feedbackDamageTimer = default;

    //======================================================================
    private void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(this, new OnHitPointChangedEvenArgs
        {
            currentHealth = currentHealth
        });
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
    public void UpdateCurrentHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0.0f, maxHealth);

        // Call OnHitPointChanged Event
        OnHealthChanged?.Invoke(this, new OnHitPointChangedEvenArgs { currentHealth = currentHealth});

        if (currentHealth <= 0)
            Despawn();
    }

    public float GetCurrentHeartPoint()
    {
        return currentHealth;
    }

    public float GetHealthRatio()
    {
        return (float)currentHealth / maxHealth;
    }

    public void Despawn()
    {
        // Call OnDestroy Event
        OnDespawnEvent?.Invoke(this, EventArgs.Empty);

        // Reset Parameters
        currentHealth = maxHealth;
        gameObject.SetActive(false);
    }

    public void DamageFeedBack()
    {
        // Health Feedback
        this.gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 0, 0);
        feedbackDamageTimer = feedbackDamageTime;
    }
}