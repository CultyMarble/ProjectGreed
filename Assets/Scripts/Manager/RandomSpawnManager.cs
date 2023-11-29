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
    public void SpawnRandom(GameObject spawnPoints,Difficulty difficulty)
    {
        if (spawnPoints != null)
        {
            spawnPointList = spawnPoints;
            if (spawnNum == 0) { return; }
            spawnAmount = spawnNum;
        }
        Spawn(difficulty);
    }

    private void EventManager_BeforeSceneUnloadEventHandler()
    {
        DespawnEnemies();
    }

    //===========================================================================
    //private bool LoadEnemySpawnPointList()
    //{
    //    if (GameObject.Find("EnemySpawnPointList")!= null){
    //        enemySpawnPointList = GameObject.Find("EnemySpawnPointList");
    //        int spawnNum = enemySpawnPointList.GetComponent<SpawnPointList>().GetSpawnAmount();
    //        if (spawnNum == 0) { return false; }
    //        spawnAmount = spawnNum;
    //        return true;
    //    }
    //    else { return false; }
    //}

    private void Spawn(Difficulty difficulty)
    {
        spawnPointIndexList.Clear();

        // Get Random Spawn Points
        for (int i = 0; i < spawnAmount; i++)
        {
            spawnPointIndex = Random.Range(0, spawnPointList.transform.childCount);

            if (spawnPointIndexList.Count == 0)
            {
                spawnPointIndexList.Add(spawnPointIndex);
            }
            else
            {
                bool _addIndex = true;
                if (spawnPointIndexList.Count == spawnPointList.transform.childCount)
                {
                    break;
                }

                // Check for unique Spawn Point Index
                for (int index = 0; index < spawnPointIndexList.Count; index++)
                {
                    if (spawnPointIndex == spawnPointIndexList[index])
                    {
                        _addIndex = false;
                    }
                }

                if (_addIndex)
                {
                    spawnPointIndexList.Add(spawnPointIndex);
                }
                else
                {
                    i--;
                }
            }
        }
        switch (difficulty)
        {
            case Difficulty.easy:
                spawnAmount = spawnNum;
                break;
            case Difficulty.medium:
                spawnAmount = (int)(spawnNum * 1.25);
                break;
            case Difficulty.hard:
                spawnAmount = (int)(spawnNum * 1.5);
                break;
        }
        // Random Spawn
        foreach (int index in spawnPointIndexList)
        {
            //Spawn Chest
            if(Random.value < chestSpawnChance)
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
            // Random Enemy Type Pool
            poolIndex = Random.Range(0, enemyTypePoolList.Length);

            // Spawn Enemy
            foreach(Transform enemy in enemyTypePoolList[poolIndex])
            {
                if(totalSpawned >= spawnNum)
                {
                    break;
                }
                if (enemy.gameObject.activeSelf == false)
                {
                    enemy.transform.position = spawnPointList.transform.GetChild(index).transform.position;
                    enemy.gameObject.SetActive(true);
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
                    }
                    totalSpawned++;
                    break;
                }
            }
            // Spawn Note
            if (Random.value < noteSpawnChance)
            {
                Instantiate(noteList[Random.Range(0, noteList.Length)], spawnPointList.transform.GetChild(index).transform.position, Quaternion.identity);
                continue;
            }
            totalSpawned = 0;
        }
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
