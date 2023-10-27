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
    private RoomManager roomManager;

    [Header("Rooms ID")]
    public RoomType currentRoomType = RoomType.normal;
    public string roomVariant;
    public bool added = false;

    [Space]

    [Header("Normal Rooms")]
    [SerializeField] private GameObject[] roomType;

    [Space]

    [Header("Special Rooms")]
    [SerializeField] private GameObject specialRoom;

    private void Awake()
    {
        roomManager = FindObjectOfType<RoomManager>();
        if (currentRoomType == RoomType.entry)
        {
            int hi = 2;
        }
        roomManager.currentRoomCount.Add(this);

        //if (!roomManager.currentRoomTotal.Contains(this.GetComponent<RoomController>()))
        //{
        //    roomManager.currentRoomCount.Add(this);
        //    //roomManager.currentRoomTotal.Add(this);
        //}

        if (roomVariant == "T" || roomVariant == "L" || roomVariant == "R" || roomVariant == "B")
        {
            roomManager.currentDeadEndRooms.Add(this);
        }

        SetRandomRoomType();
    }

    private void SetAllRoomActiveFalse() // TURN ALL ROOMS FALSE
    {
        foreach (var room in roomType)
        {
            room.SetActive(false);
        }
    }

    private void SetRandomRoomType() // SET ONE RANDOM ROOM TO BE ACTIVE
    {
        SetAllRoomActiveFalse();

        int random = Random.Range(0, roomType.Length);
        roomType[random].SetActive(true);
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
