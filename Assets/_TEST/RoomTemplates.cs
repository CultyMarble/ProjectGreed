using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomTemplates : MonoBehaviour
{
    [Header("Rooms")]
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject[] closedRooms;

    [Space]

    public List<AddRooms> rooms;
    public List<AddRooms> deadEndRooms;

    [Space]

    [Header("Room Delay")]
    public float delayTime;
    private bool delayTimeCheck;

    [Header("Room Spawn Chance")]
    public float tresureRoomChance;
    public float abandonShopChance;

    [Space]

    [Header("Dead End Room")]
    public GameObject tresure;

    [Space]

    [Header("Dead End Room")]
    public GameObject key;
    public GameObject boss;
    public GameObject shop;
    public GameObject abandonShop;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadScene();
        }

        SpawnRoomType();
    }

    private void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void SpawnRoomType()
    {
        if (delayTime <= 0F && !delayTimeCheck)
        {
            SetBossRoom();
            SetDeadEndRoomType();
            SetNoramlRoomType();

            delayTimeCheck = true;
            delayTime = 0F;
        }
        else
        {
            if (!delayTimeCheck)
            {
                delayTime -= Time.deltaTime;
            }
        }
    }

    private void SetBossRoom()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            if (i == rooms.Count - 1)
            {
                rooms[i].currentRoomType = RoomType.boss;
                Instantiate(boss, rooms[i].transform.position, Quaternion.identity);
            }
        }
    }

    private void SetNoramlRoomType()
    {
        List<AddRooms> roomsList = new List<AddRooms>();

        foreach (AddRooms room in rooms)
        {
            if (room.currentRoomType == RoomType.normal)
            {
                roomsList.Add(room);
            }
        }

        // SPAWN TREASURE ROOM
        if (rooms.Count >= 6 && Random.value < tresureRoomChance || rooms.Count >= 10) // 6 Rooms or less (Gives a chance of spawn) | 6 Rooms or more (100%)
        {
            int randomIndex = Random.Range(0, roomsList.Count);
            roomsList[randomIndex].currentRoomType = RoomType.treasure;
            Instantiate(tresure, roomsList[randomIndex].transform.position, Quaternion.identity);
            roomsList.RemoveAt(randomIndex);
        }











    }

    private void SetDeadEndRoomType()
    {
        List<AddRooms> roomsList = new List<AddRooms>();

        foreach (AddRooms room in deadEndRooms)
        {
            if (room.currentRoomType == RoomType.normal)
            {
                roomsList.Add(room);
            }
        }

        // SPAWN KEY ROOM
        if (roomsList.Count > 0)
        {
            int randomIndex = Random.Range(0, roomsList.Count);
            roomsList[randomIndex].currentRoomType = RoomType.key;
            Instantiate(key, roomsList[randomIndex].transform.position, Quaternion.identity);
            roomsList.RemoveAt(randomIndex);
        }
        else
        {
            int randomIndex = Random.Range(0, rooms.Count);
            roomsList[randomIndex].currentRoomType = RoomType.key;
            Instantiate(key, rooms[randomIndex].transform.position, Quaternion.identity);
            Debug.LogError("THERE IS NO KEY IN THE DUNGEON!");
        }

        // SPAWN SHOP ROOM
        if (roomsList.Count > 0)
        {
            int randomIndex = Random.Range(0, roomsList.Count);
            RoomType roomType = Random.value < abandonShopChance ? RoomType.abandonShop : RoomType.shop;

            roomsList[randomIndex].currentRoomType = roomType;

            if (roomType == RoomType.shop)
            {
                Instantiate(shop, roomsList[randomIndex].transform.position, Quaternion.identity);
            }
            else if (roomType == RoomType.abandonShop)
            {
                Instantiate(abandonShop, roomsList[randomIndex].transform.position, Quaternion.identity);
            }

            roomsList.RemoveAt(randomIndex);
        }

    }
















}
