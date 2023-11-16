using UnityEngine;

public class EnemyHealthBarUI : MonoBehaviour
{
    [SerializeField] private EnemyHealth health;
    [SerializeField] private Transform healtBar;

    //===========================================================================
    private void OnEnable()
    {
        health.OnHealthChanged += Health_OnHealthChanged1;
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= Health_OnHealthChanged1;
    }

    //===========================================================================
    private void Health_OnHealthChanged1(object sender, EnemyHealth.OnHealthChangedEvenArgs e)
    {
        healtBar.localScale = new Vector3(e.healthRatio, 1.0f, 1.0f);
    }
}