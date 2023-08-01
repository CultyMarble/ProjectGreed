using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GateManager : MonoBehaviour
{
    [Header("Enemy Spawn")]
    [SerializeField] private EnemySpawnScriptable enemySpawnScriptable;
    [SerializeField] private List<GameObject> currentEnemies;

    [Header("Gate Data")]
    [SerializeField] private bool playerInsideRoom = false;
    [SerializeField] private bool clearedRoom = false;
    [SerializeField] private bool disableGate = false;

    [Header("Gate Referance")]
    [SerializeField] private GameObject[] gates;

    private void OnEnable()
    {
        EnemyHealth.OnDespawnCallAll += RoomEnemyCheck;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.GetType().ToString() != Tags.CAPSULECOLLIDER2D)
        {
            playerInsideRoom = true;

            if (clearedRoom || disableGate) return;

            ActiveGates(true);

            SpawnRandomEnemy();
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

    private void SpawnRandomEnemy()
    {
        int random = Random.Range(0, enemySpawnScriptable.commonEnemies.Length);
        var enemy = Instantiate(enemySpawnScriptable.commonEnemies[random], gameObject.transform);
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



}
