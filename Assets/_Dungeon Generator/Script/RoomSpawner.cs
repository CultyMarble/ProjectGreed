using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    private RoomManager roomManager;

    [Tooltip(" 1 --> Need Bottom Door\r\n 2 --> Need Top Door\r\n 3 --> Need Left Door\r\n 4 --> Need Right Door")]
    [SerializeField] private int openingDirection;
    [SerializeField] public bool spawned = false;
    [SerializeField] public bool destroyer;

    private void Start()
    {
        roomManager = FindObjectOfType<RoomManager>();
        if (!destroyer)
        {
            Invoke("Spawn", 0.05F);
        }
        RoomManager.onRoomsGenerated += DeleteRoom;
    }
   
    private void Spawn()
    {
        if(roomManager.mapFinished)
        {
            return;
        }
        if (roomManager == null)
        {
            roomManager = FindObjectOfType<RoomManager>();
        }
        if(roomManager.currentRoomCount.Count > roomManager.maxRooms)
        {
            return;
        }
        if (!spawned)
        {
            if (openingDirection == 1)
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
    private void DeleteRoom()
    {
        if (!destroyer)
        {
            Destroy(gameObject);
        }
    }

    private void InstantiateRandomRoom(int length, GameObject[] room)
    {
        GameObject newRoom;
        
        if (roomManager.currentRoomCount.Count < roomManager.maxRooms)
        {
            int random = Random.Range(1, length);
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
        if (collision.CompareTag("RoomSpawnPoint"))
        {
            if (collision.GetComponent<RoomSpawner>().spawned == true && spawned == false && transform.position.x != 0 && transform.position.y != 0)
            {
                Destroy(gameObject);
            }
            else if (collision.GetComponent<RoomSpawner>().spawned == false && spawned == true && transform.position.x != 0 && transform.position.y != 0)
            {
                Destroy(collision.gameObject);
            }

            spawned = true;
        }
        if (collision.CompareTag("Destroyer"))
        {
            Destroy(gameObject);

            spawned = true;
        }
    }
    private void OnDestroy()
    {
        RoomManager.onRoomsGenerated -= DeleteRoom;
    }
}
