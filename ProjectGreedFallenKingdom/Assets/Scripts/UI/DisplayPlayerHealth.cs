using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPlayerHealth : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform pfPlayerHeart;
    [SerializeField] private Transform playerHealthUI;
    [SerializeField] private float distanceOffset;

    private EnemyHealth heartPoint;

    //===========================================================================
    private void OnEnable()
    {
        EventManager.AfterSceneLoadEvent += EventManager_AfterSceneLoadEventHandler;

        heartPoint = player.GetComponent<EnemyHealth>();
        heartPoint.OnHealthChanged += HeartPointBar_OnHealthChangedEventHandler;
    }

    private void OnDisable()
    {
        EventManager.AfterSceneLoadEvent += EventManager_AfterSceneLoadEventHandler;

        heartPoint.OnHealthChanged -= HeartPointBar_OnHealthChangedEventHandler;
    }

    //===========================================================================
    private void EventManager_AfterSceneLoadEventHandler()
    {
        UpdatePlayerHealthUI(heartPoint.GetCurrentHeartPoint());
    }

    private void HeartPointBar_OnHealthChangedEventHandler(object sender, EnemyHealth.OnHitPointChangedEvenArgs e)
    {
        UpdatePlayerHealthUI(e.currentHealth);
    }

    //===========================================================================
    private void UpdatePlayerHealthUI(float currentHealth)
    {
        // TODO POOLING FOR PERFORMANCE
        foreach (Transform child in playerHealthUI)
        {
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }

        for (int index = 0; index < currentHealth; ++index)
        {
            Transform iconTransform = Instantiate(pfPlayerHeart, playerHealthUI);

            iconTransform.GetComponent<Transform>().localPosition = new Vector2((distanceOffset * index), 0.0f);
            iconTransform.gameObject.SetActive(true);
        }
    }
}