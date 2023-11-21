using System;
using UnityEngine;

public class PlayerHeart : MonoBehaviour
{
    public struct OnHealthChangedEventArgs { public float currentHeart; }
    public event EventHandler<OnHealthChangedEventArgs> OnHeartChangedEvent;

    public struct OnMaxHealthChangedEventArgs { public float maxHeart; }
    public event EventHandler<OnMaxHealthChangedEventArgs> OnMaxHeartChangedEvent;

    public event EventHandler OnDespawnPlayerEvent;

    private int currentMaxHeart = default;
    public int CurrentMaxHeart => currentMaxHeart;

    private int currentHeart = default;

    private readonly float feedbackDamageDuration = 0.10f;
    private float feedbackDamageTimer = default;

    // Player Regen
    //private readonly float recoveryCooldown = 10.0f;
    //private float recoveryCooldownTimer = default;

    // IFrame
    [SerializeField] private Animator bodySpriteAnimator = default;
    [SerializeField] private float iFrameDuration = default;
    private float iFrameTimer = default;
    private bool damageImmune = default;

    //======================================================================
    private void OnEnable()
    {
        ResetPlayerHeart();
    }

    private void Update()
    {
        UpdateDamageFeedBackTimer();
        UpdateIFrameTimer();

        // Recovery
        //if (currentHeart < currentMaxHeart)
        //{
        //    recoveryCooldownTimer-= Time.deltaTime;
        //    if (recoveryCooldownTimer <= 0.0f)
        //    {
        //        recoveryCooldownTimer = recoveryCooldown;
        //        UpdateCurrentHeart(1);
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.Home))
        {
            Debug.Log(currentHeart + "/" + currentMaxHeart);
        }
    }

    //======================================================================
    private void TriggerDamageFeedBack()
    {
        // Health Feedback
        GetComponentInChildren<SpriteRenderer>().color = new Color(255, 0, 0);
        feedbackDamageTimer = feedbackDamageDuration;
    }

    private void UpdateDamageFeedBackTimer()
    {
        if (feedbackDamageTimer <= 0)
            return;

        feedbackDamageTimer -= Time.deltaTime;
        if (feedbackDamageTimer <= 0)
        {
            feedbackDamageTimer = 0;
            this.gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 255, 255);
        }
    }

    private void UpdateIFrameTimer()
    {
        if (iFrameTimer <= 0.0f)
            return;

        iFrameTimer -= Time.deltaTime;
        if (iFrameTimer <= 0.0f)
        {
            bodySpriteAnimator.SetBool("IFrame", false);

            damageImmune = false;
        }
    }

    private void DespawnPlayer()
    {
        // Reset Parameters
        ResetPlayerHeart();

        // Call OnDestroy Event
        OnDespawnPlayerEvent?.Invoke(this, EventArgs.Empty);

        Player.Instance.actionState = PlayerActionState.none;
        Player.Instance.gameObject.SetActive(false);
    }

    //======================================================================
    public void ResetPlayerHeart()
    {
        currentHeart = currentMaxHeart;

        //Invoke Event
        OnMaxHeartChangedEvent?.Invoke(this, new OnMaxHealthChangedEventArgs { maxHeart = currentMaxHeart });

        //Invoke Event
        OnHeartChangedEvent?.Invoke(this, new OnHealthChangedEventArgs { currentHeart = currentHeart });
    }

    public void UpdateCurrentMaxHeart(int amount = 0)
    {
        currentMaxHeart += amount;
        currentHeart += amount;

        if (currentMaxHeart <= 0)
            currentMaxHeart = 0;

        if (currentHeart > currentMaxHeart)
            currentHeart = currentMaxHeart;

        //Invoke Event
        OnMaxHeartChangedEvent?.Invoke(this, new OnMaxHealthChangedEventArgs { maxHeart = currentMaxHeart });

        //Invoke Event
        OnHeartChangedEvent?.Invoke(this, new OnHealthChangedEventArgs { currentHeart = currentHeart });
    }

    public void UpdateCurrentHeart(int amount = 0)
    {
        if (amount < 0 && damageImmune == true)
            return;

        if (amount < 0)
        {
            TriggerDamageFeedBack();
            IFrameActive();
        }

        currentHeart += amount;
        if (currentHeart <= 0)
        {
            currentHeart = 0;
            DespawnPlayer();
        }
        else if (currentHeart > currentMaxHeart)
        {
            currentHeart = currentMaxHeart;
        }

        //Invoke Event
        OnHeartChangedEvent?.Invoke(this, new OnHealthChangedEventArgs { currentHeart = currentHeart });
    }

    public void UpdatePlayerHeartParameters()
    {
        currentMaxHeart = PlayerDataManager.Instance.PlayerDataRuntime.BaseMaxHealth;
        currentHeart = currentMaxHeart;

        UpdateCurrentMaxHeart();
        UpdateCurrentHeart();
    }

    //======================================================================
    // IFrame
    private void IFrameActive()
    {
        iFrameTimer = iFrameDuration;
        bodySpriteAnimator.SetBool("IFrame", true);

        damageImmune = true;
    }
}