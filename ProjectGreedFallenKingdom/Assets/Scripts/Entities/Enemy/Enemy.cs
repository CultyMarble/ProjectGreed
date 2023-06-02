using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public bool isPushBack;

    //[SerializeField] private Stun stunStatusEffect;

    private float stunImmuneTime = 3.5f;
    private float stunImmuneTimer = default;

    //======================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collisions") && isPushBack && stunImmuneTimer <= 0.0f)
        {
            GetComponent<Stun>().Activate();

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
        {
            //trying to fix issue where enemies get stunned while not actually being pushedback, delete if issues arise
            isPushBack = false;
            return;
        }

        stunImmuneTimer -= Time.deltaTime;
    }
    public void InflictStatusEffect(AbilityStatusEffect statusEffect)
    {
        switch (statusEffect)
        {
            case AbilityStatusEffect.Poison:
                GetComponentInChildren<Poison>().Activate(0.3f, 5f);
                break;
            case AbilityStatusEffect.Rot:
                GetComponentInChildren<Rot>().Activate();
                break;
            case AbilityStatusEffect.none:
                break;
        }
    }
    public AbilityStatusEffect CheckStatusEffect()
    {
        if (gameObject.GetComponentInChildren<Poison>().CheckActive())
        {
            return AbilityStatusEffect.Poison;
        }
        if (gameObject.GetComponentInChildren<Rot>().CheckActive())
        {
            return AbilityStatusEffect.Rot;
        }
        else
        {
            return AbilityStatusEffect.none;
        }
    }
}