using System.Collections.Generic;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private List<Sprite> healthbarSprite;

    private EnemyHealth health;
    private SpriteRenderer healthBar;

    //===========================================================================
    private void Awake()
    {
        health = GetComponentInParent<EnemyHealth>();
        healthBar = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (!health || !healthBar)
            return;

        // Update current health bar sprite based on current health ratio
        int _spriteIndex = (int)(health.GetHealthRatio() * 10.0f) - 1;
        if (_spriteIndex < 0) _spriteIndex = 0;

        healthBar.sprite = healthbarSprite[_spriteIndex];
    }
}