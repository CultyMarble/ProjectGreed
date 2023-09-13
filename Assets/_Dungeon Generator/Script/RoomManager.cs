using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private GameObject aStar;

    [SerializeField] private GameObject entryRoom;
    [SerializeField] public int minRooms;
    [SerializeField] public int maxRooms;
    [SerializeField] public float centerRoomChance;
    [HideInInspector] public int potentialRooms;

    [Header("Rooms")]
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject[] centerRoom;
    public GameObject[] closedRooms;

    [Space]

    public List<RoomController> currentRooms;
    public List<RoomController> currentDeadEndRooms;

    [Space]

    [Header("Room Delay")]
    public float delaySpawnRoomType = 0.75F;
    private bool delaySpawnRoomCheck = false;
    [HideInInspector] public bool bossSpawned = false;
    [HideInInspector] public bool shopSpawned = false;

    [Header("Room Spawn Chance")]
    public float npcRoomChance = 1.0F;
    public float abandonedShopChance = 0.1F;

    [Space]

    [Header("Special Room")]
    public GameObject[] treasureItems;
    public GameObject[] npcList;

    [Space]

    [Header("Dead End Room")]
    public GameObject key;
    public GameObject boss;
    public GameObject shop;
    public GameObject abandonedShop;

    public delegate void OnRoomsGenerated();
    public static event OnRoomsGenerated onRoomsGenerated;

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
        if (bossSpawned && shopSpawned)
        {
            //if (currentRooms.Count < 20)
            //{
            //    SceneControlManager.Instance.LoadScene("DemoSceneDungeon", Vector3.zero);
            //}
            //else
            //{
            //    aStar.SetActive(true);
            //}
            RoomsFinished();
        }
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
            SetDeadEndRoomType();
            SetNormalRoomType();
            SetShopRoom();

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
                currentRooms[i].SetSpecialRoomActive();

                break;
            }
        }
    }

    private void SetShopRoom()
    {
        GameObject newShop;
        int randomIndex = Random.Range(0, currentRooms.Count);
        RoomType roomType = Random.value < abandonedShopChance ? RoomType.abandonShop : RoomType.shop;

        while (currentRooms[randomIndex].currentRoomType != RoomType.normal)
        {
            randomIndex = Random.Range(0, currentRooms.Count);
        }
        currentRooms[randomIndex].currentRoomType = RoomType.shop;
        if (roomType == RoomType.shop)
        {
            newShop = Instantiate(shop, currentRooms[randomIndex].transform.position, Quaternion.identity);
            newShop.transform.parent = currentRooms[randomIndex].transform;
        }
        else if (roomType == RoomType.abandonShop)
        {
            newShop = Instantiate(abandonedShop, currentRooms[randomIndex].transform.position, Quaternion.identity);
            newShop.transform.parent = currentRooms[randomIndex].transform;
        }
        currentRooms[randomIndex].SetSpecialRoomActive();
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
        if (currentRooms.Count >= 6 && currentRooms.Count >= 10) // 6 Rooms or less (Gives a chance of spawn) | 6 Rooms or more (100%)
        {
            GameObject newTreasure;
            int randomItemIndex = Random.Range(0, treasureItems.Length);  
            int randomRoomIndex = Random.Range(0, roomsList.Count);
            roomsList[randomRoomIndex].currentRoomType = RoomType.treasure;
            newTreasure = Instantiate(treasureItems[randomItemIndex], roomsList[randomRoomIndex].transform.position, Quaternion.identity);
            newTreasure.transform.parent = roomsList[randomRoomIndex].transform;

            roomsList.RemoveAt(randomRoomIndex);
        }
        // SPAWN NPC ROOM
        if (currentRooms.Count >= 6 && Random.value < npcRoomChance || currentRooms.Count >= 10) // 6 Rooms or less (Gives a chance of spawn) | 6 Rooms or more (100%)
        {
            GameObject newNPC;
            int randomItemIndex = Random.Range(0, npcList.Length);
            int randomRoomIndex = Random.Range(0, roomsList.Count);
            roomsList[randomRoomIndex].currentRoomType = RoomType.npc;
            newNPC = Instantiate(npcList[randomItemIndex], roomsList[randomRoomIndex].transform.position, Quaternion.identity);
            newNPC.transform.parent = roomsList[randomRoomIndex].transform;

            roomsList.RemoveAt(randomRoomIndex);
            currentRooms[randomRoomIndex].SetSpecialRoomActive();
        }
    }
    public void RoomsFinished()
    {
        aStar.SetActive(true);
        if(onRoomsGenerated != null)
        {
            onRoomsGenerated();
        }
    }
}
