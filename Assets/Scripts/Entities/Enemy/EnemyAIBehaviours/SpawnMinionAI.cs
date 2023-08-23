using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TargetingAI))]
public class SpawnMinionAI : MonoBehaviour
{
    [SerializeField] private Transform pfEnemyMinion;
    [SerializeField] private Transform minions;
    [SerializeField] private int spawnAmount;
    [SerializeField] private float minSpawnRadius = 0.0f;
    [SerializeField] private float maxSpawnRadius = 5.0f;

    [SerializeField] private bool noEnemiesNoPatrol;
    [SerializeField] private bool spawnWhenTargetIsValid;
    [SerializeField] private bool spawnAtInterval;
    [SerializeField] private float spawnIntervalTime;
    [SerializeField] private bool spawnAtCertainHPThreshold;
    [SerializeField] private List<float> hpThresholdRatio;
    [SerializeField] private bool spawnWhenAllMinionsAreKilled;

    private Transform minionPool;

    private TargetingAI targetingAI;
    private EnemyHealth health;
    private float spawnIntervalTimeCounter;
    private int spawnAmountCounter;

    private bool canSpawn = false;
    private bool noMinions = true;
    private bool intervalDone = true;

    //===========================================================================
    private void Awake()
    {
        targetingAI = GetComponent<TargetingAI>();
        health = GetComponent<EnemyHealth>();
    }

    private void Start()
    {
        spawnIntervalTimeCounter = spawnIntervalTime;
        minionPool = GameObject.Find("Zombies").transform;
    }

    private void Update()
    {
        if (spawnWhenTargetIsValid == true && targetingAI.CheckNoTarget())
            return;

        if(noEnemiesNoPatrol && CheckNoMinions())
        {
            targetingAI.TogglePatrol(false);
        }

        if (spawnWhenAllMinionsAreKilled)
            CheckNoMinions(); 

        if (spawnAtInterval)
            IntervalTimeCounter();

        if(intervalDone && noMinions)
        {
            canSpawn = true;
        }
        else
        {
            canSpawn = false;
        }

        if (canSpawn)
        {
            SpawnMinion(spawnAmount);
            targetingAI.TogglePatrol(true);
        }
    }

    //===========================================================================
    private void IntervalTimeCounter()
    {
        if (spawnWhenAllMinionsAreKilled && !noMinions)
        {
            return;
        }
        spawnIntervalTimeCounter -= Time.deltaTime;
        if (spawnIntervalTimeCounter <= 0.0f)
        {
            spawnIntervalTimeCounter += spawnIntervalTime;
            intervalDone = true;
        }
        else
        {
            intervalDone = false;
        }

    }

    private void SpawnMinion(int spawnAmount)
    {
        for(int num = 0; num < spawnAmount; num++) { 
            foreach (Transform minion in minionPool)
            {
                if (minion.gameObject.activeInHierarchy == false)
                {
                    Vector3 newPosition = transform.position + CultyMarbleHelper.GetRandomDirection() * UnityEngine.Random.Range(minSpawnRadius, maxSpawnRadius);
                    minion.gameObject.SetActive(true);
                    Collider2D collider = Physics2D.OverlapCircle(newPosition, 0.1f);
                    if (collider != null)
                    {
                        while (collider.CompareTag("Collisions"))
                        {
                            newPosition = transform.position + CultyMarbleHelper.GetRandomDirection() * UnityEngine.Random.Range(minSpawnRadius, maxSpawnRadius);
                            collider = Physics2D.OverlapCircle(newPosition, 0.1f);
                        }
                    }
                    minion.position = transform.position + CultyMarbleHelper.GetRandomDirection() * UnityEngine.Random.Range(minSpawnRadius, maxSpawnRadius);
                    break;
                }
            }
        }
    }
    private bool CheckNoMinions()
    {
        int minionCount = 0;
        foreach (Transform minion in minionPool)
        {
            if(minion.gameObject.activeSelf == true)
            {
                minionCount++;
            }
        }
        if (minionCount > 0)
        {
            noMinions = false;
            return false;
        }
        else
        {
            noMinions = true;
            return true;
        }
    }
}
