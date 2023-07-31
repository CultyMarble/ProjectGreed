using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class RoomManager : MonoBehaviour
{
    public static event UnityAction OnBossChange;
    public static event UnityAction OnShopChange;

    [Header("Rooms")]
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject[] closedRooms;

    [Space]

    public List<RoomController> currentRooms;
    public List<RoomController> currentDeadEndRooms;

    [Space]

    [Header("Room Delay")]
    public float delaySpawnRoomType = 0.75F;
    private bool delaySpawnRoomCheck = false;

    [Header("Room Spawn Chance")]
    public float tresureRoomChance = 0.5F;
    public float abandonShopChance = 0.1F;

    [Space]

    [Header("Special Room")]
    public GameObject treasure;

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SpawnRoomType()
    {
        if (delaySpawnRoomType <= 0F && !delaySpawnRoomCheck)
        {
            SetBossRoom();
            SetDeadEndRoomType();
            SetNoramlRoomType();

            delaySpawnRoomCheck = true;
            delaySpawnRoomType = 0F;
        }
        else if (!delaySpawnRoomCheck)
        {
            delaySpawnRoomType -= Time.deltaTime;
        }
    }

    private void SetBossRoom() // LOOPS UNTIL FIND THE FIRST NON NULL ROOM
    {
        for (int i = currentRooms.Count - 1; i >= 0; i--)
        {
            if (currentRooms[i].currentRoomType != RoomType.empty)
            {
                currentRooms[i].currentRoomType = RoomType.boss;
                Instantiate(boss, currentRooms[i].transform.position, Quaternion.identity);
                OnBossChange?.Invoke();
                break;
            }
        }
    }

    private void SetDeadEndRoomType()
    {
        List<RoomController> roomsList = new List<RoomController>();

        foreach (RoomController room in currentDeadEndRooms)
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
            // NO DEAD-END ROOMS!!! SPAWN KEY ROOM IN A RANDOM ROOM!!!
            int randomIndex = Random.Range(0, currentRooms.Count);
            currentRooms[randomIndex].currentRoomType = RoomType.key;
            Instantiate(key, currentRooms[randomIndex].transform.position, Quaternion.identity);
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
            OnShopChange?.Invoke();
        }
    }

    private void SetNoramlRoomType()
    {
        List<RoomController> roomsList = new List<RoomController>();

        foreach (RoomController room in currentRooms)
        {
            if (room.currentRoomType == RoomType.normal)
            {
                roomsList.Add(room);
            }
        }

        // SPAWN TREASURE ROOM
        if (currentRooms.Count >= 6 && Random.value < tresureRoomChance || currentRooms.Count >= 10) // 6 Rooms or less (Gives a chance of spawn) | 6 Rooms or more (100%)
        {
            int randomIndex = Random.Range(0, roomsList.Count);
            roomsList[randomIndex].currentRoomType = RoomType.treasure;
            Instantiate(treasure, roomsList[randomIndex].transform.position, Quaternion.identity);
            roomsList.RemoveAt(randomIndex);
        }

        // <----------------------------------------------------------------------------------------------------------------------------------------------------------------------- FOR FUTRUE GARY
    }

}
