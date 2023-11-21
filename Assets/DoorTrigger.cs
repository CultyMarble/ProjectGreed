using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Centre,
    Top,
    Right,
    Bottom,
    Left,
}
public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private GateManager gateManager;
    [SerializeField] private Direction doorSide;

    private ToolTip toolTipMenu;
    private RoomController roomController;
    public bool doorChecked = false;
    private void Awake()
    {
        roomController = transform.parent.parent.parent.parent.GetComponent<RoomController>();
        RoomManager.onRoomsGenerated += CheckForDoors;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && gateManager.playerInLockZone == true)
        {
            switch (gateManager.keytype)
            {
                case PlayerCurrencies.KeyType.Silver:
                    if (PlayerCurrencies.Instance.hasSilverKey)
                    {
                        gateManager.disableGate = true;
                        gateManager.locked = false;
                        if(toolTipMenu != null)
                        {
                            toolTipMenu.ClearToolTip();
                        }
                        Player.Instance.SetInteractPromtTextActive(false);
                    }
                    else
                    {
                        ActivateDialogueManager("Hmm... I think I need to find a specific key to open this.");
                    }
                    break;
                case PlayerCurrencies.KeyType.Gold:
                    if (PlayerCurrencies.Instance.hasGoldKey)
                    {
                        gateManager.disableGate = true;
                        gateManager.locked = false;
                        if (toolTipMenu != null)
                        {
                            toolTipMenu.ClearToolTip();
                        }
                        Player.Instance.SetInteractPromtTextActive(false);
                    }
                    else
                    {
                        ActivateDialogueManager("Hmm... I think I need to find a specific key to open this.");
                    }
                    break;
            }
        }
    }
    private void ActivateDialogueManager(string text)
    {
        DialogManager.Instance.SetDialogLine(text);
        DialogManager.Instance.SetDialogPanelActiveState(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) 
        {
            return;
        }
        DrawRoomOnMap();
            
        if(gateManager.locked)
        {
            switch (gateManager.keytype)
            {
                case PlayerCurrencies.KeyType.Silver:
                    toolTipMenu = new ToolTip("Silver Locked Gate", "All rusted. I'll have to find some way to unlock this.");
                    toolTipMenu.SetToolTip();
                    break;
                case PlayerCurrencies.KeyType.Gold:
                    toolTipMenu = new ToolTip("Golden Locked Gate", "Quite magestic. Must be a key somewhere.");
                    toolTipMenu.SetToolTip();
                    break;
            }
            Player.Instance.SetInteractPromtTextActive(true);
            gateManager.playerInLockZone = true;
        }
    }

    private void DrawRoomOnMap()
    {
        if (gateManager.roomDrawn)
        {
            return;
        }
        switch (doorSide)
        {
            case Direction.Top:
                MapGenerator.Instance.CreateRoomLayout(CreateDirection.Down, roomController.roomShape);
                break;
            case Direction.Right:
                MapGenerator.Instance.CreateRoomLayout(CreateDirection.Left, roomController.roomShape);
                break;
            case Direction.Bottom:
                MapGenerator.Instance.CreateRoomLayout(CreateDirection.Up, roomController.roomShape);
                break;
            case Direction.Left:
                MapGenerator.Instance.CreateRoomLayout(CreateDirection.Right, roomController.roomShape);
                break;
        }
        gateManager.roomDrawn = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(toolTipMenu == null)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            toolTipMenu.ClearToolTip();
            Player.Instance.SetInteractPromtTextActive(false);
            gateManager.playerInLockZone = false;
        }
    }
    public void CheckForDoors()
    {
        if (doorChecked)
        {
            return;
        }

        Collider2D[] otherDoors = Physics2D.OverlapCircleAll(transform.position, 1.0f);

        if (otherDoors == null || otherDoors.Length == 1)
        {
            switch (roomController.roomShape)
            {
                case RoomShape.TR:
                    if(doorSide == Direction.Right)
                    {
                        roomController.rightDoorState = DoorState.closed;
                    }
                    else if (doorSide == Direction.Top)
                    {
                        roomController.topDoorState = DoorState.closed;
                    }
                    break;
                case RoomShape.TB:
                    if (doorSide == Direction.Bottom)
                    {
                        roomController.bottomDoorState = DoorState.closed;
                    }
                    else if (doorSide == Direction.Top)
                    {
                        roomController.topDoorState = DoorState.closed;
                    }
                    break;
                case RoomShape.TL:
                    if (doorSide == Direction.Left)
                    {
                        roomController.leftDoorState = DoorState.closed;
                    }
                    else if (doorSide == Direction.Top)
                    {
                        roomController.topDoorState = DoorState.closed;
                    }
                    break;
                case RoomShape.RB:
                    if (doorSide == Direction.Right)
                    {
                        roomController.rightDoorState = DoorState.closed;
                    }
                    else if (doorSide == Direction.Bottom)
                    {
                        roomController.bottomDoorState = DoorState.closed;
                    }
                    break;
                case RoomShape.LR:
                    if (doorSide == Direction.Right)
                    {
                        roomController.rightDoorState = DoorState.closed;
                    }
                    else if (doorSide == Direction.Left)
                    {
                        roomController.leftDoorState = DoorState.closed;
                    }
                    break;
                case RoomShape.LB:
                    if (doorSide == Direction.Left)
                    {
                        roomController.leftDoorState = DoorState.closed;
                    }
                    else if (doorSide == Direction.Bottom)
                    {
                        roomController.bottomDoorState = DoorState.closed;
                    }
                    break;
            }
            roomController.doorsChecked++;
            doorChecked = true;
            return;
        }
        else
        {
            foreach (Collider2D collider in otherDoors)
            {
                if(collider.gameObject == this.gameObject)
                {
                    continue;
                }
                if (collider.CompareTag("LockTrigger"))
                {
                    switch (roomController.roomShape)
                    {
                        case RoomShape.TR:
                            if (doorSide == Direction.Right && collider.GetComponent<DoorTrigger>().doorSide == Direction.Left)
                            {
                                collider.GetComponent<DoorTrigger>().roomController.leftDoorState = DoorState.open;
                                collider.GetComponent<DoorTrigger>().roomController.openRooms++;
                                collider.GetComponent<DoorTrigger>().roomController.doorsChecked++;
                                collider.GetComponent<DoorTrigger>().doorChecked = true;
                                roomController.rightDoorState = DoorState.open;
                                roomController.openRooms++;
                            }
                            else if (doorSide == Direction.Top && collider.GetComponent<DoorTrigger>().doorSide == Direction.Bottom)
                            {
                                collider.GetComponent<DoorTrigger>().roomController.bottomDoorState = DoorState.open;
                                collider.GetComponent<DoorTrigger>().roomController.openRooms++;
                                collider.GetComponent<DoorTrigger>().roomController.doorsChecked++;
                                collider.GetComponent<DoorTrigger>().doorChecked = true;
                                roomController.topDoorState = DoorState.open;
                                roomController.openRooms++;
                            }
                            break;
                        case RoomShape.TB:
                            if (doorSide == Direction.Bottom && collider.GetComponent<DoorTrigger>().doorSide == Direction.Top)
                            {
                                collider.GetComponent<DoorTrigger>().roomController.topDoorState = DoorState.open;
                                collider.GetComponent<DoorTrigger>().roomController.openRooms++;
                                collider.GetComponent<DoorTrigger>().roomController.doorsChecked++;
                                collider.GetComponent<DoorTrigger>().doorChecked = true;
                                roomController.bottomDoorState = DoorState.open;
                                roomController.openRooms++;
                            }
                            else if (doorSide == Direction.Top && collider.GetComponent<DoorTrigger>().doorSide == Direction.Bottom)
                            {
                                collider.GetComponent<DoorTrigger>().roomController.bottomDoorState = DoorState.open;
                                collider.GetComponent<DoorTrigger>().roomController.openRooms++;
                                collider.GetComponent<DoorTrigger>().roomController.doorsChecked++;
                                collider.GetComponent<DoorTrigger>().doorChecked = true;
                                roomController.topDoorState = DoorState.open;
                                roomController.openRooms++;
                            }
                            break;
                        case RoomShape.TL:
                            if (doorSide == Direction.Left && collider.GetComponent<DoorTrigger>().doorSide == Direction.Right)
                            {
                                collider.GetComponent<DoorTrigger>().roomController.rightDoorState = DoorState.open;
                                collider.GetComponent<DoorTrigger>().roomController.openRooms++;
                                collider.GetComponent<DoorTrigger>().roomController.doorsChecked++;
                                collider.GetComponent<DoorTrigger>().doorChecked = true;
                                roomController.leftDoorState = DoorState.open;
                                roomController.openRooms++;
                            }
                            else if (doorSide == Direction.Top && collider.GetComponent<DoorTrigger>().doorSide == Direction.Bottom)
                            {
                                collider.GetComponent<DoorTrigger>().roomController.bottomDoorState = DoorState.open;
                                collider.GetComponent<DoorTrigger>().roomController.openRooms++;
                                collider.GetComponent<DoorTrigger>().roomController.doorsChecked++;
                                collider.GetComponent<DoorTrigger>().doorChecked = true;
                                roomController.topDoorState = DoorState.open;
                                roomController.openRooms++;
                            }
                            break;
                        case RoomShape.RB:
                            if (doorSide == Direction.Right && collider.GetComponent<DoorTrigger>().doorSide == Direction.Left)
                            {
                                collider.GetComponent<DoorTrigger>().roomController.leftDoorState = DoorState.open;
                                collider.GetComponent<DoorTrigger>().roomController.openRooms++;
                                collider.GetComponent<DoorTrigger>().roomController.doorsChecked++;
                                collider.GetComponent<DoorTrigger>().doorChecked = true;
                                roomController.rightDoorState = DoorState.open;
                                roomController.openRooms++;
                            }
                            else if (doorSide == Direction.Bottom && collider.GetComponent<DoorTrigger>().doorSide == Direction.Top)
                            {
                                collider.GetComponent<DoorTrigger>().roomController.topDoorState = DoorState.open;
                                collider.GetComponent<DoorTrigger>().roomController.openRooms++;
                                collider.GetComponent<DoorTrigger>().roomController.doorsChecked++;
                                collider.GetComponent<DoorTrigger>().doorChecked = true;
                                roomController.bottomDoorState = DoorState.open;
                                roomController.openRooms++;
                            }
                            break;
                        case RoomShape.LR:
                            if (doorSide == Direction.Right && collider.GetComponent<DoorTrigger>().doorSide == Direction.Left)
                            {
                                collider.GetComponent<DoorTrigger>().roomController.leftDoorState = DoorState.open;
                                collider.GetComponent<DoorTrigger>().roomController.openRooms++;
                                collider.GetComponent<DoorTrigger>().roomController.doorsChecked++;
                                collider.GetComponent<DoorTrigger>().doorChecked = true;
                                roomController.rightDoorState = DoorState.open;
                                roomController.openRooms++;
                            }
                            else if (doorSide == Direction.Left && collider.GetComponent<DoorTrigger>().doorSide == Direction.Right)
                            {
                                collider.GetComponent<DoorTrigger>().roomController.rightDoorState = DoorState.open;
                                collider.GetComponent<DoorTrigger>().roomController.openRooms++;
                                collider.GetComponent<DoorTrigger>().roomController.doorsChecked++;
                                collider.GetComponent<DoorTrigger>().doorChecked = true;
                                roomController.leftDoorState = DoorState.open;
                                roomController.openRooms++;
                            }
                            break;
                        case RoomShape.LB:
                            if (doorSide == Direction.Left && collider.GetComponent<DoorTrigger>().doorSide == Direction.Right)
                            {
                                collider.GetComponent<DoorTrigger>().roomController.rightDoorState = DoorState.open;
                                collider.GetComponent<DoorTrigger>().roomController.openRooms++;
                                collider.GetComponent<DoorTrigger>().roomController.doorsChecked++;
                                collider.GetComponent<DoorTrigger>().doorChecked = true;
                                roomController.leftDoorState = DoorState.open;
                                roomController.openRooms++;
                            }
                            else if (doorSide == Direction.Bottom && collider.GetComponent<DoorTrigger>().doorSide == Direction.Top)
                            {
                                collider.GetComponent<DoorTrigger>().roomController.topDoorState = DoorState.open;
                                collider.GetComponent<DoorTrigger>().roomController.openRooms++;
                                collider.GetComponent<DoorTrigger>().roomController.doorsChecked++;
                                collider.GetComponent<DoorTrigger>().doorChecked = true;
                                roomController.bottomDoorState = DoorState.open;
                                roomController.openRooms++;
                            }
                            break;
                    }
                    
                }

            }
            roomController.doorsChecked++;
            doorChecked = true;
            return;
        }
        //if (active)
        //{
        //    foreach (Collider2D collider in otherDoors)
        //    {
        //        if (collider.CompareTag("DoorCheck"))
        //        {
        //            if (collider.gameObject.GetComponent<DoorCheck>().active)
        //            {
        //                DisableObstacle();
        //                isChecked = true;
        //                collider.gameObject.GetComponent<DoorCheck>().DisableObstacle();
        //                collider.gameObject.GetComponent<DoorCheck>().isChecked = true;
        //                return;
        //            }
        //        }

        //    }
        //}
        //else
        //{
        //    foreach (Collider2D collider in otherDoors)
        //    {
        //        if (collider.CompareTag("LockTrigger"))
        //        {
        //            Destroy(collider.gameObject);
        //        }

        //    }
        //}
        //EnableObstacle();
        //isChecked = true;
    }
    private void OnDisable()
    {
        RoomManager.onRoomsGenerated -= CheckForDoors;
    }
}
