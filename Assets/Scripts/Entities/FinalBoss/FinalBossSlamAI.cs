using UnityEngine;

public class FinalBossSlamAI : FinalBossAttackAI
{
    [SerializeField] private int triggerTime = default;
    private int triggerTimeCounter = default;

    [Header("Slam Behaviour")]
    [SerializeField] private Transform pfShockWave = default;
    [SerializeField] private Transform shockwaveParent = default;

    //======================================================================
    private void OnEnable()
    {
        triggerTimeCounter = triggerTime;
    }

    public override void TriggerAttack()
    {
        triggerTimeCounter = triggerTime;
        animator.SetBool("SlamAttack", true);
    }

    public void SetTriggerTime(int amount)
    {
        triggerTime = amount;
    }

    //======================================================================
    public void AnimEvent_SlamAttack()
    {
        // Get Direction with player
        Vector3 _direction = (Player.Instance.transform.position - transform.position).normalized;

        Transform _shockwave = Instantiate(pfShockWave, shockwaveParent);
        _shockwave.transform.position = transform.position;
        _shockwave.GetComponent<PrefabShockWave>().SetMoveDirection();

        triggerTimeCounter--;
        if (triggerTimeCounter == 0)
        {
            animator.SetBool("SlamAttack", false);
            GetComponent<FinalBossFirstForm>().SetCooldownTimer();
        }
    }
}