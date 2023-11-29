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
    [SerializeField] private GameObject topDoor;
    [SerializeField] private GameObject rightDoor;
    [SerializeField] private GameObject bottomDoor;
    [SerializeField] private GameObject leftDoor;

    public DoorState topDoorState = DoorState.none;
    public DoorState rightDoorState = DoorState.none;
    public DoorState bottomDoorState = DoorState.none;
    public DoorState leftDoorState = DoorState.none;

    public int doorsChecked = 0;
    public int openRooms = 0;
    public bool allChecked = false;
    public bool activated = false;

    private void Awake()
    {
        if (!standalone)
        {
            room = transform.parent.GetComponentInParent<Room>();
        }
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
        RandomSpawnManager.Instance.SpawnRandom(randomSpawnPoints,room.difficulty);
    }

    private void DeadEndHandler()
    {
        RoomType previousRoomType = room.activeRoom.currentRoomType;
        switch (room.activeRoom.roomShape)
        {
            case RoomShape.Centre:
                if (topDoorState == DoorState.closed)
                {
                    topDoor.SetActive(true);
                }
                else
                {
                    topDoor.SetActive(false);
                }
                if (rightDoorState == DoorState.closed)
                {
                    rightDoor.SetActive(true);
                }
                else
                {
                    rightDoor.SetActive(false);
                }
                if (bottomDoorState == DoorState.closed)
                {
                    bottomDoor.SetActive(true);
                }
                else
                {
                    bottomDoor.SetActive(false);
                }
                if (leftDoorState == DoorState.closed)
                {
                    leftDoor.SetActive(true);
                }
                else
                {
                    leftDoor.SetActive(false);
                }
                break;
            case RoomShape.TR:
                if (openRooms < 2)
                {
                    if (topDoorState == DoorState.closed && rightDoorState == DoorState.open)
                    {
                        room.SetActiveRoom(RoomShape.R);
                    }
                    else if (topDoorState == DoorState.open && rightDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.T);
                    }
                }
                break;
            case RoomShape.TB:
                if (openRooms < 2)
                {
                    if (topDoorState == DoorState.closed && bottomDoorState == DoorState.open)
                    {
                        room.SetActiveRoom(RoomShape.B);
                    }
                    else if (topDoorState == DoorState.open && bottomDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.T);
                    }
                }
                break;
            case RoomShape.TL:
                if (openRooms < 2)
                {
                    if (topDoorState == DoorState.closed && leftDoorState == DoorState.open)
                    {
                        room.SetActiveRoom(RoomShape.L);
                    }
                    else if (topDoorState == DoorState.open && leftDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.T);
                    }
                }
                break;
            case RoomShape.RB:
                if (openRooms < 2)
                {
                    if (rightDoorState == DoorState.closed && bottomDoorState == DoorState.open)
                    {
                        room.SetActiveRoom(RoomShape.B);
                    }
                    else if (rightDoorState == DoorState.open && bottomDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.R);
                    }
                }
                break;
            case RoomShape.LR:
                if (openRooms < 2)
                {
                    if (leftDoorState == DoorState.closed && rightDoorState == DoorState.open)
                    {
                        room.SetActiveRoom(RoomShape.R);
                    }
                    else if (leftDoorState == DoorState.open && rightDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.L);
                    }
                }
                break;
            case RoomShape.LB:
                if (openRooms < 2)
                {
                    if (leftDoorState == DoorState.closed && bottomDoorState == DoorState.open)
                    {
                        room.SetActiveRoom(RoomShape.B);
                    }
                    else if (leftDoorState == DoorState.open && bottomDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.L);
                    }
                }
                break;
            case RoomShape.TRB:
                if (openRooms < 3)
                {
                    if (bottomDoorState == DoorState.open && topDoorState == DoorState.closed && rightDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.B);
                    }
                    else if (topDoorState == DoorState.open && rightDoorState == DoorState.closed && bottomDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.T);
                    }
                    else if (rightDoorState == DoorState.open && topDoorState == DoorState.closed && bottomDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.R);
                    }
                    else if (topDoorState == DoorState.open && rightDoorState == DoorState.open && bottomDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.TR);
                    }
                    else if (topDoorState == DoorState.open && bottomDoorState == DoorState.open && rightDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.TB);
                    }
                    else if (topDoorState == DoorState.closed && bottomDoorState == DoorState.open && rightDoorState == DoorState.open)
                    {
                        room.SetActiveRoom(RoomShape.RB);
                    }
                }
                break;
            case RoomShape.TRL:
                if (openRooms < 3)
                {
                    if (leftDoorState == DoorState.open && topDoorState == DoorState.closed && rightDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.L);
                    }
                    else if (topDoorState == DoorState.open && rightDoorState == DoorState.closed && leftDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.T);
                    }
                    else if (rightDoorState == DoorState.open && topDoorState == DoorState.closed && leftDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.R);
                    }
                    else if (topDoorState == DoorState.open && rightDoorState == DoorState.open && leftDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.TR);
                    }
                    else if (topDoorState == DoorState.open && leftDoorState == DoorState.open && rightDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.TL);
                    }
                    else if (topDoorState == DoorState.closed && leftDoorState == DoorState.open && rightDoorState == DoorState.open)
                    {
                        room.SetActiveRoom(RoomShape.LR);
                    }
                }
                break;
            case RoomShape.TBL:
                if (openRooms < 3)
                {
                    if (leftDoorState == DoorState.open && topDoorState == DoorState.closed && bottomDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.L);
                    }
                    else if (topDoorState == DoorState.open && bottomDoorState == DoorState.closed && leftDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.T);
                    }
                    else if (bottomDoorState == DoorState.open && topDoorState == DoorState.closed && leftDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.B);
                    }
                    else if (topDoorState == DoorState.open && bottomDoorState == DoorState.open && leftDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.TB);
                    }
                    else if (topDoorState == DoorState.open && leftDoorState == DoorState.open && bottomDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.TL);
                    }
                    else if (topDoorState == DoorState.closed && leftDoorState == DoorState.open && bottomDoorState == DoorState.open)
                    {
                        room.SetActiveRoom(RoomShape.LB);
                    }
                }
                break;
            case RoomShape.RBL:
                if (openRooms < 3)
                {
                    if (bottomDoorState == DoorState.open && leftDoorState == DoorState.closed && rightDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.B);
                    }
                    else if (leftDoorState == DoorState.open && rightDoorState == DoorState.closed && bottomDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.L);
                    }
                    else if (rightDoorState == DoorState.open && leftDoorState == DoorState.closed && bottomDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.R);
                    }
                    else if (leftDoorState == DoorState.open && rightDoorState == DoorState.open && bottomDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.LR);
                    }
                    else if (leftDoorState == DoorState.open && bottomDoorState == DoorState.open && rightDoorState == DoorState.closed)
                    {
                        room.SetActiveRoom(RoomShape.LB);
                    }
                    else if (leftDoorState == DoorState.closed && bottomDoorState == DoorState.open && rightDoorState == DoorState.open)
                    {
                        room.SetActiveRoom(RoomShape.RB);
                    }
                }
                break;
        }
        switch (previousRoomType)
        {
            case RoomType.boss:
                room.activeRoom.currentRoomType = RoomType.boss;
                room.activeRoom.SetSpecialRoomActive();
                break;
            case RoomType.shop:
                room.activeRoom.currentRoomType = RoomType.shop;
                room.activeRoom.SetSpecialRoomActive();
                break;
            case RoomType.npc:
                room.activeRoom.currentRoomType = RoomType.npc;
                room.activeRoom.SetSpecialRoomActive();
                break;
            case RoomType.normal:
                room.activeRoom.currentRoomType = RoomType.normal;
                break;
            case RoomType.entry:
                room.activeRoom.currentRoomType = RoomType.entry;
                break;
            case RoomType.empty:
                room.activeRoom.currentRoomType = RoomType.empty;
                break;
            case RoomType.trap:
                room.activeRoom.currentRoomType = RoomType.trap;
                break;
            case RoomType.treasure:
                room.activeRoom.currentRoomType = RoomType.treasure;
                break;
            case RoomType.key:
                room.activeRoom.currentRoomType = RoomType.key;
                break;
            case RoomType.abandonShop:
                room.activeRoom.currentRoomType = RoomType.abandonShop;
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
                ActiveGates(true);
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
    public void SetDoorsActive()
    {
        if (activated)
        {
            return;
        }
        topDoorState = DoorState.closed;
        rightDoorState = DoorState.closed;
        leftDoorState = DoorState.closed;
        bottomDoorState = DoorState.closed;

        switch (room.activeRoom.roomShape)
        {
            case RoomShape.Centre:
                topDoorState = DoorState.open;
                rightDoorState = DoorState.open;
                leftDoorState = DoorState.open;
                bottomDoorState = DoorState.open;
                break;
            case RoomShape.T:
                topDoorState = DoorState.open;
                break;
            case RoomShape.R:
                rightDoorState = DoorState.open;
                break;
            case RoomShape.B:
                bottomDoorState = DoorState.open;
                break;
            case RoomShape.L:
                leftDoorState = DoorState.open;
                break;
            case RoomShape.TR:
                topDoorState = DoorState.open;
                rightDoorState = DoorState.open;
                break;
            case RoomShape.TB:
                topDoorState = DoorState.open;
                bottomDoorState = DoorState.open;
                break;
            case RoomShape.TL:
                topDoorState = DoorState.open;
                leftDoorState = DoorState.open;
                break;
            case RoomShape.RB:
                rightDoorState = DoorState.open;
                bottomDoorState = DoorState.open;
                break;
            case RoomShape.LR:
                leftDoorState = DoorState.open;
                rightDoorState = DoorState.open;
                break;
            case RoomShape.LB:
                leftDoorState = DoorState.open;
                bottomDoorState = DoorState.open;
                break;
            case RoomShape.TRB:
                topDoorState = DoorState.open;
                rightDoorState = DoorState.open; 
                bottomDoorState = DoorState.open;
                break;
            case RoomShape.TRL:
                topDoorState = DoorState.open;
                rightDoorState = DoorState.open;
                leftDoorState = DoorState.open;
                break;
            case RoomShape.TBL:
                topDoorState = DoorState.open;
                bottomDoorState = DoorState.open;
                leftDoorState = DoorState.open;
                break;
            case RoomShape.RBL:
                rightDoorState = DoorState.open;
                bottomDoorState = DoorState.open;
                leftDoorState = DoorState.open;
                break;
        }
        activated = true;
    }

}
