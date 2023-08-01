using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour
{
    private RoomController roomController;

    [Header("Enemy Spawn")]
    [SerializeField] private EnemySpawnScriptable enemySpawnScriptable;
    [SerializeField] private List<GameObject> currentEnemies;
    private BoxCollider2D spawnTrigger;

    [Header("Gate Data")]
    public bool playerInsideRoom = false;
    public bool clearedRoom = false;
    public bool disableGate = false;

    [Header("Gate Referance")]
    [SerializeField] private GameObject[] gates;

    private void Awake()
    {
        roomController = GetComponentInParent<RoomController>();
        spawnTrigger = GetComponentInChildren<BoxCollider2D>();
    }

    private void OnEnable()
    {
        RoomManager.OnBossChange += RoomBossCheck;
        EnemyHealth.OnDespawnCallAll += RoomEnemyCheck;
    }

    private void OnDisable()
    {
        RoomManager.OnBossChange += RoomBossCheck;
        EnemyHealth.OnDespawnCallAll -= RoomEnemyCheck;
    }

    private void ActiveGates(bool active)
    {
        foreach (var gate in gates)
        {
            gate.SetActive(active);
        }
    }

    public void RoomCleared()
    {
        clearedRoom = true;
        ActiveGates(false);
    }

    private void SpawnWithinTrigger(BoxCollider2D trigger)
    {
        Vector2 randomPoint = GetRandomPointInTrigger(trigger);
        SpawnRandomEnemy(randomPoint);
    }

    private Vector2 GetRandomPointInTrigger(BoxCollider2D trigger)
    {
        Vector2 randomPoint = new Vector2(
            Random.Range(trigger.bounds.min.x, trigger.bounds.max.x),
            Random.Range(trigger.bounds.min.y, trigger.bounds.max.y)
        );

        return randomPoint;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.GetType().ToString() != Tags.CAPSULECOLLIDER2D)
        {
            playerInsideRoom = true;

            if (clearedRoom || disableGate) return;

            ActiveGates(true);

            SpawnWithinTrigger(spawnTrigger);
            SpawnWithinTrigger(spawnTrigger);
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

    private void SpawnRandomEnemy(Vector2 spawnPosition)
    {
        int random = Random.Range(0, enemySpawnScriptable.commonEnemies.Length);
        var enemy = Instantiate(enemySpawnScriptable.commonEnemies[random], spawnPosition, Quaternion.identity);
        currentEnemies.Add(enemy);
    }

    private void RoomEnemyCheck()
    {
        if (!playerInsideRoom) return;

        StartCoroutine(RoomEnemyCheckDelay());
    }

    private IEnumerator RoomEnemyCheckDelay()
    {
        yield return 0;

        for (int i = currentEnemies.Count - 1; i >= 0; i--)
        {
            var enemy = currentEnemies[i];

            if (!enemy.activeInHierarchy)
            {
                currentEnemies.RemoveAt(i);
            }
        }

        if (currentEnemies.Count == 0)
        {
            RoomCleared();
        }
    }

    // <-- TEST

    private void RoomBossCheck()
    {
        if (roomController.currentRoomType != RoomType.boss) return;

        //print("TEST");

    }
















}
