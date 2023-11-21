using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class RoomManager : MonoBehaviour
{
    [Header("Room Spawn Parameters")]

    public GameObject aStar;
    public GameObject room;
    public int minRooms;
    public int maxRooms;

    [Header("Active Rooms")]

    public List<RoomController> currentRoomTotal;
    public List<RoomController> currentRoomCount;
    public List<RoomController> currentDeadEndRooms;

    [Header("Spawning")]
    [HideInInspector] public bool bossSpawned = false;
    [HideInInspector] public bool shopSpawned = false;
    [HideInInspector] public bool treasureSpawned = false;
    [HideInInspector] public bool keySpawned = false;
    [HideInInspector] public bool roomsFinished = false;
    [HideInInspector] public bool mapFinished = false;
    [HideInInspector] public int loops = 0;
    private float delaySpawnRoomType = 0.1F;
    private bool delaySpawnRoomCheck = false;

    [Space]

    [Header("Special Room Objects")]
    public GameObject[] treasureItems;
    public List<GameObject> npcList;
    public GameObject[] keys;
    public GameObject boss;
    public GameObject shop;
    public GameObject abandonedShop;
    public GameObject startRoomDialogue;
    public GameObject silverRoomDialogue;
    public GameObject goldRoomDialogue;

    public delegate void OnRoomsGenerated();
    public static event OnRoomsGenerated onRoomsGenerated;

    private void Start()
    {
        LoadScene();
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    LoadScene();
        //}

        if (!mapFinished)
        {
            SpawnRoomTypes();
        }
        else
        {
            SetBossRoom();
        }
    }

    private void LoadScene()
    {
        shopSpawned = false;
        keySpawned = false;

        if (loops == 2)
        {
            bossSpawned = false;
            delaySpawnRoomCheck = false;
        }
        if (loops > 0 && currentRoomCount.Count > 3)
        {
            StartNewBranch();
            delaySpawnRoomType = 1F;
            return;
        }
        else if (loops > 0)
        {
            loops = 0;
        }
        if (currentRoomCount.Count <= 3)
        {
            foreach (RoomController room in currentRoomTotal)
            {
                Destroy(room.gameObject);
            }
            currentRoomTotal.Clear();
        }
        foreach (RoomController room in currentRoomCount)
        {
            Destroy(room.gameObject);
        }
        currentRoomCount.Clear();
        GameObject newEntryRoom;
        GameObject startDialogue;
        newEntryRoom = Instantiate(room);
        newEntryRoom.transform.parent = this.transform;
        startDialogue = Instantiate(startRoomDialogue);
        startDialogue.transform.parent = newEntryRoom.transform;

        delaySpawnRoomCheck = false;
        delaySpawnRoomType = 0.5F;
    }

    private void SpawnRoomTypes()
    {
        if(currentRoomCount.Count == 0)
        {
            delaySpawnRoomCheck = true;
            delaySpawnRoomType = 0F;
            CheckBranchFinished();
            return;
        }
        if (delaySpawnRoomType <= 0F && !delaySpawnRoomCheck)
        {
            SetShopRoom();
            SetKeyRoom();
            SetTreasureRoom();
            SetNPCRoom();

            delaySpawnRoomCheck = true;
            delaySpawnRoomType = 0F;
            CheckBranchFinished();
        }
        else if (!delaySpawnRoomCheck)
        {
            delaySpawnRoomType -= Time.deltaTime;
        }
        else
        {
            return;
        }
    }

    private void SetBossRoom()
    {
        if (bossSpawned)
        {
            return;
        }
        GameObject newBoss;
        for (int i = currentRoomTotal.Count - 1; i >= 0; i--)
        {
            if (currentRoomTotal[i].currentRoomType != RoomType.empty)
            {
                currentRoomTotal[i].currentRoomType = RoomType.boss;
                newBoss = Instantiate(boss, currentRoomTotal[i].transform.position, Quaternion.identity);
                newBoss.transform.parent = currentRoomTotal[i].transform;
                currentRoomTotal[i].SetSpecialRoomActive();

                break;
            }
        }
    }

    private void SetShopRoom()
    {
        if (shopSpawned)
        {
            return;
        }
        GameObject newShop;
        int randomIndex = Random.Range(0, currentRoomCount.Count);
        int chance = Random.Range(0, 4);
        for(int i = currentRoomCount.Count/2; i < currentRoomCount.Count-1; i++)
        {
            if(currentRoomCount[randomIndex].currentRoomType != RoomType.normal)
            {
                continue;
            }
            else
            {
                currentRoomCount[i].currentRoomType = RoomType.shop;
                if(chance == 3)
                {
                    newShop = Instantiate(abandonedShop, currentRoomCount[i].transform.position, Quaternion.identity);
                    newShop.transform.SetParent(currentRoomCount[i].transform);
                }
                else
                {
                    newShop = Instantiate(shop, currentRoomCount[i].transform.position, Quaternion.identity);
                    newShop.transform.SetParent(currentRoomCount[i].transform);
                }
                
                currentRoomCount[i].SetSpecialRoomActive();
                shopSpawned = true;
                return;
            }
        }
        
    }

    public void SetTreasureRoom()
    {
        if (treasureSpawned)
        {
            return;
        }
        GameObject newTreasure;
        int randomItemIndex = Random.Range(0, treasureItems.Length);
        int randomRoomIndex = Random.Range(0, currentRoomCount.Count);

        if (currentRoomCount[randomRoomIndex].currentRoomType != RoomType.normal)
        {
            return;
        }

        currentRoomCount[randomRoomIndex].currentRoomType = RoomType.treasure;
        newTreasure = Instantiate(treasureItems[randomItemIndex], currentRoomCount[randomRoomIndex].transform.position, Quaternion.identity);
        newTreasure.transform.parent = currentRoomCount[randomRoomIndex].transform;
    }
    public void SetKeyRoom()
    {
        if (keySpawned || loops == 2)
        {
            keySpawned = true;
            return;
        }
        GameObject newKey;

        for (int i = 0; i < currentRoomCount.Count; i++)
        {
            if (currentRoomCount[i].currentRoomType == RoomType.normal)
            {
                currentRoomCount[i].currentRoomType = RoomType.key;
                newKey = Instantiate(keys[loops], currentRoomCount[i].transform.position, Quaternion.identity);
                newKey.transform.parent = currentRoomCount[i].transform;
                keySpawned = true;
                return;
            }
        }
    }
    public void SetNPCRoom()
    {
        if(npcList.Count == 0)
        {
            return;
        }
        GameObject newNPC;
        int randomItemIndex = Random.Range(0, npcList.Count);
        int randomRoomIndex = Random.Range(0, currentRoomCount.Count);

        if (currentRoomCount[randomRoomIndex].currentRoomType != RoomType.normal || currentRoomCount.Count <=0)
        {
            return;
        }

        currentRoomCount[randomRoomIndex].currentRoomType = RoomType.npc;
            
        newNPC = Instantiate(npcList[randomItemIndex], currentRoomCount[randomRoomIndex].transform.position, Quaternion.identity);
        newNPC.transform.parent = currentRoomCount[randomRoomIndex].transform;

        npcList.RemoveAt(randomItemIndex);
        currentRoomCount[randomRoomIndex].SetSpecialRoomActive();
    }

    public void CheckBranchFinished()
    {
        if (currentRoomCount.Count < minRooms || currentRoomCount.Count > maxRooms || !shopSpawned || !keySpawned)
        {
            if(currentRoomCount.Count <= 1)
            {
                loops = 0;
            }
            LoadScene();
            return;
        }
        else
        {
            foreach(RoomController room in currentRoomCount)
            {
                currentRoomTotal.Add(room);
            }
            currentRoomCount.Clear();
            CheckMapFinished();

            if (loops < 2 && !mapFinished)
            {
                StartNewBranch();
                loops++;
            }
            delaySpawnRoomCheck = false;
            roomsFinished = true;
        }
    }
    public void CheckMapFinished()
    {
        if(loops != 2 || currentRoomTotal.Count < 3 * minRooms)
        {
            roomsFinished = false;
            return;
        }
        else
        {
            foreach (RoomController room in currentRoomCount)
            {
                Destroy(room.gameObject);
            }
            currentRoomCount.Clear();
            roomsFinished = true;
            mapFinished = true;

            SceneControlManager.Instance.CurrentGameplayState = GameplayState.Ongoing;
            aStar.SetActive(true);
            if (onRoomsGenerated != null)
            {
                onRoomsGenerated();
            }
        }
    }

    public void StartNewBranch()
    {
        if(loops > 2)
        {
            return;
        }
        if (currentRoomCount.Count > 0)
        {
            Vector3 startLocation = currentRoomCount[0].transform.position;
            foreach (RoomController room in currentRoomCount)
            {
                if (room.currentRoomType == RoomType.entry)
                {
                    startLocation = room.transform.position;
                    break;
                }
            }
            foreach (RoomController room in currentRoomCount)
            {
                Destroy(room.gameObject);
            }
            currentRoomCount.Clear();
            RoomController newRoom = Instantiate(room, startLocation, Quaternion.identity).GetComponent<RoomController>();
            newRoom.transform.parent = this.transform;
            if (newRoom != null)
            {
                newRoom.gameObject.GetComponentInChildren<GateManager>().locked = true;
                if(loops == 1)
                {
                    newRoom.gameObject.GetComponentInChildren<GateManager>().keytype = PlayerCurrencies.KeyType.Silver;
                    GameObject silverDialogue;
                    silverDialogue = Instantiate(silverRoomDialogue);
                    silverDialogue.transform.parent = newRoom.gameObject.transform;
                    silverDialogue.transform.position = newRoom.gameObject.transform.position;
                }
                else if(loops == 2)
                {
                    newRoom.gameObject.GetComponentInChildren<GateManager>().keytype = PlayerCurrencies.KeyType.Gold;
                    GameObject goldDialogue;
                    goldDialogue = Instantiate(goldRoomDialogue);
                    goldDialogue.transform.parent = newRoom.gameObject.transform;
                    goldDialogue.transform.position = newRoom.gameObject.transform.position;
                }
            }
            delaySpawnRoomCheck = false;
            delaySpawnRoomType = 0.1F;
            shopSpawned = false;
            return;
        }
        else
        {
            for (int i = currentRoomTotal.Count - 2; i > 0; i--)
            {
                if (currentRoomTotal[i].currentRoomType == RoomType.normal)
                {
                    Destroy(currentRoomTotal[i].gameObject);
                    currentRoomCount.Clear();
                    RoomController newRoom = Instantiate(room, new Vector2(currentRoomTotal[i].transform.position.x, currentRoomTotal[i].transform.position.y), Quaternion.identity).GetComponent<RoomController>();
                    
                    currentRoomTotal.RemoveAt(i);
                    newRoom.transform.parent = this.transform;
                    newRoom.transform.GetComponent<RoomController>().added = true;
                    if (newRoom != null)
                    {
                        newRoom.gameObject.GetComponentInChildren<GateManager>().locked = true;
                        if (loops == 0)
                        {
                            newRoom.gameObject.GetComponentInChildren<GateManager>().keytype = PlayerCurrencies.KeyType.Silver;
                            GameObject silverDialogue;
                            silverDialogue = Instantiate(silverRoomDialogue);
                            silverDialogue.transform.parent = newRoom.gameObject.transform;
                            silverDialogue.transform.position = newRoom.gameObject.transform.position;
                        }
                        else if (loops == 1)
                        {
                            newRoom.gameObject.GetComponentInChildren<GateManager>().keytype = PlayerCurrencies.KeyType.Gold;
                            GameObject goldDialogue;
                            goldDialogue = Instantiate(goldRoomDialogue);
                            goldDialogue.transform.parent = newRoom.gameObject.transform;
                            goldDialogue.transform.position = newRoom.gameObject.transform.position;

                        }
                    }

                    delaySpawnRoomCheck = false;
                    delaySpawnRoomType = 0.1F;
                    shopSpawned = false;
                    return;
                }
            }
        }
    }
}
