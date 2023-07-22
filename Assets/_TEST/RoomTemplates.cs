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

    [Header("Special Room")]
    public float delayTime;
    private bool delayTimeCheck;

    [Space]

    public GameObject boss;
    public GameObject key;
    public GameObject shop;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadScene();
        }

        SpawnSpecialRoomType();
    }

    private void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void SpawnSpecialRoomType()
    {
        if (delayTime <= 0F && !delayTimeCheck)
        {
            SetBossRoom();
            SetSpeicalRoom();

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

    private void SetSpeicalRoom()
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

        // SPAWN SHOP ROOM
        if (roomsList.Count > 0)
        {
            int randomIndex = Random.Range(0, roomsList.Count);
            roomsList[randomIndex].currentRoomType = RoomType.shop;
            Instantiate(shop, roomsList[randomIndex].transform.position, Quaternion.identity);
            roomsList.RemoveAt(randomIndex);
        }








    }



}
