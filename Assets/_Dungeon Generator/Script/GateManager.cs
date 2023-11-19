using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour
{
    private RoomController roomController;

    [Header("Enemy Spawn")]
    private GameObject randomSpawnPoints;
    private GameObject[] enemyPool;

    [Header("Gate Data")]
    public bool roomDrawn = false;
    public bool playerInsideRoom = false;
    public bool playerInLockZone = false;
    public bool clearedRoom = false;
    public bool disableGate = false;
    public bool locked = false;
    public PlayerCurrencies.KeyType keytype;

    [Header("Gate Referance")]
    [SerializeField] private GameObject roomVariants;
    [SerializeField] private GameObject gates;

    private RoomController activeRoomVariant;

    private void Awake()
    {
        roomController = GetComponentInParent<RoomController>();
        
        //RoomManager.onRoomsGenerated += CheckForDoors;
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
    }
    private void ActiveGates(bool active)
    {
        if (locked)
        {
            SpriteRenderer[] sprites = gates.GetComponentsInChildren<SpriteRenderer>();
            if(keytype == PlayerCurrencies.KeyType.Silver)
            {
                foreach (SpriteRenderer gate in sprites)
                {
                    gate.color = Color.grey;
                }
            }
            else if (keytype == PlayerCurrencies.KeyType.Gold)
            {
                foreach (SpriteRenderer gate in sprites)
                {
                    gate.color = Color.yellow;
                }
            }
        }
         gates.SetActive(active);
    }

    public void RoomCleared()
    {
        clearedRoom = true;
        ActiveGates(false);
    }

    private void SpawnWithinTrigger()
    {
        if(randomSpawnPoints == null)
        {
            return;
        }
        RandomSpawnManager.Instance.SpawnRandom(randomSpawnPoints);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.GetType().ToString() != Tags.CAPSULECOLLIDER2D && !playerInsideRoom)
        {
            playerInsideRoom = true;
            UpdatePlayerPositionOnMap(collision.transform.position);
            if (clearedRoom || disableGate || locked)
            {
                return;
            }
            if (roomVariants == null)
            {
                enemyPool = GameObject.FindGameObjectsWithTag("Enemy");
                if (enemyPool.Length == 0)
                {
                    RoomCleared();
                }
                else
                {
                    ActiveGates(true);
                }
                return;
            }
            
            if (roomController.activeRoomVariant.transform.Find("SpawnPointList") != null)
            {
                randomSpawnPoints = roomController.activeRoomVariant.transform.Find("SpawnPointList").gameObject;
                ActiveGates(true);
                SpawnWithinTrigger();
                return;
            }
            
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

    private void UpdatePlayerPositionOnMap(Vector3 playerPosition)
    {
        if(transform.position == playerPosition)
        {
            roomDrawn = true;
            return;
        }
        Vector2 direction = transform.position - playerPosition;
        float angle = Vector2.SignedAngle(transform.right, direction);
        if(angle > -135 && angle < -45) //entering from North side
        {
            MapGenerator.Instance.MovePositionMarker(CreateDirection.Down);
        }
        if (angle > 135 && angle <= 180 || angle < -135 && angle > -180) //entering from East side
        {
            MapGenerator.Instance.MovePositionMarker(CreateDirection.Left);
        }
        if (angle > 45 && angle < 135) //entering from South side
        {
            MapGenerator.Instance.MovePositionMarker(CreateDirection.Up);
        }
        if (angle >= 0 && angle < 45 || angle <= 0 && angle > -45) //entering from West side
        {
            MapGenerator.Instance.MovePositionMarker(CreateDirection.Right);
        }
        roomDrawn = true;
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
