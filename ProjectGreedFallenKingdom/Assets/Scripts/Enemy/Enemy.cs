using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public bool isPushBack;

    [SerializeField] private Stun stunStatusEffect;

    private float stunImmuneTime = 3.5f;
    private float stunImmuneTimer = default;

    //======================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collisions") && isPushBack && stunImmuneTimer <= 0.0f)
        {
            stunStatusEffect.Activate();

            isPushBack = false;

            stunImmuneTimer = stunImmuneTime;
        }
    }

    //======================================================================
    private void Update()
    {
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