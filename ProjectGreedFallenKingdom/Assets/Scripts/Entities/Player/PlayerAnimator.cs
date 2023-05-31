using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    // Components
    private SpriteRenderer playerSpriteRenderer;

    //===========================================================================
    private void Awake()
    {
        playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        PlayerSpriteFlipHandler();
    }

    //===========================================================================
    private void PlayerSpriteFlipHandler()
    {
        if (transform.position.x > CultyMarbleHelper.GetMouseToWorldPosition().x)
        {
            playerSpriteRenderer.flipX = true;
        }
        else
        {
            playerSpriteRenderer.flipX = false;
        }
    }
}
