using UnityEngine;



public class PlayerAnimator2 : SingletonMonobehaviour<PlayerAnimator>
{
    [Header("Component Reference:")]
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private Animator animator;

    [Header("Sprite List")]
    [SerializeField] private Sprite[] playerUpWalk = default;
    [SerializeField] private Sprite[] playerDownWalk = default;
    [SerializeField] private Sprite[] playerSideWalk = default;

    private Vector3 toMouseDirectionVector = default;
    private float zEulerAngle = default;

    private readonly float effectAnimationSpeed = 0.1f;
    private float effectAnimationTimer = default;
    private int currentAnimationIndex = default;

    private bool playingAnimation = default;
    //===========================================================================

    private void FixedUpdate()
    {
        toMouseDirectionVector = (CultyMarbleHelper.GetMouseToWorldPosition() - transform.position).normalized;
        zEulerAngle = CultyMarbleHelper.GetAngleFromVector(toMouseDirectionVector);

        PlayerSpriteFlipHandler();
        PlayerAnimationStateHandler();

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            playingAnimation = true;
        else
            playingAnimation = false;
    }

    //===========================================================================
    private void PlayerAnimationStateHandler()
    {
        if (zEulerAngle > 45 && zEulerAngle <= 135)
        {
            if (playingAnimation)
            {
                PlayAnimation(playerUpWalk);
                animator.SetBool("isFacingUp", true);
                animator.SetBool("isFacingDown", false);

            }
            else
            {
                playerSpriteRenderer.sprite = playerUpWalk[0];
            }
        }
        else if (zEulerAngle > -135 && zEulerAngle <= -45)
        {
            if (playingAnimation)
            {
                PlayAnimation(playerDownWalk);
                animator.SetBool("isFacingDown", true);
                animator.SetBool("isFacingUp", false);

            }
            else
            {
                playerSpriteRenderer.sprite = playerDownWalk[0];
            }
        }
        else
        {
            if (playingAnimation)
            {
                PlayAnimation(playerSideWalk);
            }
            else
            {
                playerSpriteRenderer.sprite = playerSideWalk[0];
            }
        }
    }

    private void PlayAnimation(Sprite[] sprites)
    {
        effectAnimationTimer += Time.deltaTime;
        if (effectAnimationTimer >= effectAnimationSpeed)
        {
            effectAnimationTimer -= effectAnimationSpeed;

            playerSpriteRenderer.sprite = sprites[currentAnimationIndex];

            currentAnimationIndex++;

            if (currentAnimationIndex == sprites.Length)
                currentAnimationIndex = 0;
        }
    }

    private void PlayerSpriteFlipHandler()
    {
        if (zEulerAngle < -135 || zEulerAngle >= 135)
            playerSpriteRenderer.flipX = true;
        else
            playerSpriteRenderer.flipX = false;
    }
}
