using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHeartPointUI : MonoBehaviour
{
    [SerializeField] private Transform pf_playerHeartPointTemplate;
    [SerializeField] private float intialOffset;
    [SerializeField] private float distanceOffset;

    private EnemyHealth player_HeartPoint;
    //===========================================================================
    private void Start()
    {
        player_HeartPoint = FindObjectOfType<Player>().GetComponent<EnemyHealth>();

        player_HeartPoint.OnHealthChanged += UI_PlayerHeartPoint_OnHealthChangedEventHandler;

        CreateHeartPointUI(player_HeartPoint.GetCurrentHeartPoint());
    }

    private void OnDestroy()
    {
        player_HeartPoint.OnHealthChanged -= UI_PlayerHeartPoint_OnHealthChangedEventHandler;
    }

    //===========================================================================
    private void UI_PlayerHeartPoint_OnHealthChangedEventHandler(object sender, EnemyHealth.OnHitPointChangedEvenArgs e)
    {
        CreateHeartPointUI(e.currentHealth);
    }

    private void CreateHeartPointUI(float currentHeartPoint)
    {
        for (int index = 0; index < currentHeartPoint; index++)
        {
            Transform iconTransform = Instantiate(pf_playerHeartPointTemplate, transform);

            iconTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-intialOffset + (distanceOffset * index), intialOffset);

            iconTransform.gameObject.SetActive(true);
        }
    }
}
