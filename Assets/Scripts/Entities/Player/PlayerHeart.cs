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
    private int currentHeart = default;

    private readonly float feedbackDamageTime = 0.10f;
    private float feedbackDamageTimer = default;

    //======================================================================
    private void OnEnable()
    {
        EventManager.AfterSceneLoadEvent += EventManager_AfterSceneLoadEvent;
    }

    private void Start()
    {
        currentMaxHeart = PlayerDataManager.Instance.PlayerDataRuntime.BaseMaxHealth;
        currentHeart = currentMaxHeart;

        UpdateCurrentMaxHeart();
        UpdateCurrentHeart();
    }

    private void Update()
    {
        UpdateDamageFeedBackTimer();

        if (Input.GetKeyDown(KeyCode.Home))
        {
            UpdateCurrentMaxHeart(1);
        }

        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            UpdateCurrentMaxHeart(1);
            UpdateCurrentHeart(currentMaxHeart);

            PlayerDataManager.Instance.PlayerDataRuntime.SetBaseMaxHealth(currentMaxHeart);
            PlayerDataManager.Instance.SaveRunTimeDataToPlayerDataSlot();
        }
    }

    private void OnDisable()
    {
        EventManager.AfterSceneLoadEvent -= EventManager_AfterSceneLoadEvent;
    }

    //======================================================================
    private void EventManager_AfterSceneLoadEvent()
    {
        currentMaxHeart = PlayerDataManager.Instance.PlayerDataRuntime.BaseMaxHealth;
        currentHeart = currentMaxHeart;

        UpdateCurrentMaxHeart();
        UpdateCurrentHeart();
    }

    //======================================================================
    private void TriggerDamageFeedBack()
    {
        // Health Feedback
        GetComponentInChildren<SpriteRenderer>().color = new Color(255, 0, 0);
        feedbackDamageTimer = feedbackDamageTime;
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

    private void DespawnPlayer()
    {
        // Reset Parameters
        UpdateCurrentHeart(currentHeart);

        Player.Instance.gameObject.SetActive(false);

        // Call OnDestroy Event
        OnDespawnPlayerEvent?.Invoke(this, EventArgs.Empty);
    }

    //======================================================================
    public void ResetPlayerHealth(int baseMaxHealth)
    {
        currentMaxHeart = baseMaxHealth;
        currentHeart = baseMaxHealth;

        //Invoke Event
        OnMaxHeartChangedEvent?.Invoke(this, new OnMaxHealthChangedEventArgs { maxHeart = currentMaxHeart });

        //Invoke Event
        OnHeartChangedEvent?.Invoke(this, new OnHealthChangedEventArgs { currentHeart = currentHeart });
    }

    public void UpdateCurrentMaxHeart(int amount = 0)
    {
        if (amount > 0)
            TriggerDamageFeedBack();

        currentMaxHeart += amount;
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
        if (amount != 0)
        {
            feedbackDamageTimer = feedbackDamageTime;

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
        }

        //Invoke Event
        OnHeartChangedEvent?.Invoke(this, new OnHealthChangedEventArgs { currentHeart = currentHeart });
    }
}