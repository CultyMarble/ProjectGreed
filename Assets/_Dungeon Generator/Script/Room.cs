using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject[] topRooms;
    public GameObject[] rightRooms;
    public GameObject[] bottomRooms;
    public GameObject[] leftRooms;

    public RoomController centreRoom;
    public RoomController topRoom;
    public RoomController rightRoom;
    public RoomController bottomRoom;
    public RoomController leftRoom;
    public RoomController leftRightRoom;
    public RoomController leftBottomRoom;
    public RoomController rightBottomRoom;
    public RoomController topBottomRoom;
    public RoomController topLeftRoom;
    public RoomController topRightRoom;
    public Direction spawnDirection;
    private GameObject activeRoom;
   
    private void OnEnable()
    {
    }

    public void SetActiveRoom(RoomShape roomShape)
    {
        //DisableAllRooms();
        activeRoom.SetActive(false);
        switch (roomShape)
        {
            case RoomShape.Centre:
                activeRoom = centreRoom.gameObject;
                break;
            case RoomShape.T:
                activeRoom = topRoom.gameObject;
                break;
            case RoomShape.R:
                activeRoom = rightRoom.gameObject;
                break;
            case RoomShape.B:
                activeRoom = bottomRoom.gameObject;
                break;
            case RoomShape.L:
                activeRoom = leftRoom.gameObject;
                break;
            case RoomShape.TR:
                activeRoom = topRightRoom.gameObject;
                break;
            case RoomShape.TB:
                activeRoom = topBottomRoom.gameObject;
                break;
            case RoomShape.TL:
                activeRoom = topLeftRoom.gameObject;
                break;
            case RoomShape.RB:
                activeRoom = rightBottomRoom.gameObject;
                break;
            case RoomShape.LR:
                activeRoom = leftRightRoom.gameObject;
                break;
            case RoomShape.LB:
                activeRoom = leftBottomRoom.gameObject;
                break;
        }
        activeRoom.SetActive(true);
    }
    public void SetActiveRoomRandom(Direction direction)
    {
        int random;
        //DisableAllRooms();
        activeRoom.SetActive(false);
        switch (direction)
        {
            case Direction.Top:
                random = Random.Range(0, 4);
                activeRoom = bottomRooms[random];
                break;
            case Direction.Right:
                random = Random.Range(0, 4);
                activeRoom = leftRooms[random];
                break;
            case Direction.Bottom:
                random = Random.Range(0, 4);
                activeRoom = topRooms[random];
                break;
            case Direction.Left:
                random = Random.Range(0, 4);
                activeRoom = rightRooms[random];
                break;
        }
        spawnDirection = direction;
        activeRoom.SetActive(true);
    }
    void DisableAllRooms()
    {
        centreRoom.gameObject.SetActive(false);
        topRoom.gameObject.SetActive(false);
        rightRoom.gameObject.SetActive(false);
        bottomRoom.gameObject.SetActive(false);
        leftRoom.gameObject.SetActive(false);
        leftRightRoom.gameObject.SetActive(false);
        rightBottomRoom.gameObject.SetActive(false);
        topBottomRoom.gameObject.SetActive(false);
        topLeftRoom.gameObject.SetActive(false);
        topRightRoom.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
