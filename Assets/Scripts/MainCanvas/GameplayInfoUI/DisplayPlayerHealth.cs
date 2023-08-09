using UnityEngine;
using UnityEngine.UI;

public class DisplayPlayerHealth : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Canvas References:")]
    [SerializeField] private Transform fullHeartPool = default;
    [SerializeField] private Transform emptyHeartPool = default;

    //===========================================================================
    private void OnEnable()
    {
        playerHealth.OnMaxHealthChangedEvent += PlayerHealth_OnMaxHealthEventChanged;
        playerHealth.OnHealthChanged += PlayerHealth_OnHealthChanged;
    }

    private void OnDisable()
    {
        playerHealth.OnMaxHealthChangedEvent -= PlayerHealth_OnMaxHealthEventChanged;
        playerHealth.OnHealthChanged -= PlayerHealth_OnHealthChanged;
    }

    //===========================================================================
    private void PlayerHealth_OnMaxHealthEventChanged(object sender, PlayerHealth.OnMaxHealthChangedEventArgs e)
    {
        foreach (Transform fullHeart in emptyHeartPool)
        {
            fullHeart.gameObject.SetActive(false);
        }

        for (int i = 0; i < e.currentMaxHealth; i++)
        {
            if (emptyHeartPool.GetChild(i).gameObject.activeInHierarchy == false)
            {
                emptyHeartPool.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    private void PlayerHealth_OnHealthChanged(object sender, PlayerHealth.OnHealthChangedEventArgs e)
    {
        foreach (Transform fullHeart in fullHeartPool)
        {
            fullHeart.gameObject.SetActive(false);
        }

        for (int i = 0; i < e.currentHealth; i++)
        {
            if (fullHeartPool.GetChild(i).gameObject.activeInHierarchy == false)
            {
                fullHeartPool.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
