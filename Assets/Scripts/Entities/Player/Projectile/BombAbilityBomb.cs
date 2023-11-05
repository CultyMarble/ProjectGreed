using UnityEngine;

public class BombAbilityBomb : MonoBehaviour
{
    private float damage;
    private float radius;
    private float delayTime;

    //===========================================================================
    private void Update()
    {
        if (SceneControlManager.Instance.CurrentGameplayState == GameplayState.Pause)
            return;

        UpdateDelayTime();

        if (delayTime <= 0)
            TriggerBombEffect();
    }

    //===========================================================================
    private void UpdateDelayTime()
    {
        if (delayTime <= 0)
            return;

        delayTime -= Time.deltaTime;
    }

    private void TriggerBombEffect()
    {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D collider2D in collider2DArray)
        {
            if (collider2D.CompareTag("Enemy"))
            {
                // Deal Damage
                collider2D.GetComponent<EnemyHealth>().UpdateCurrentHealth(-damage);
            }
        }

        gameObject.SetActive(false);
        gameObject.transform.position = Vector3.zero;
    }

    //===========================================================================
    public void SetDamage(float newDamage) { damage = newDamage; }

    public void SetRadius(float newRadius) { radius = newRadius; }

    public void SetDelayTime(float newDelay) { delayTime = newDelay; } 
}