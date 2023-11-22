using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DoorState
{
    none,
    open,
    closed,
}
public class GateManager : MonoBehaviour
{
    public Room room;

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
    public bool standalone = false;

    public PlayerCurrencies.KeyType keytype;

    [Header("Gate Referance")]
    [SerializeField] private GameObject roomVariants;
    [SerializeField] private GameObject gates;

    public DoorState topDoorState = DoorState.none;
    public DoorState rightDoorState = DoorState.none;
    public DoorState bottomDoorState = DoorState.none;
    public DoorState leftDoorState = DoorState.none;

    public int doorsChecked = 0;
    public int openRooms = 0;
    public bool allChecked = false;

    private RoomController activeRoomVariant;

    private void Awake()
    {
        room = transform.parent.GetComponentInParent<Room>();
        
        //RoomManager.onRoomsGenerated += CheckForDoors;
    }

    private void Update()
    {
        if (doorsChecked == 4 && !allChecked)
        {
            DeadEndHandler();
        }
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

    private void DeadEndHandler()
    {
        switch (room.activeRoom.roomShape)
        {
            case RoomShape.TR:
                if (openRooms < 2)
                {
                    if (topDoorState == DoorState.closed && rightDoorState == DoorState.open)
                    {
                        room.SetActiveRoom(RoomShape.R);

                    //GameObject newRoom = Instantiate(roomManager.rightRooms[0], transform.position, Quaternion.identity);
                    //    newRoom.transform.parent = this.transform.parent;
                    }
                    else if (topDoorState == DoorState.open && rightDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.T);

                        //GameObject newRoom = Instantiate(roomManager.topRooms[0], transform.position, Quaternion.identity);
                        //newRoom.transform.parent = this.transform.parent;
                    }
                }
                break;
            case RoomShape.TB:
                if (openRooms < 2)
                {
                    if (topDoorState == DoorState.closed && bottomDoorState == DoorState.open)
                    {
                        room.SetActiveRoom(RoomShape.B);

                        //GameObject newRoom = Instantiate(roomManager.rightRooms[0], transform.position, Quaternion.identity);
                        //newRoom.transform.parent = this.transform.parent;
                    }
                    else if (topDoorState == DoorState.open && bottomDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.T);

                        //GameObject newRoom = Instantiate(roomManager.bottomRooms[0], transform.position, Quaternion.identity);
                        //newRoom.transform.parent = this.transform.parent;
                    }
                }
                break;
            case RoomShape.TL:
                if (openRooms < 2)
                {
                    if (topDoorState == DoorState.closed && leftDoorState == DoorState.open)
                    {
                        room.SetActiveRoom(RoomShape.L);

                        //GameObject newRoom = Instantiate(roomManager.rightRooms[0], transform.position, Quaternion.identity);
                        //newRoom.transform.parent = this.transform.parent;
                    }
                    else if (topDoorState == DoorState.open && leftDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.T);

                        //GameObject newRoom = Instantiate(roomManager.leftRooms[0], transform.position, Quaternion.identity);
                        //newRoom.transform.parent = this.transform.parent;
                    }
                }
                break;
            case RoomShape.RB:
                if (openRooms < 2)
                {
                    if (rightDoorState == DoorState.closed && bottomDoorState == DoorState.open)
                    {
                        room.SetActiveRoom(RoomShape.B);

                        //GameObject newRoom = Instantiate(roomManager.bottomRooms[0], transform.position, Quaternion.identity);
                        //newRoom.transform.parent = this.transform.parent;
                    }
                    else if (rightDoorState == DoorState.open && bottomDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.R);

                        //GameObject newRoom = Instantiate(roomManager.rightRooms[0], transform.position, Quaternion.identity);
                        //newRoom.transform.parent = this.transform.parent;
                    }
                }
                break;
            case RoomShape.LR:
                if (openRooms < 2)
                {
                    if (leftDoorState == DoorState.closed && rightDoorState == DoorState.open)
                    {
                        room.SetActiveRoom(RoomShape.R);

                        //GameObject newRoom = Instantiate(roomManager.rightRooms[0], transform.position, Quaternion.identity);
                        //newRoom.transform.parent = this.transform.parent;
                    }
                    else if (leftDoorState == DoorState.open && rightDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.L);

                        //GameObject newRoom = Instantiate(roomManager.leftRooms[0], transform.position, Quaternion.identity);
                        //newRoom.transform.parent = this.transform.parent;
                    }
                }
                break;
            case RoomShape.LB:
                if (openRooms < 2)
                {
                    if (leftDoorState == DoorState.closed && bottomDoorState == DoorState.open)
                    {
                        room.SetActiveRoom(RoomShape.B);

                        //GameObject newRoom = Instantiate(roomManager.bottomRooms[0], transform.position, Quaternion.identity);
                        //newRoom.transform.parent = this.transform.parent;
                    }
                    else if (leftDoorState == DoorState.open && bottomDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.L);

                        //GameObject newRoom = Instantiate(roomManager.leftRooms[0], transform.position, Quaternion.identity);
                        //newRoom.transform.parent = this.transform.parent;
                    }
                }
                break;
        }
        allChecked = true;
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
            if (standalone)
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
            
            if (room.activeRoom.activeRoomVariant.transform.Find("SpawnPointList") != null)
            {
                randomSpawnPoints = room.activeRoom.activeRoomVariant.transform.Find("SpawnPointList").gameObject;
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
