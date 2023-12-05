using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnManager : SingletonMonobehaviour<RandomSpawnManager>
{
    //[SerializeField] private int spawnAmount;
    private int spawnAmount;

    [SerializeField] private Transform[] enemyTypePoolList;
    [SerializeField] private GameObject[] objectList;
    [SerializeField] private GameObject[] noteList;
    [SerializeField] private GameObject[] chestList;

    [SerializeField] private float objectSpawnChance;
    [SerializeField] private float noteSpawnChance;
    [SerializeField] private float chestSpawnChance;
    [SerializeField] private int spawnNum;
    private int totalSpawned = 0;
    private GameObject spawnPointList;


    private int poolIndex = default;

    private List<int> spawnPointIndexList = default;
    private int spawnPointIndex = default;

    //===========================================================================
    private void OnEnable()
    {
        //EventManager.AfterSceneLoadEvent += EventManager_AfterSceneLoadEventHandler;
        EventManager.BeforeSceneUnloadEvent += EventManager_BeforeSceneUnloadEventHandler;
        spawnPointIndexList = new();
    }

    private void OnDisable()
    {
        //EventManager.AfterSceneLoadEvent -= EventManager_AfterSceneLoadEventHandler;
        EventManager.BeforeSceneUnloadEvent -= EventManager_BeforeSceneUnloadEventHandler;
    }

    //===========================================================================
    //===========================================================================
    public void SpawnRandom(GameObject spawnPoints, Difficulty difficulty)
    {
        if (spawnPoints != null)
        {
            spawnPointList = spawnPoints;
            if (spawnNum == 0) { return; }
        }
        Spawn(difficulty);
    }

    private void EventManager_BeforeSceneUnloadEventHandler()
    {
        DespawnEnemies();
    }

    //===========================================================================

    private void Spawn(Difficulty difficulty)
    {
        bool spawnItems = true;
        switch (difficulty)
        {
            case Difficulty.easy:
                spawnAmount = spawnNum;
                break;
            case Difficulty.medium:
                spawnAmount = (int)Mathf.Clamp(spawnNum * 1.5f, 0f, 2f * spawnPointList.transform.childCount);
                break;
            case Difficulty.hard:
                spawnAmount = (int)Mathf.Clamp(spawnNum * 2f, 0f, 2f * spawnPointList.transform.childCount);
                break;
        }
        bool noteSpawned = false;
        // Random Spawn
        for (int index = 0; index < spawnPointList.transform.childCount; index++)
        {
            int offSetX = Random.Range(-1, 1);
            int offSetY = Random.Range(-1, 1);
            Vector3 offset = new Vector3(offSetX, offSetY, 0);

            if (index == spawnPointList.transform.childCount - 1 && totalSpawned < spawnAmount)
            {
                index = 0;
                spawnItems = false;
            }
            if (spawnItems)
            {
                //Spawn Chest
                if (Random.value < chestSpawnChance)
                {
                    Instantiate(chestList[Random.Range(0, chestList.Length)], spawnPointList.transform.GetChild(index).transform.position, Quaternion.identity);
                    continue;
                }
                // Spawn Item
                else if (Random.value < objectSpawnChance)
                {
                    Instantiate(objectList[Random.Range(0, objectList.Length)], spawnPointList.transform.GetChild(index).transform.position, Quaternion.identity);
                    continue;
                }
                // Spawn Note
                if (Random.value < noteSpawnChance && !noteSpawned)
                {
                    Instantiate(noteList[Random.Range(0, noteList.Length)], spawnPointList.transform.GetChild(index).transform.position, Quaternion.identity);
                    noteSpawned = true;
                }
            }

            // Random Enemy Type Pool
            poolIndex = Random.Range(0, enemyTypePoolList.Length);

            // Spawn Enemy
            foreach (Transform enemy in enemyTypePoolList[poolIndex])
            {
                if (totalSpawned >= spawnAmount)
                {
                    break;
                }
                if (enemy.gameObject.activeSelf == false)
                {
                    enemy.transform.position = spawnPointList.transform.GetChild(index).transform.position + offset;
                    enemy.gameObject.SetActive(true);
                    totalSpawned++;
                    if (enemy.CompareTag("EnemyGroup"))
                    {
                        switch (difficulty)
                        {
                            case Difficulty.easy:
                                break;
                            case Difficulty.medium:
                                foreach (EnemyHealth child in enemy.transform.GetComponentsInChildren<EnemyHealth>())
                                {
                                    child.currentMaxHealth = 1.25f * child.baseMaxHealth;
                                    child.currentHealth = child.currentMaxHealth;
                                }
                                break;
                            case Difficulty.hard:
                                foreach (EnemyHealth child in enemy.transform.GetComponentsInChildren<EnemyHealth>())
                                {
                                    child.currentMaxHealth = 1.5f * child.baseMaxHealth;
                                    child.currentHealth = child.currentMaxHealth;
                                }
                                break;
                        }
                        break;
                    }
                    else
                    {
                        switch (difficulty)
                        {
                            case Difficulty.easy:
                                break;
                            case Difficulty.medium:
                                enemy.GetComponent<EnemyHealth>().currentMaxHealth = 1.25f * enemy.GetComponent<EnemyHealth>().baseMaxHealth;
                                enemy.GetComponent<EnemyHealth>().currentHealth = enemy.GetComponent<EnemyHealth>().currentMaxHealth;
                                break;
                            case Difficulty.hard:
                                enemy.GetComponent<EnemyHealth>().currentMaxHealth = 1.5f * enemy.GetComponent<EnemyHealth>().baseMaxHealth;
                                enemy.GetComponent<EnemyHealth>().currentHealth = enemy.GetComponent<EnemyHealth>().currentMaxHealth;
                                break;
                        }
                        break;
                    }
                }
            }

        }
        totalSpawned = 0;
    }
    private void DespawnEnemies()
    {
        //int survivingEnemies = 0;
        for (int i = 0; i < enemyTypePoolList.Length; i++)
        {
            foreach (Transform enemy in enemyTypePoolList[i])
            {
                if (enemy.gameObject.activeSelf == true)
                {
                    if (enemy.CompareTag("EnemyGroup")) 
                    { 
                        enemy.gameObject.SetActive(false); 
                    }
                    else
                    {
                        enemy.GetComponent<Enemy>().ResetStatusEffects();
                        enemy.GetComponent<EnemyHealth>().Despawn();
                    }
                }
            }
        }
        
    }
}

