using UnityEngine;

public abstract class EnemyStatusEffectNew : MonoBehaviour
{
    protected EnemyHealth enemyHealth = default;
    protected ChasingAI chasingAI = default;

    //======================================================================
    protected virtual void Awake()
    {
        enemyHealth = GetComponentInParent<EnemyHealth>();
        chasingAI = GetComponentInParent<ChasingAI>();
    }

    protected virtual void OnEnable()
    {
        enemyHealth.OnDespawnEvent += EnemyHealth_OnEnemyDespawn;
    }

    protected virtual void OnDisable()
    {
        enemyHealth.OnDespawnEvent -= EnemyHealth_OnEnemyDespawn;
    }

    //======================================================================
    private void EnemyHealth_OnEnemyDespawn(object sender, System.EventArgs e)
    {
        this.enabled = false;
    }
}