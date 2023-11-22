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
    [SerializeField] private bool active;


    private ToolTip toolTipMenu;
    public bool doorChecked = false;
    private void Awake()
    {
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
                MapGenerator.Instance.CreateRoomLayout(CreateDirection.Down, gateManager.room.activeRoom.roomShape);
                break;
            case Direction.Right:
                MapGenerator.Instance.CreateRoomLayout(CreateDirection.Left, gateManager.room.activeRoom.roomShape);
                break;
            case Direction.Bottom:
                MapGenerator.Instance.CreateRoomLayout(CreateDirection.Up, gateManager.room.activeRoom.roomShape);
                break;
            case Direction.Left:
                MapGenerator.Instance.CreateRoomLayout(CreateDirection.Right, gateManager.room.activeRoom.roomShape);
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

        if (otherDoors.Length > 1)
        {
            foreach (Collider2D collider in otherDoors)
            {
                if (collider.gameObject == this.gameObject)
                {
                    continue;
                }
                if (collider.CompareTag("LockTrigger"))
                {
                    switch (gateManager.room.activeRoom.roomShape)
                    {
                        case RoomShape.TR:
                            if (doorSide == Direction.Right && collider.GetComponent<DoorTrigger>().doorSide == Direction.Left)
                            {
                                gateManager.rightDoorState = DoorState.open;
                                gateManager.openRooms++;
                            }
                            else if (doorSide == Direction.Top && collider.GetComponent<DoorTrigger>().doorSide == Direction.Bottom)
                            {
                                gateManager.topDoorState = DoorState.open;
                                gateManager.openRooms++;
                            }
                            break;
                        case RoomShape.TB:
                            if (doorSide == Direction.Bottom && collider.GetComponent<DoorTrigger>().doorSide == Direction.Top)
                            {
                                gateManager.bottomDoorState = DoorState.open;
                                gateManager.openRooms++;
                            }
                            else if (doorSide == Direction.Top && collider.GetComponent<DoorTrigger>().doorSide == Direction.Bottom)
                            {
                                gateManager.topDoorState = DoorState.open;
                                gateManager.openRooms++;
                            }
                            break;
                        case RoomShape.TL:
                            if (doorSide == Direction.Left && collider.GetComponent<DoorTrigger>().doorSide == Direction.Right)
                            {
                                gateManager.leftDoorState = DoorState.open;
                                gateManager.openRooms++;
                            }
                            else if (doorSide == Direction.Top && collider.GetComponent<DoorTrigger>().doorSide == Direction.Bottom)
                            {
                                gateManager.topDoorState = DoorState.open;
                                gateManager.openRooms++;
                            }
                            break;
                        case RoomShape.RB:
                            if (doorSide == Direction.Right && collider.GetComponent<DoorTrigger>().doorSide == Direction.Left)
                            {
                                gateManager.rightDoorState = DoorState.open;
                                gateManager.openRooms++;
                            }
                            else if (doorSide == Direction.Bottom && collider.GetComponent<DoorTrigger>().doorSide == Direction.Top)
                            {
                                gateManager.bottomDoorState = DoorState.open;
                                gateManager.openRooms++;
                            }
                            break;
                        case RoomShape.LR:
                            if (doorSide == Direction.Right && collider.GetComponent<DoorTrigger>().doorSide == Direction.Left)
                            {
                                gateManager.rightDoorState = DoorState.open;
                                gateManager.openRooms++;
                            }
                            else if (doorSide == Direction.Left && collider.GetComponent<DoorTrigger>().doorSide == Direction.Right)
                            {
                                gateManager.leftDoorState = DoorState.open;
                                gateManager.openRooms++;
                            }
                            break;
                        case RoomShape.LB:
                            if (doorSide == Direction.Left && collider.GetComponent<DoorTrigger>().doorSide == Direction.Right)
                            {
                                gateManager.leftDoorState = DoorState.open;
                                gateManager.openRooms++;
                            }
                            else if (doorSide == Direction.Bottom && collider.GetComponent<DoorTrigger>().doorSide == Direction.Top)
                            {
                                gateManager.bottomDoorState = DoorState.open;
                                gateManager.openRooms++;
                            }
                            break;
                    }
                    gateManager.doorsChecked++;
                    doorChecked = true;
                    return;
                }
            }
        }
        else
        {
            switch (gateManager.room.activeRoom.roomShape)
            {
                case RoomShape.TR:
                    if (doorSide == Direction.Right)
                    {
                        gateManager.rightDoorState = DoorState.closed;
                    }
                    else if (doorSide == Direction.Top)
                    {
                        gateManager.topDoorState = DoorState.closed;
                    }
                    break;
                case RoomShape.TB:
                    if (doorSide == Direction.Bottom)
                    {
                        gateManager.bottomDoorState = DoorState.closed;
                    }
                    else if (doorSide == Direction.Top)
                    {
                        gateManager.topDoorState = DoorState.closed;
                    }
                    break;
                case RoomShape.TL:
                    if (doorSide == Direction.Left)
                    {
                        gateManager.leftDoorState = DoorState.closed;
                    }
                    else if (doorSide == Direction.Top)
                    {
                        gateManager.topDoorState = DoorState.closed;
                    }
                    break;
                case RoomShape.RB:
                    if (doorSide == Direction.Right)
                    {
                        gateManager.rightDoorState = DoorState.closed;
                    }
                    else if (doorSide == Direction.Bottom)
                    {
                        gateManager.bottomDoorState = DoorState.closed;
                    }
                    break;
                case RoomShape.LR:
                    if (doorSide == Direction.Right)
                    {
                        gateManager.rightDoorState = DoorState.closed;
                    }
                    else if (doorSide == Direction.Left)
                    {
                        gateManager.leftDoorState = DoorState.closed;
                    }
                    break;
                case RoomShape.LB:
                    if (doorSide == Direction.Left)
                    {
                        gateManager.leftDoorState = DoorState.closed;
                    }
                    else if (doorSide == Direction.Bottom)
                    {
                        gateManager.bottomDoorState = DoorState.closed;
                    }
                    break;
            }
        }
        gateManager.doorsChecked++;
        doorChecked = true;
        return;
    }
    private void OnDestroy()
    {
        RoomManager.onRoomsGenerated -= CheckForDoors;
    }
}
