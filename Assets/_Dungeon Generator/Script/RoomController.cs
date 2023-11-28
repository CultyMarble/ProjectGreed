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

public class RoomController : MonoBehaviour
{
    [SerializeField] private GateManager gateManager;

    [Header("Rooms ID")]
    public RoomType currentRoomType = RoomType.normal;
    public RoomShape roomShape;
    public bool added = false;

    [Space]

    [Header("Normal Rooms")]
    [SerializeField] public GameObject[] roomVariants;

    [Space]

    [Header("Special Rooms")]
    [SerializeField] private GameObject specialRoom;

    [HideInInspector] public GameObject activeRoomVariant;

    
    private RoomManager roomManager;

    private void Awake()
    {
        roomManager = FindObjectOfType<RoomManager>();
        //if (roomShape == RoomShape.T || roomShape == RoomShape.L|| roomShape == RoomShape.R|| roomShape == RoomShape.B)
        //{
        //    roomManager.currentDeadEndRooms.Add(this);
        //}

        SetRandomRoomType();
    }

    private void Update()
    {
        
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
        gateManager = transform.parent.parent.GetComponentInChildren<GateManager>();
        gateManager.disableGate = true;
    }
}
