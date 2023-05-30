using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public struct OnHealthChangedEvenArgs
    {
        public float currentHealth;
        public float healthRatio;
    }
    public event EventHandler<OnHealthChangedEvenArgs> OnHealthChanged;

    public event EventHandler OnDespawnEvent;

    [SerializeField] private float maxHealth;

    private float currentHealth;

    private float feedbackDamageTime = 0.10f;
    private float feedbackDamageTimer = default;

    //======================================================================
    private void Awake()
    {
        currentHealth = maxHealth;
        UpdateCurrentHealth();
    }

    private void Update()
    {
        UpdateDamageFeedBackTimer();

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            UpdateCurrentHealth(-10);
        }
    }

    //======================================================================
    private void DamageFeedBack()
    {
        // Health Feedback
        this.gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 0, 0);
        feedbackDamageTimer = feedbackDamageTime;
    }

    private void UpdateDamageFeedBackTimer()
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

    private void Despawn()
    {
        // Call OnDestroy Event
        OnDespawnEvent?.Invoke(this, EventArgs.Empty);

        // Reset Parameters
        currentHealth = maxHealth;
        gameObject.SetActive(false);
    }

    //======================================================================
    public void UpdateCurrentHealth(int amount = 0)
    {
        if (amount != 0)
        {
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0.0f, maxHealth);

            if (amount < 0)
                DamageFeedBack();

            // Call OnHitPointChanged Event
            OnHealthChanged?.Invoke(this, new OnHealthChangedEvenArgs
            {
                currentHealth = currentHealth,
                healthRatio = currentHealth / maxHealth
            });

            if (currentHealth <= 0)
                Despawn();
        }
    }
}