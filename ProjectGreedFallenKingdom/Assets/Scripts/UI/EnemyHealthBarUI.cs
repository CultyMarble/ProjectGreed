using UnityEngine;

public class EnemyHealthBarUI : MonoBehaviour
{
    [SerializeField] private EnemyHealth health;
    [SerializeField] private Transform healtBar;

    //===========================================================================
    private void OnEnable()
    {
        health.OnHealthChanged += Health_OnHealthChangedHandler;
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= Health_OnHealthChangedHandler;
    }

    //===========================================================================
    private void Health_OnHealthChangedHandler(object sender, EnemyHealth.OnHealthChangedEvenArgs e)
    {
        transform.localScale = new Vector3(e.healthRatio, 1.0f, 1.0f);
    }
}