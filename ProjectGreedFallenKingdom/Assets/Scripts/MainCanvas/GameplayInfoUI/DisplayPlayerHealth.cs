using UnityEngine;
using UnityEngine.UI;

public class DisplayPlayerHealth : MonoBehaviour
{
    [Header("Component References:")]
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Canvas References:")]
    [SerializeField] private Image playerHealthFrameImage;
    [SerializeField] private Image playerHealthBarImage;

    //===========================================================================
    private void OnEnable()
    {
        playerHealth.OnHealthChanged += PlayerHealth_OnHealthChangedHandler;
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChanged -= PlayerHealth_OnHealthChangedHandler;
    }

    //===========================================================================
    private void PlayerHealth_OnHealthChangedHandler(object sender, PlayerHealth.OnHealthChangedEvenArgs e)
    {
        playerHealthBarImage.rectTransform.localScale = new Vector3(e.currentHealth / e.maxHealth, 1.0f, 1.0f);
    }
}
