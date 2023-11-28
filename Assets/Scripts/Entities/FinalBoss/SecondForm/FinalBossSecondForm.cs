using UnityEngine;

public class FinalBossSecondForm : MonoBehaviour
{
    [SerializeField] private Animator animator = default;
    [SerializeField] private EnemyHealth health = default;

    [Header("Body part")]
    [SerializeField] private FinalBossSecondStageHead head = default;
    [SerializeField] private FinalBossArm ArmL = default;
    [SerializeField] private FinalBossArm ArmR = default;

    private bool isReady = default;
    public bool IsReady => isReady;

    // Combo
    private int comboIndex = default;
    private int comboMinStage1 = 0;
    private int comboMaxStage1 = 3;
    private int comboMinStage2 = 0;
    private int comboMaxStage2 = 2;

    // ComboTriggerTime
    private readonly int combo0TriggerTime = 5;
    private readonly int combo1TriggerTime = 3;
    private readonly int combo2TriggerTime = 1;

    private readonly int combo3TriggerTime = 1;
    private readonly int combo4TriggerTime = 4;
    private readonly int combo5TriggerTime = 3;

    private int triggerCounter = default;

    [Header("Put In Arm")]
    [SerializeField] private Transform zombieSpawnPoints = default;
    [SerializeField] private Transform parent = default;
    [SerializeField] private Transform pfPutInArm = default;

    //===========================================================================
    private void FixedUpdate()
    {
        if (SceneControlManager.Instance.CurrentGameplayState == GameplayState.Pause)
            return;

        if (SceneControlManager.Instance.CurrentGameplayState != GameplayState.Pause &&
            animator.isActiveAndEnabled == false)
        {
            GetComponent<Animator>().enabled = true;
        }

        if (health.GetHealthPercentage() <= 33)
        {
            if (head.gameObject.activeInHierarchy == false)
            {
                head.gameObject.SetActive(true);

                ArmL.GetComponent<ArmSlamAttackAI>().SetProjectileAmount(7);
                ArmR.GetComponent<ArmSlamAttackAI>().SetProjectileAmount(7);
            }
        }
        else if (health.GetHealthPercentage() < 66)
        {
            animator.SetTrigger("Stage2");

            ArmL.GetComponent<ArmSlamAttackAI>().SetProjectileAmount(5);
            ArmL.GetComponent<ArmSlamAttackAI>().SetTrapAmount(5);

            ArmR.GetComponent<ArmSlamAttackAI>().SetProjectileAmount(5);
            ArmR.GetComponent<ArmSlamAttackAI>().SetTrapAmount(5);
        }
    }

    //===========================================================================
    public void GetComboIndexStage1()
    {
        ArmSlamAttackAI _ArmL = ArmL.GetComponent<ArmSlamAttackAI>();
        ArmSlamAttackAI _ArmR = ArmR.GetComponent<ArmSlamAttackAI>();

        comboIndex  = Random.Range(comboMinStage1, comboMaxStage1);
        switch (comboIndex)
        {
            case 0:
                _ArmL.SetTriggerShockWave(true);
                _ArmR.SetTriggerShockWave(true);
                _ArmL.SetTriggerBulletWave(false);
                _ArmR.SetTriggerBulletWave(false);
                _ArmL.SetTriggerTrapWave(false);
                _ArmR.SetTriggerTrapWave(false);

                triggerCounter = combo0TriggerTime;
                animator.SetBool("Combo0", true);
                break;
                case 1:
                _ArmL.SetTriggerShockWave(false);
                _ArmR.SetTriggerShockWave(false);
                _ArmL.SetTriggerBulletWave(true);
                _ArmR.SetTriggerBulletWave(true);
                _ArmL.SetTriggerTrapWave(false);
                _ArmR.SetTriggerTrapWave(false);

                triggerCounter = combo1TriggerTime;
                animator.SetBool("Combo1", true);
                break;
                case 2:
                _ArmL.SetTriggerShockWave(false);
                _ArmR.SetTriggerShockWave(false);
                _ArmL.SetTriggerBulletWave(false);
                _ArmR.SetTriggerBulletWave(false);
                _ArmL.SetTriggerTrapWave(true);
                _ArmR.SetTriggerTrapWave(true);

                triggerCounter = combo2TriggerTime;
                animator.SetBool("Combo2", true);
                break;
            default:
                break;
        }
    }

    public void GetComboIndexStage2()
    {
        ArmSlamAttackAI _ArmL = ArmL.GetComponent<ArmSlamAttackAI>();
        ArmSlamAttackAI _ArmR = ArmR.GetComponent<ArmSlamAttackAI>();

        if (health.GetHealthPercentage() < 50)
            comboIndex = Random.Range(comboMinStage2, comboMaxStage2 + 1);
        else
            comboIndex = Random.Range(comboMinStage2, comboMaxStage2);

        switch (comboIndex)
        {
            case 0:
                triggerCounter = combo3TriggerTime;
                animator.SetBool("Combo3", true);
                break;
            case 1:
                triggerCounter = combo4TriggerTime;
                animator.SetBool("Combo4", true);
                break;
            case 2:
                triggerCounter = combo5TriggerTime;
                animator.SetBool("Combo5", true);
                break;
            default:
                break;
        }
    }

    public void ReduceTriggerTime()
    {
        triggerCounter--;
        if (triggerCounter == 0)
        {
            animator.SetBool("Combo0", false);
            animator.SetBool("Combo1", false);
            animator.SetBool("Combo2", false);
            animator.SetBool("Combo3", false);
            animator.SetBool("Combo4", false);
            animator.SetBool("Combo5", false);

            if (ArmL.gameObject.activeSelf == false)
                ArmL.gameObject.SetActive(true);

            if (ArmR.gameObject.activeSelf == false)
                ArmR.gameObject.SetActive(true);
        }
    }

    //===========================================================================
    public void TriggerSlamAttackLeftArm() { ArmL.SlamAttack.TriggerAttack(); }
    public void TriggerSlamAttackRightArm() { ArmR.SlamAttack.TriggerAttack(); }

    public void TriggerSlamShockWaveLeftArm()
    {
        ArmSlamAttackAI _ArmL = ArmL.GetComponent<ArmSlamAttackAI>();
        _ArmL.SetTriggerShockWave(true);
        _ArmL.SetTriggerBulletWave(false);
        _ArmL.SetTriggerTrapWave(false);

        ArmL.SlamAttack.TriggerAttack();
    }
    public void TriggerSlamShockWaveRightArm()
    {
        ArmSlamAttackAI _ArmR = ArmR.GetComponent<ArmSlamAttackAI>();
        _ArmR.SetTriggerShockWave(true);
        _ArmR.SetTriggerBulletWave(false);
        _ArmR.SetTriggerTrapWave(false);

        ArmR.SlamAttack.TriggerAttack();
    }

    public void TriggerSlamBulletWaveLeftArm()
    {
        ArmSlamAttackAI _ArmL = ArmL.GetComponent<ArmSlamAttackAI>();
        _ArmL.SetTriggerShockWave(false);
        _ArmL.SetTriggerBulletWave(true);
        _ArmL.SetTriggerTrapWave(false);

        ArmL.SlamAttack.TriggerAttack();
    }
    public void TriggerSlamBulletWaveRightArm()
    {
        ArmSlamAttackAI _ArmR = ArmR.GetComponent<ArmSlamAttackAI>();
        _ArmR.SetTriggerShockWave(false);
        _ArmR.SetTriggerBulletWave(true);
        _ArmR.SetTriggerTrapWave(false);

        ArmR.SlamAttack.TriggerAttack();
    }

    public void SpawnPutInArm()
    {
        int _index = Random.Range(0, zombieSpawnPoints.childCount);
        Instantiate(pfPutInArm, parent).transform.position = zombieSpawnPoints.GetChild(_index).transform.position;
    }
    public void DisableArms()
    {
        ArmL.gameObject.SetActive(false);
        ArmR.gameObject.SetActive(false);
    }
}