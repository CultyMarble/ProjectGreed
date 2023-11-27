using UnityEngine;

public enum EnemyType
{
    zombie,
    archer,
    bat,
    miniMushroom,
    boss,
}
public class Enemy : MonoBehaviour
{
    [HideInInspector] public bool isPushBack;
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyType enemyType;

    //[SerializeField] private Stun stunStatusEffect;

    private float stunImmuneTime = 1.5f;
    private float stunImmuneTimer = 0.0f;
    private float pushBackTime = 1.0f;

    private float idleAudioTime = 3f;
    private float idleAudioTimer = default;


    //======================================================================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Collisions") && isPushBack && stunImmuneTimer <= 0.0f)
        {
            GetComponentInChildren<Stun>().Activate();

            isPushBack = false;

            animator.SetBool("isStunned", true);

            stunImmuneTimer = stunImmuneTime;
        }
    }

    //======================================================================
    private void Update()
    {
        UpdateStunImmuneTime();
        if (isPushBack)
        {
            pushBackTime -= Time.deltaTime;
            if (pushBackTime <= 0)
            {
                isPushBack = false;
            }
        }
        if(idleAudioTimer <= 0)
        {
            idleAudioTimer = 0;
        }
        IdleAudioHandler();
    }

        //======================================================================
    private void UpdateStunImmuneTime()
    {
        if (stunImmuneTimer <= 0.0f)
        {
            //trying to fix issue where enemies get stunned while not actually being pushedback, delete if issues arise
            //isPushBack = false;
            return;
        }

    stunImmuneTimer -= Time.deltaTime;
    }

    public void InflictStatusEffect(AbilityStatusEffect statusEffect, int stackAmount)
    {
        switch (statusEffect)
        {
            case AbilityStatusEffect.Poison:
                GetComponentInChildren<Poison>().Activate(0.3f, 5f, stackAmount);
                break;
            case AbilityStatusEffect.Rot:
                GetComponentInChildren<Rot>().Activate(0.5f, 10.5f, 25);
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

    public void ResetStatusEffects()
    {
        if (gameObject.GetComponentInChildren<Poison>())
        {
            gameObject.GetComponentInChildren<Poison>().Deactivate();
        }

        if (gameObject.GetComponentInChildren<Rot>())
        {
            gameObject.GetComponentInChildren<Rot>().Deactivate();
        }
    }

    private void IdleAudioHandler()
    {
        if (idleAudioTimer > 0)
        {
            idleAudioTimer -= Time.deltaTime;
            return;
        }
        switch (enemyType)
        {
            case EnemyType.zombie:
                if(Random.Range(0,2) == 1)
                {
                    AudioManager.Instance.playSFXClip(AudioManager.SFXSound.zombieIdle1);
                }
                else {
                    AudioManager.Instance.playSFXClip(AudioManager.SFXSound.zombieIdle2);
                }
                break;
            case EnemyType.archer:
                break;
            case EnemyType.miniMushroom:
                break;
            case EnemyType.boss:
                break;
        }
        idleAudioTimer = idleAudioTime;
    }
}