using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class RoomManager : MonoBehaviour
{
    public static event UnityAction OnBossChange;
    public static event UnityAction OnShopChange;

    [SerializeField] private GameObject entryRoom;
    [SerializeField] public int minRooms;
    [SerializeField] public int maxRooms;
    [HideInInspector] public int potentialRooms;

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
    public float treasureRoomChance = 1.0F;
    public float abandonedShopChance = 0.1F;

    [Space]

    [Header("Special Room")]
    public GameObject treasure;

    [Space]

    [Header("Dead End Room")]
    public GameObject key;
    public GameObject boss;
    public GameObject shop;
    public GameObject abandonShop;

    private void Start()
    {
        LoadScene();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadScene();
        }

        SpawnRoomType();
    }

    private void LoadScene()
    {
        GameObject newEntryRoom;
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        newEntryRoom = Instantiate(entryRoom);
        newEntryRoom.transform.parent = this.transform;

        delaySpawnRoomCheck = false;
        delaySpawnRoomType = 0.75F;
    }

    private void SpawnRoomType()
    {
        if (delaySpawnRoomType <= 0F && !delaySpawnRoomCheck)
        {
            SetBossRoom();
            SetShopRoom();

            SetDeadEndRoomType();
            SetNormalRoomType();

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
        GameObject newBoss;
        for (int i = currentRooms.Count - 1; i >= 0; i--)
        {
            if (currentRooms[i].currentRoomType != RoomType.empty)
            {
                currentRooms[i].currentRoomType = RoomType.boss;
                newBoss = Instantiate(boss, currentRooms[i].transform.position, Quaternion.identity);
                newBoss.transform.parent = currentRooms[i].transform;
                OnBossChange?.Invoke();
                break;
            }
        }
    }
    private void SetShopRoom()
    {
        GameObject newShop;
        int randomIndex = Random.Range(0, currentRooms.Count);
        RoomType roomType = Random.value < abandonedShopChance ? RoomType.abandonShop : RoomType.shop;

        for (int i = currentRooms.Count - 1; i >= 0; i--)
        {
            if (currentRooms[i].currentRoomType != RoomType.empty && currentRooms[i].currentRoomType != RoomType.boss)
            {
                //currentRooms[randomIndex].currentRoomType = roomType;

                if (roomType == RoomType.shop)
                {
                    newShop = Instantiate(shop, currentRooms[i].transform.position, Quaternion.identity);
                    newShop.transform.parent = currentRooms[i].transform;
                }
                else if (roomType == RoomType.abandonShop)
                {
                    newShop = Instantiate(abandonShop, currentRooms[i].transform.position, Quaternion.identity);
                    newShop.transform.parent = currentRooms[i].transform;
                }
                OnShopChange?.Invoke();
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
        //if (roomsList.Count > 0)
        //{
        //    int randomIndex = Random.Range(0, roomsList.Count);
        //    roomsList[randomIndex].currentRoomType = RoomType.key;
        //    Instantiate(key, roomsList[randomIndex].transform.position, Quaternion.identity);
        //    roomsList.RemoveAt(randomIndex);
        //}
        //else
        //{
        //    // NO DEAD-END ROOMS!!! SPAWN KEY ROOM IN A RANDOM ROOM!!!
        //    int randomIndex = Random.Range(0, currentRooms.Count);
        //    currentRooms[randomIndex].currentRoomType = RoomType.key;
        //    Instantiate(key, currentRooms[randomIndex].transform.position, Quaternion.identity);
        //    Debug.LogError("THERE IS NO KEY IN THE DUNGEON!");
        //}

        // SPAWN SHOP ROOM
        //if (roomsList.Count > 0)
        //{
        //    GameObject newShop;
        //    int randomIndex = Random.Range(0, roomsList.Count);
        //    RoomType roomType = Random.value < abandonShopChance ? RoomType.abandonShop : RoomType.shop;

        //    roomsList[randomIndex].currentRoomType = roomType;

        //    if (roomType == RoomType.shop)
        //    {
        //        newShop = Instantiate(shop, roomsList[randomIndex].transform.position, Quaternion.identity);
        //        newShop.transform.parent = roomsList[randomIndex].transform;
        //    }
        //    else if (roomType == RoomType.abandonShop)
        //    {
        //        newShop = Instantiate(abandonShop, roomsList[randomIndex].transform.position, Quaternion.identity);
        //        newShop.transform.parent = roomsList[randomIndex].transform;
        //    }

        //    roomsList.RemoveAt(randomIndex);
        //    OnShopChange?.Invoke();
        //}
    }

    private void SetNormalRoomType()
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
        if (currentRooms.Count >= 6 && Random.value < treasureRoomChance || currentRooms.Count >= 10) // 6 Rooms or less (Gives a chance of spawn) | 6 Rooms or more (100%)
        {
            GameObject newTreasure;
            int randomIndex = Random.Range(0, roomsList.Count);
            roomsList[randomIndex].currentRoomType = RoomType.treasure;
            newTreasure = Instantiate(treasure, roomsList[randomIndex].transform.position, Quaternion.identity);
            newTreasure.transform.parent = roomsList[randomIndex].transform;

            roomsList.RemoveAt(randomIndex);
        }

        // <----------------------------------------------------------------------------------------------------------------------------------------------------------------------- FOR FUTRUE GARY
    }

}
