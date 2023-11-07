using UnityEngine;

public abstract class FinalBossAttackAI : MonoBehaviour
{
    [SerializeField] protected Animator animator = default;

    //===========================================================================
    public abstract void TriggerAttack();
}