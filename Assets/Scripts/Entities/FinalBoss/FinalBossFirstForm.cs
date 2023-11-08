using UnityEngine;

public class FinalBossFirstForm : MonoBehaviour
{
    [SerializeField] private Animator animator = default;
    [Header("Attack Cooldown")]
    [SerializeField] private float coolDownMax = default;
    [SerializeField] private float coolDownMin = default;
    private float coolDownTimer = default;

    [Header("List of Attacks:")]
    [SerializeField] private FinalBossAttackAI[] attackList = default;

    private bool isReady = default;
    public bool IsReady => isReady;

    //===========================================================================
    private void Start()
    {
        coolDownTimer = coolDownMax;
    }

    protected virtual void Update()
    {
        if (SceneControlManager.Instance.CurrentGameplayState == GameplayState.Pause)
            return;

        UpdateCooldownTimer();

        if (isReady == false)
            return;

        int _index = Random.Range(0, attackList.Length);
        attackList[_index].TriggerAttack();

        isReady = false;
    }

    private void FixedUpdate()
    {
        if (SceneControlManager.Instance.CurrentGameplayState == GameplayState.Pause)
            return;

        if (SceneControlManager.Instance.CurrentGameplayState != GameplayState.Pause &&
            animator.isActiveAndEnabled == false)
        { 
            GetComponent<Animator>().enabled = true;
        }
    }

    //===========================================================================
    public void SetCooldownTimer()
    {
        coolDownTimer = Random.Range(coolDownMin, coolDownMax);
    }

    private void UpdateCooldownTimer()
    {
        if (coolDownTimer <= 0.0f)
            return;

        coolDownTimer -= Time.deltaTime;
        if (coolDownTimer <= 0.0f)
            isReady = true;
    }
}