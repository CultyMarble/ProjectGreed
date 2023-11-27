using Mono.Cecil.Cil;
using UnityEngine;

public abstract class Trap : MonoBehaviour
{
    [SerializeField] protected Sprite[] trapSprites = default;
    [SerializeField] private float animationSpeed = default;
    [SerializeField] private int triggerIndex = default;

    [Header("Trap Settings:")]
    [SerializeField] private bool oneTimeTrigger = default;
    [SerializeField] private float reaimDuration = default;
    [SerializeField] private bool movementImpair = default;
    [SerializeField] private float movementImpairDuration = default;

    private SpriteRenderer trapSpriteRenderer = default;
    private bool playAnimation = default;
    private float effectAnimationTimer = default;
    protected int currentAnimationIndex = default;
    private float reaimTimer = default;

    private bool haveTriggered = default;
    protected bool playerInside = default;

    //===========================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player) && collision.GetType().ToString() == Tags.BOXCOLLIDER2D)
        {
            playerInside = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (oneTimeTrigger && haveTriggered)
            return;

        if (haveTriggered)
            return;

        if (collision.TryGetComponent(out Player player) && collision.GetType().ToString() == Tags.BOXCOLLIDER2D)
        {
            haveTriggered = true;
            playAnimation = true;
            currentAnimationIndex = 1;
            
            if (movementImpair && Player.Instance.actionState != PlayerActionState.IsDashing)
                Player.Instance.GetComponent<PlayerMovement>().SetImpairDuration(movementImpairDuration);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player) && collision.GetType().ToString() == Tags.BOXCOLLIDER2D)
        {
            playerInside = false;
        }
    }

    //===========================================================================
    private void Awake()
    {
        trapSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    //===========================================================================
    protected virtual void Update()
    {
        if (playAnimation)
        {
            TrapAnimation();
        }

        if (oneTimeTrigger)
            return;

        ReaimDurationCheck();
    }

    //===========================================================================
    private void TrapAnimation()
    {
        effectAnimationTimer += Time.deltaTime;
        if (effectAnimationTimer >= animationSpeed)
        {
            effectAnimationTimer -= animationSpeed;

            if (currentAnimationIndex == triggerIndex)
            {
                TriggerTrapEffect();
                AudioManager.Instance.playSFXClip(AudioManager.SFXSound.spikes);
            }
            if (currentAnimationIndex == trapSprites.Length)
            {
                playAnimation = false;
                reaimTimer = reaimDuration;
                return;
            }

            trapSpriteRenderer.sprite = trapSprites[currentAnimationIndex];
            currentAnimationIndex++;
        }
    }

    private void ReaimDurationCheck()
    {
        if (reaimTimer <= 0)
            return;

        reaimTimer -= Time.deltaTime;
        if (reaimTimer <= 0)
        {
            currentAnimationIndex = 0;
            trapSpriteRenderer.sprite = trapSprites[currentAnimationIndex];

            haveTriggered = false;
        }
    }

    //===========================================================================
    protected abstract void TriggerTrapEffect();
}