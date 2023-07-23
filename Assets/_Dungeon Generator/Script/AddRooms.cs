using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    // Default
    normal,
    entry,

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

    [SerializeField] private GameObject[] roomType;

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

    private void SetRandomRoomType()
    {
        foreach (var room in roomType)
        {
            room.SetActive(false);
        }

        int random = Random.Range(0, roomType.Length);
        roomType[random].SetActive(true);

    }

}
