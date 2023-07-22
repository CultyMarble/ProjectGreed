using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [Tooltip(" 1 --> Need Bottom Door\r\n 2 --> Need Top Door\r\n 3 --> Need Left Door\r\n 4 --> Need Right Door")]
    public int openingDirection;

    private RoomTemplates templates;
    private bool spawned = false;

    private void Start()
    {
        templates = FindObjectOfType<RoomTemplates>();
        Invoke("Spawn", 0.1F);
    }

    private void Spawn()
    {
        if (!spawned)
        {
            if (openingDirection == 1)
            {
                InstantiateRandomRoom(templates.bottomRooms.Length, templates.bottomRooms);
            }
            else if (openingDirection == 2)
            {
                InstantiateRandomRoom(templates.topRooms.Length, templates.topRooms);
            }
            else if (openingDirection == 3)
            {
                InstantiateRandomRoom(templates.leftRooms.Length, templates.leftRooms);
            }
            else if (openingDirection == 4)
            {
                InstantiateRandomRoom(templates.rightRooms.Length, templates.rightRooms);
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
            Destroy(gameObject);
        }
    }

}
