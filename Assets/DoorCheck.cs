using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCheck : MonoBehaviour
{
    public bool isChecked = false;
    public bool active = false;
    public Transform doorCheck;

    void OnEnable()
    {
        if (!active)
        {
            EnableObstacle();
        }
        RoomManager.onRoomsGenerated += CheckForDoors;
    }
    void OnDisable()
    {
        RoomManager.onRoomsGenerated -= CheckForDoors;
    }
    public void CheckForDoors()
    {
        if (isChecked)
        {
            return;
        }
        
        Collider2D[] otherDoors = Physics2D.OverlapCircleAll(doorCheck.position, 1.0f);

        if (otherDoors == null)
        {
            EnableObstacle();
            isChecked = true;
            return;
        }
        if (active)
        {
            foreach (Collider2D collider in otherDoors)
            {
                if (collider.CompareTag("DoorCheck"))
                {
                    if (collider.gameObject.GetComponent<DoorCheck>().active)
                    {
                        DisableObstacle();
                        isChecked = true;
                        collider.gameObject.GetComponent<DoorCheck>().DisableObstacle();
                        collider.gameObject.GetComponent<DoorCheck>().isChecked = true;
                        return;
                    }
                }

            }
        }
        else
        {
            foreach (Collider2D collider in otherDoors)
            {
                if (collider.CompareTag("LockTrigger"))
                {
                    Destroy(collider.gameObject);
                }

            }
        }
        EnableObstacle();
        isChecked = true;
    }
    public void EnableObstacle()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
    public void DisableObstacle()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
