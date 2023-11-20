using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    // Default
    normal,
    entry,
    empty,

    // Special
    trap,
    treasure,

    // Dead End
    key,
    boss,
    shop,
    abandonShop,
    npc,
}
public enum DoorState
{
    none,
    open,
    closed,
}
public class RoomController : MonoBehaviour
{
    private RoomManager roomManager;

    [Header("Rooms ID")]
    public RoomType currentRoomType = RoomType.normal;
    public RoomShape roomShape;
    public bool added = false;

    [Space]

    [Header("Normal Rooms")]
    [SerializeField] private GameObject[] roomVariants;

    [Space]

    [Header("Special Rooms")]
    [SerializeField] private GameObject specialRoom;

    [HideInInspector] public GameObject activeRoomVariant;

    public DoorState topDoorState = DoorState.none;
    public DoorState rightDoorState = DoorState.none;
    public DoorState bottomDoorState = DoorState.none;
    public DoorState leftDoorState = DoorState.none;
    public int doorsChecked = 0;
    public int openRooms = 0;
    public bool allChecked = false;

    private void Awake()
    {
        roomManager = FindObjectOfType<RoomManager>();
        roomManager.currentRoomCount.Add(this);

        if (roomShape == RoomShape.T || roomShape == RoomShape.L|| roomShape == RoomShape.R|| roomShape == RoomShape.B)
        {
            roomManager.currentDeadEndRooms.Add(this);
        }

        SetRandomRoomType();
    }

    private void Update()
    {
        if(doorsChecked == 4 && !allChecked)
        {
            switch (roomShape)
            {
                case RoomShape.TR:
                    if(openRooms < 2)
                    {
                        if(topDoorState == DoorState.closed && rightDoorState == DoorState.open)
                        {
                            GameObject newRoom = Instantiate(roomManager.rightRooms[0], transform.position, Quaternion.identity);
                            newRoom.transform.parent = this.transform.parent;
                        }
                        else if (topDoorState == DoorState.open && rightDoorState == DoorState.closed)
                        {
                            GameObject newRoom = Instantiate(roomManager.topRooms[0], transform.position, Quaternion.identity);
                            newRoom.transform.parent = this.transform.parent;
                        }
                        Destroy(this.gameObject);
                    }
                    break;
                case RoomShape.TB:
                    if (openRooms < 2)
                    {
                        if (topDoorState == DoorState.closed && bottomDoorState == DoorState.open)
                        {
                            GameObject newRoom = Instantiate(roomManager.rightRooms[0], transform.position, Quaternion.identity);
                            newRoom.transform.parent = this.transform.parent;
                        }
                        else if (topDoorState == DoorState.open && bottomDoorState == DoorState.closed)
                        {
                            GameObject newRoom = Instantiate(roomManager.bottomRooms[0], transform.position, Quaternion.identity);
                            newRoom.transform.parent = this.transform.parent;
                        }
                        Destroy(this.gameObject);
                    }
                    break;
                case RoomShape.TL:
                    if (openRooms < 2)
                    {
                        if (topDoorState == DoorState.closed && leftDoorState == DoorState.open)
                        {
                            GameObject newRoom = Instantiate(roomManager.rightRooms[0], transform.position, Quaternion.identity);
                            newRoom.transform.parent = this.transform.parent;
                        }
                        else if (topDoorState == DoorState.open && leftDoorState == DoorState.closed)
                        {
                            GameObject newRoom = Instantiate(roomManager.leftRooms[0], transform.position, Quaternion.identity);
                            newRoom.transform.parent = this.transform.parent;
                        }
                        Destroy(this.gameObject);
                    }
                    break;
                case RoomShape.RB:
                    if (openRooms < 2)
                    {
                        if (rightDoorState == DoorState.closed && bottomDoorState == DoorState.open)
                        {
                            GameObject newRoom = Instantiate(roomManager.bottomRooms[0], transform.position, Quaternion.identity);
                            newRoom.transform.parent = this.transform.parent;
                        }
                        else if (rightDoorState == DoorState.open && bottomDoorState == DoorState.closed)
                        {
                            GameObject newRoom = Instantiate(roomManager.rightRooms[0],transform.position, Quaternion.identity);
                            newRoom.transform.parent = this.transform.parent;
                        }
                        Destroy(this.gameObject);
                    }
                    break;
                case RoomShape.LR:
                    if (openRooms < 2)
                    {
                        if (leftDoorState == DoorState.closed && rightDoorState == DoorState.open)
                        {
                            GameObject newRoom = Instantiate(roomManager.rightRooms[0], transform.position, Quaternion.identity);
                            newRoom.transform.parent = this.transform.parent;
                        }
                        else if (leftDoorState == DoorState.open && rightDoorState == DoorState.closed)
                        {
                            GameObject newRoom = Instantiate(roomManager.leftRooms[0], transform.position, Quaternion.identity);
                            newRoom.transform.parent = this.transform.parent;
                        }
                        Destroy(this.gameObject);
                    }
                    break;
                case RoomShape.LB:
                    if (openRooms < 2)
                    {
                        if (leftDoorState == DoorState.closed && bottomDoorState == DoorState.open)
                        {
                            GameObject newRoom = Instantiate(roomManager.bottomRooms[0], transform.position, Quaternion.identity);
                            newRoom.transform.parent = this.transform.parent;
                        }
                        else if (leftDoorState == DoorState.open && bottomDoorState == DoorState.closed)
                        {
                            GameObject newRoom = Instantiate(roomManager.leftRooms[0], transform.position, Quaternion.identity);
                            newRoom.transform.parent = this.transform.parent;
                        }
                        Destroy(this.gameObject);
                    }
                    break;
                //case RoomShape.TRB:
                //    break;
                //case RoomShape.TRL:
                //    break;
                //case RoomShape.TBL:
                //    break;
                //case RoomShape.RBL:
                //    break;
            }
            allChecked = true;
        }
    }

    private void SetAllRoomActiveFalse() // TURN ALL ROOMS FALSE
    {
        foreach (var room in roomVariants)
        {
            room.SetActive(false);
        }
    }

    private void SetRandomRoomType() // SET ONE RANDOM ROOM TO BE ACTIVE
    {
        SetAllRoomActiveFalse();

        int random = Random.Range(0, roomVariants.Length);
        activeRoomVariant = roomVariants[random];
        activeRoomVariant.SetActive(true);
    }

    public void SetSpecialRoomActive()
    {
        if (currentRoomType == RoomType.boss)
        {
            roomManager.bossSpawned = true;
        }
        else if (currentRoomType == RoomType.shop || currentRoomType == RoomType.abandonShop)
        {
            roomManager.shopSpawned = true;
        }
        else if (currentRoomType == RoomType.npc)
        {

        }
        else
        {
            return;
        }
        SetAllRoomActiveFalse();
        if (specialRoom != null) specialRoom.SetActive(true);
        GetComponentInChildren<GateManager>().disableGate = true;
    }
}
