using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] private int spawnAmount;
    [SerializeField] private Transform[] enemyTypePoolList;

    private int poolIndex = default;

    private Transform enemySpawnPointList;
    private List<int> spawnPointIndexList = default;
    private int spawnPointIndex = default;

    //===========================================================================
    private void OnEnable()
    {
        EventManager.AfterSceneLoadEvent += EventManager_AfterSceneLoadEventHandler;
        EventManager.BeforeSceneUnloadEvent += EventManager_BeforeSceneUnloadEventHandler;
    }

    private void OnDisable()
    {
        EventManager.AfterSceneLoadEvent -= EventManager_AfterSceneLoadEventHandler;
        EventManager.BeforeSceneUnloadEvent -= EventManager_BeforeSceneUnloadEventHandler;
    }

    //===========================================================================
    private void Awake()
    {
        spawnPointIndexList = new();
    }

    //===========================================================================
    private void EventManager_AfterSceneLoadEventHandler()
    {
        if (LoadEnemySpawnPointList())
        {
            SpawnEnemy();
        }
    }

    private void EventManager_BeforeSceneUnloadEventHandler()
    {
        DespawnEnemies();
    }

    //===========================================================================
    private bool LoadEnemySpawnPointList()
    {
        if (GameObject.Find("EnemySpawnPointList")!= null){
            enemySpawnPointList = GameObject.Find("EnemySpawnPointList").transform;
            return true;
        }
        else { return false; }
    }

    private void SpawnEnemy()
    {
        spawnPointIndexList.Clear();

        // Get Random Enemy Spawn Points
        for (int i = 0; i < spawnAmount; i++)
        {
            spawnPointIndex = Random.Range(0, enemySpawnPointList.childCount);

            if (spawnPointIndexList.Count == 0)
            {
                spawnPointIndexList.Add(spawnPointIndex);
            }
            else
            {
                bool _addIndex = true;
                if (spawnPointIndexList.Count == enemySpawnPointList.childCount)
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

        // Spawn Enemy
        foreach (int index in spawnPointIndexList)
        {
            // Random Enemy Type Pool
            poolIndex = Random.Range(0, enemyTypePoolList.Length);

            // Spawn Enemy
            foreach(Transform enemy in enemyTypePoolList[poolIndex])
            {
                if (enemy.gameObject.activeSelf == false)
                {
                    enemy.transform.position = enemySpawnPointList.GetChild(index).gameObject.transform.position;
                    enemy.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    private void DespawnEnemies()
    {
        for (int i = 0; i < enemyTypePoolList.Length; i++)
        {
            foreach (Transform enemy in enemyTypePoolList[i])
            {
                if (enemy.gameObject.activeSelf == true)
                {
                    enemy.gameObject.SetActive(false);
                    break;
                }
            }
        }
    }
}
