using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public struct OnHealthChangedEventArgs { public float currentHealth; }
    public event EventHandler<OnHealthChangedEventArgs> OnHealthChanged;

    public struct OnMaxHealthChangedEventArgs { public float currentMaxHealth; }
    public event EventHandler<OnMaxHealthChangedEventArgs> OnMaxHealthChangedEvent;

    public event EventHandler OnDespawnEvent;

    private readonly int maxHealth = 3;

    private int currentMaxHealth;
    private int currentHealth;

    private float feedbackDamageTime = 0.10f;
    private float feedbackDamageTimer = default;

    //======================================================================
    private void OnEnable()
    {
        ResetPlayerHealth();
    }

    private void Update()
    {
        UpdateDamageFeedBackTimer();

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            UpdateCurrentHealth(-1);
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

    public void UpdateCurrentMaxHealth(int amount = 0)
    {
        currentMaxHealth += amount;
        if (currentMaxHealth <= 0)
            currentMaxHealth = 0;

        if (currentHealth > currentMaxHealth)
            currentHealth = currentMaxHealth;

        //Invoke Event
        OnMaxHealthChangedEvent?.Invoke(this, new OnMaxHealthChangedEventArgs { currentMaxHealth = currentMaxHealth });

        //Invoke Event
        OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs { currentHealth = currentHealth });
    }

    public void UpdateCurrentHealth(int amount = 0)
    {
        if (amount != 0)
        {
            currentHealth += amount;
            if (currentHealth <= 0)
                currentHealth = 0;
            else if (currentHealth > currentMaxHealth)
                currentHealth = currentMaxHealth;

            if (amount < 0)
                DamageFeedBack();

            if (currentHealth <= 0)
                Despawn();
        }

        //Invoke Event
        OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs { currentHealth = currentHealth });
    }
}