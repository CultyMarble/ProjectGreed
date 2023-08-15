using System;
using UnityEngine;

public class PlayerHeartManager : MonoBehaviour
{
    public struct OnHealthChangedEventArgs { public float currentHeart; }
    public event EventHandler<OnHealthChangedEventArgs> OnHeartChangedEvent;

    public struct OnMaxHealthChangedEventArgs { public float maxHeart; }
    public event EventHandler<OnMaxHealthChangedEventArgs> OnMaxHeartChangedEvent;

    public event EventHandler OnDespawnPlayerEvent;

    private int currentMaxHeart = default;
    private int currentHeart = default;

    //private readonly float feedbackDamageTime = 0.10f;
    //private float feedbackDamageTimer = default;

    //======================================================================
    private void Start()
    {
        currentMaxHeart = Player.Instance.PlayerData.baseMaxHealth;
        currentHeart = currentMaxHeart;

        UpdateCurrentMaxHeart();
        UpdateCurrentHeart();
    }

    private void OnEnable()
    {
        EventManager.AfterSceneLoadEvent += EventManager_AfterSceneLoadEvent;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.PageDown))
            UpdateCurrentHeart(-1);

        if (Input.GetKeyDown(KeyCode.PageUp))
            UpdateCurrentHeart(1);

    }

    private void OnDisable()
    {
        EventManager.AfterSceneLoadEvent -= EventManager_AfterSceneLoadEvent;
    }

    //======================================================================
    private void EventManager_AfterSceneLoadEvent()
    {
        //currentMaxHeart = Player.Instance.PlayerData.baseMaxHealth;
        //currentHeart = currentMaxHeart;

        //UpdateCurrentMaxHeart();
        //UpdateCurrentHeart();
    }

    //======================================================================
    //private void DamageFeedBack()
    //{
    //    // Health Feedback
    //    this.gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 0, 0);
    //    feedbackDamageTimer = feedbackDamageTime;
    //}

    //private void UpdateDamageFeedBackTimer()
    //{
    //    if (feedbackDamageTimer > 0)
    //    {
    //        feedbackDamageTimer -= Time.deltaTime;
    //        if (feedbackDamageTimer <= 0)
    //        {
    //            feedbackDamageTimer = 0;
    //            this.gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 255, 255);
    //        }
    //    }
    //}

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
            currentHeart += amount;
            if (currentHeart <= 0)
            {
                currentHeart = 0;
            }
            else if (currentHeart > currentMaxHeart)
            {
                currentHeart = currentMaxHeart;
            }

            if (currentHeart <= 0)
                DespawnPlayer();
        }

        //Invoke Event
        OnHeartChangedEvent?.Invoke(this, new OnHealthChangedEventArgs { currentHeart = currentHeart });
    }
}