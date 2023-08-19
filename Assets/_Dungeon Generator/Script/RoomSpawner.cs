using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    private RoomManager roomManager;

    [Tooltip(" 1 --> Need Bottom Door\r\n 2 --> Need Top Door\r\n 3 --> Need Left Door\r\n 4 --> Need Right Door")]
    [SerializeField] private int openingDirection;
    [SerializeField] private bool spawned = false;

    private float waitTime = 4F;

    private void Start()
    {
        Destroy(gameObject, waitTime);

        roomManager = FindObjectOfType<RoomManager>();
        Invoke("Spawn", 0.05F);
    }

    private void Spawn()
    {
        if (roomManager == null)
        {
            roomManager = FindObjectOfType<RoomManager>();
        }
        if (!spawned)
        {
            if(Random.value < roomManager.centerRoomChance)
            {
                InstantiateRandomRoom(1, roomManager.centerRoom);
            }
            else if (openingDirection == 1)
            {
                InstantiateRandomRoom(roomManager.bottomRooms.Length, roomManager.bottomRooms);
            }
            else if (openingDirection == 2)
            {
                InstantiateRandomRoom(roomManager.topRooms.Length, roomManager.topRooms);
            }
            else if (openingDirection == 3)
            {
                InstantiateRandomRoom(roomManager.leftRooms.Length, roomManager.leftRooms);
            }
            else if (openingDirection == 4)
            {
                InstantiateRandomRoom(roomManager.rightRooms.Length, roomManager.rightRooms);
            }

            spawned = true;
        }
    }

    private void InstantiateRandomRoom(int length, GameObject[] room)
    {
        GameObject newRoom;
        //GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("RoomSpawnPoint");

        //int roomsLeft = roomManager.maxRooms - (roomManager.currentRooms.Count + spawnPoints.Length);
        if(roomManager.currentRooms.Count >= roomManager.maxRooms || length == 1)
        {
            newRoom = Instantiate(room[0], new Vector2(transform.position.x, transform.position.y), Quaternion.identity); //First room in list (index 0) must be dead end (T,B,R,L)
            newRoom.transform.parent = this.transform.parent.parent;
        }
        else if (roomManager.currentRooms.Count < roomManager.minRooms)
        {
            int random = Random.Range(1, length);
            newRoom = Instantiate(room[random], new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            newRoom.transform.parent = this.transform.parent.parent;
        }
        else
        {
            int random = Random.Range(0, length);
            newRoom = Instantiate(room[random], new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            newRoom.transform.parent = this.transform.parent.parent;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (roomManager == null)
        {
            roomManager = FindObjectOfType<RoomManager>();
        }
        if (collision.CompareTag("RoomSpawnPoint") || collision.CompareTag("Destroyer"))
        {
            if (collision.GetComponent<RoomSpawner>().spawned == false && spawned == false && transform.position.x != 0 && transform.position.y != 0)
            {
                InstantiateRandomRoom(roomManager.closedRooms.Length, roomManager.closedRooms);
                Destroy(gameObject);
            }

            spawned = true;
        }
    }
}
