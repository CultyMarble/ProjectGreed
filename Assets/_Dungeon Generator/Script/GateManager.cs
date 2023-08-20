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
    public bool clearedRoom = false;
    public bool disableGate = false;

    [Header("Gate Referance")]
    [SerializeField] private GameObject roomVariants;
    [SerializeField] private GameObject gates;

    private void Awake()
    {
        roomController = GetComponentInParent<RoomController>();
        randomSpawnManager = GameObject.Find("RandomSpawnManager").GetComponent<RandomSpawnManager>();

    }

    private void OnEnable()
    {
        RoomManager.OnBossChange += RoomBossCheck;
    }

    private void OnDisable()
    {
        RoomManager.OnBossChange += RoomBossCheck;
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
        if (collision.gameObject.CompareTag("Player") && collision.GetType().ToString() != Tags.CAPSULECOLLIDER2D)
        {
            playerInsideRoom = true;

            if (clearedRoom || disableGate)
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
        if (collision.gameObject.CompareTag("Player") && collision.GetType().ToString() != Tags.CAPSULECOLLIDER2D)
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

    private void RoomBossCheck()
    {
        if (roomController.currentRoomType != RoomType.boss) return;

        //print("TEST");

    }
}
