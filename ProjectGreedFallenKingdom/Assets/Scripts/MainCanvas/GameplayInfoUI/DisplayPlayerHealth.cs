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
        playerHealth.OnMaxHealthEventChanged += PlayerHealth_OnMaxHealthEventChanged;
        playerHealth.OnHealthChanged += PlayerHealth_OnHealthChanged;
    }

    private void OnDisable()
    {
        playerHealth.OnMaxHealthEventChanged -= PlayerHealth_OnMaxHealthEventChanged;
        playerHealth.OnHealthChanged -= PlayerHealth_OnHealthChanged;
    }

    //===========================================================================
    private void PlayerHealth_OnMaxHealthEventChanged(object sender, PlayerHealth.OnMaxHealthChangedEventArgs e)
    {
        playerHealthFrameImage.rectTransform.localScale = new Vector3(e.currentMaxHealth / 100.0f, 1.0f, 1.0f);
    }

    private void PlayerHealth_OnHealthChanged(object sender, PlayerHealth.OnHealthChangedEventArgs e)
    {
        playerHealthBarImage.rectTransform.localScale = new Vector3(e.currentHealth / 100.0f, 1.0f, 1.0f);
    }
}
