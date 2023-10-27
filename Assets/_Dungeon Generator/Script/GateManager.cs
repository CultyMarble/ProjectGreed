using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour
{
    private RoomController roomController;

    [Header("Enemy Spawn")]
    private RandomSpawnManager randomSpawnManager;
    private GameObject randomSpawnPoints;
    private GameObject[] enemyPool;

    [Header("Gate Data")]
    public bool playerInsideRoom = false;
    public bool playerInLockZone = false;
    public bool clearedRoom = false;
    public bool disableGate = false;
    public bool locked = false;

    [Header("Gate Referance")]
    [SerializeField] private GameObject roomVariants;
    [SerializeField] private GameObject gates;

    private void Awake()
    {
        roomController = GetComponentInParent<RoomController>();
        if (!locked)
        {
            randomSpawnManager = GameObject.Find("RandomSpawnManager").GetComponent<RandomSpawnManager>();
        }
    }
    private void Update()
    {
        if (disableGate)
        {
            ActiveGates(false);
            return;
        }
        else if (locked)
        {
            ActiveGates(true);
        }
        else
        {
            ActiveGates(false);
        }
    }
    private void ActiveGates(bool active)
    {
         gates.SetActive(active);
    }

    public void RoomCleared()
    {
        clearedRoom = true;
        ActiveGates(false);
    }

    private void SpawnWithinTrigger()
    {
        randomSpawnManager.GetComponent<RandomSpawnManager>().SpawnRandom(randomSpawnPoints);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.GetType().ToString() != Tags.CAPSULECOLLIDER2D && !playerInsideRoom)
        {
            playerInsideRoom = true;

            if (clearedRoom || disableGate || locked)
            {
                return;
            }
            for (int i = 0; i < roomVariants.transform.childCount; i++)
            {
                GameObject variant = roomVariants.transform.GetChild(i).gameObject;
                if (!variant.activeSelf)
                {
                    continue;
                }
                if (variant.transform.Find("SpawnPointList") != null)
                {
                    randomSpawnPoints = variant.transform.Find("SpawnPointList").gameObject;
                }
                else
                {
                    return;
                }
            }
            ActiveGates(true);
            SpawnWithinTrigger();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.GetType().ToString() != Tags.CAPSULECOLLIDER2D && clearedRoom)
        {
            playerInsideRoom = false;
        }
    }

    // ---------------------------------------------------------------------------

    private void FixedUpdate()
    {
        if (!playerInsideRoom) return;

        RoomEnemyCheckDelay();
    }
    private void RoomEnemyCheckDelay()
    {
        enemyPool = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemyPool.Length == 0)
        {
            RoomCleared();
        }
    }
}
