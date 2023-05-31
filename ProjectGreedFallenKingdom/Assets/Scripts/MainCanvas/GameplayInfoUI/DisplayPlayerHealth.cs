using UnityEngine;
using TMPro;

public class DisplayPlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private TextMeshProUGUI playerHealthText;

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
        playerHealthText.SetText("Player Health: " + e.currentHealth);
    }
}
