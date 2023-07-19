using System.Collections.Generic;
using UnityEngine;

public class FloorTrap : MonoBehaviour
{
    [SerializeField] private SpriteRenderer trapSR = default(SpriteRenderer);
    [SerializeField] private List<Sprite> trapSprites = new();
    [SerializeField] private float effectAnimationSpeed = default(float);
    [SerializeField] private int damageIndex = default(int);

    [Header("Trap Settings:")]
    [SerializeField] private float trapDamage = default(float);

    private bool triggered = default(bool);
    private float effectAnimationTimer = default(float);
    private int currentAnimationIndex = default(int);

    private int useTime = 1;

    //===========================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (useTime == 0)
            return;

        if (collision.TryGetComponent(out Player player))
        {
            triggered = true;
            useTime = 0;
        }
    }

    //===========================================================================
    private void Update()
    {
        if (triggered)
        {
            TrapEffectAnimation();
        }
    }

    //===========================================================================

    private void TrapEffectAnimation()
    {
        effectAnimationTimer += Time.deltaTime;
        if (effectAnimationTimer >= effectAnimationSpeed)
        {
            effectAnimationTimer -= effectAnimationSpeed;

            if (currentAnimationIndex == damageIndex)
            {
                Player.Instance.GetComponent<PlayerHealth>().UpdateCurrentHealth(-trapDamage);
            }

            if (currentAnimationIndex == trapSprites.Count)
            {
                triggered = false;
                return;
            }

            trapSR.sprite = trapSprites[currentAnimationIndex];

            currentAnimationIndex++;
        }
    }
}
