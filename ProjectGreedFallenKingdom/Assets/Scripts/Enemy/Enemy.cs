using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public bool isKnockedBack;

    [SerializeField] private Stun stunStatusEffect;

    private float stunImmuneTime = 3.5f;
    private float stunImmuneTimer = default;
    private float knockedTimer;
    private float knockedCounter;

    //======================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collisions") && isKnockedBack && stunImmuneTimer <= 0.0f)
        {
            stunStatusEffect.Activate();

            isKnockedBack = false;

            stunImmuneTimer = stunImmuneTime;
        }
    }

    //======================================================================
    private void Update()
    {
        if (isKnockedBack)
        {
            knockedCounter += Time.deltaTime;
        }
        if(knockedCounter >= knockedTimer)
        {
            isKnockedBack = false;
        }
        UpdateStunImmuneTime();
    }

    //======================================================================
    private void UpdateStunImmuneTime()
    {
        if (stunImmuneTimer <= 0.0f)
            return;

        stunImmuneTimer -= Time.deltaTime;
    }
}