using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public struct OnHealthChangedEventArgs { public float currentHealth; }
    public event EventHandler<OnHealthChangedEventArgs> OnHealthChanged;

    public struct OnMaxHealthChangedEventArgs { public float currentMaxHealth; }
    public event EventHandler<OnMaxHealthChangedEventArgs> OnMaxHealthEventChanged;

    public event EventHandler OnDespawnEvent;

    private readonly float maxHealth = 100.0f;

    private float currentMaxHealth;
    private float currentHealth;

    private float feedbackDamageTime = 0.10f;
    private float feedbackDamageTimer = default;

    //======================================================================
    private void Start()
    {
        ResetPlayerHealth();
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
        // Reset Parameters
        UpdateCurrentHealth(currentMaxHealth);
        gameObject.SetActive(false);

        // Call OnDestroy Event
        OnDespawnEvent?.Invoke(this, EventArgs.Empty);
    }

    //======================================================================
    public void ResetPlayerHealth()
    {
        currentMaxHealth = maxHealth;
        currentHealth = maxHealth;

        UpdateCurrentMaxHealth();
        UpdateCurrentHealth();
    }

    public void UpdateCurrentMaxHealth(float amount = 0)
    {
        float _healthRatio = currentHealth / currentMaxHealth;

        currentMaxHealth += amount;
        if (currentMaxHealth <= 0) currentMaxHealth = 0;

        //Invoke Event
        OnMaxHealthEventChanged?.Invoke(this, new OnMaxHealthChangedEventArgs { currentMaxHealth = currentMaxHealth });

        if (amount > 0)
        {
            float _amount = (currentMaxHealth * _healthRatio) - currentHealth;
            UpdateCurrentHealth(_amount);
        }
    }

    public void UpdateCurrentHealth(float amount = 0)
    {
        if (amount != 0)
        {
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0.0f, currentMaxHealth);

            if (amount < 0)
                DamageFeedBack();

            if (currentHealth <= 0)
                Despawn();
        }

        //Invoke Event
        OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs { currentHealth = currentHealth });
    }
}