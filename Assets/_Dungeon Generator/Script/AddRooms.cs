using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    // Default
    normal,
    entry,
    empty,

    // Normal
    trap,
    treasure,

    // Dead End
    key,
    boss,
    shop,
    abandonShop,
}

public class AddRooms : MonoBehaviour
{
    private RoomTemplates templates;

    [Header("Rooms")]
    public RoomType currentRoomType = RoomType.normal;
    public string roomVariant;

    [Space]

    [Header("Normal Rooms")]
    [SerializeField] private GameObject[] roomType;

    [Space]

    [Header("Special Rooms")]
    [SerializeField] private GameObject shopRoom;

    private void Start()
    {
        templates = FindObjectOfType<RoomTemplates>();
        templates.rooms.Add(this);

        if (roomVariant == "T" || roomVariant == "L" || roomVariant == "R" || roomVariant == "B")
        {
            templates.deadEndRooms.Add(this);
        }

        SetRandomRoomType();
    }

    private void OnEnable()
    {
        RoomTemplates.OnBossChange += SetBossActive;
        RoomTemplates.OnShopChange += SetShopActive;
    }

    private void OnDisable()
    {
        RoomTemplates.OnBossChange -= SetBossActive;
        RoomTemplates.OnShopChange -= SetShopActive;
    }

    private void SetAllRoomActiveFalse()
    {
        foreach (var room in roomType)
        {
            room.SetActive(false);
        }
    }

    private void SetRandomRoomType()
    {
        SetAllRoomActiveFalse();

        int random = Random.Range(0, roomType.Length);
        roomType[random].SetActive(true);
    }

    private void SetBossActive()
    {
        if (currentRoomType == RoomType.boss)
        {
            SetAllRoomActiveFalse();
            if (shopRoom != null) shopRoom.SetActive(true);
        }
    }

    private void SetShopActive()
    {
        if (currentRoomType == RoomType.shop || currentRoomType == RoomType.abandonShop)
        {
            SetAllRoomActiveFalse();
            if (shopRoom != null) shopRoom.SetActive(true);
        }
    }


}
