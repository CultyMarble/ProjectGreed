using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class TriggerDemoOverWhenKilled : MonoBehaviour
{
    [SerializeField] private EnemyHealth health;

    //===========================================================================
    private void OnEnable()
    {
        health.OnDespawnEvent += Health_OnDespawnEvent;
    }

    private void OnDisable()
    {
        health.OnDespawnEvent -= Health_OnDespawnEvent;
    }

    //===========================================================================
    private void Health_OnDespawnEvent(object sender, System.EventArgs e)
    {
        DemoOverMenuGUI.Instance.SetContentActive(true);
    }
}
