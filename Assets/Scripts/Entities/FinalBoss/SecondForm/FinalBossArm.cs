using UnityEngine;

public class FinalBossArm : MonoBehaviour
{
    [SerializeField] private Animator animator = default;

    [Header("List of Attack:")]
    [SerializeField] private FinalBossAttackAI slamAttack = default;
    public FinalBossAttackAI SlamAttack => slamAttack;

    //===========================================================================
    private void FixedUpdate()
    {
        if (SceneControlManager.Instance.CurrentGameplayState == GameplayState.Pause)
        {
            animator.speed = 0;
            return;
        }

        if (SceneControlManager.Instance.CurrentGameplayState != GameplayState.Pause &&
            animator.isActiveAndEnabled == false)
        {
            GetComponent<Animator>().enabled = true;
            animator.speed = 1;
        }
    }
}