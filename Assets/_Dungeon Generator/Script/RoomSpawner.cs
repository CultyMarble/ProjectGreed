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

    private void InstantiateRandomRoom(int length, GameObject[] room)
    {
        int random = Random.Range(0, length);
        Instantiate(room[random], new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("RoomSpawnPoint"))
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
