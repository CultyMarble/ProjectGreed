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
        Color color = Color.white;
        if (gateManager.locked && gateManager.keytype == PlayerCurrencies.KeyType.Silver)
        {
            color = Color.gray;
        }
        else if (gateManager.locked && gateManager.keytype == PlayerCurrencies.KeyType.Gold)
        {
            color = Color.blue;
        }
        else if(gateManager.room.activeRoom.currentRoomType == RoomType.boss)
        {
            color = Color.magenta;
        }
        else if (gateManager.room.activeRoom.currentRoomType == RoomType.shop || gateManager.room.activeRoom.currentRoomType == RoomType.abandonShop)
        {
            color = Color.green;
        }
        switch (doorSide)
        {
            case Direction.Top:
                MapGenerator.Instance.CreateRoomLayout(CreateDirection.Down, gateManager.room.activeRoom.roomShape,color);
                break;
            case Direction.Right:
                MapGenerator.Instance.CreateRoomLayout(CreateDirection.Left, gateManager.room.activeRoom.roomShape, color);
                break;
            case Direction.Bottom:
                MapGenerator.Instance.CreateRoomLayout(CreateDirection.Up, gateManager.room.activeRoom.roomShape, color);
                break;
            case Direction.Left:
                MapGenerator.Instance.CreateRoomLayout(CreateDirection.Right, gateManager.room.activeRoom.roomShape, color);
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
        if (!gateManager.activated)
        {
            gateManager.SetDoorsActive();
        }
        
        Collider2D[] otherDoors = Physics2D.OverlapCircleAll(transform.position, 1.0f,LayerMask.GetMask("DoorTrigger"));
        
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
                    if (!collider.transform.parent.GetComponentInParent<GateManager>().activated)
                    {
                        collider.transform.parent.GetComponentInParent<GateManager>().SetDoorsActive();
                    }
                    switch (doorSide)
                    {
                        case Direction.Top:
                            if(gateManager.topDoorState == DoorState.closed)
                            {
                                break;
                            }
                            if (collider.transform.parent.GetComponentInParent<GateManager>().bottomDoorState != DoorState.open)
                            {
                                gateManager.topDoorState = DoorState.closed;
                            }
                            else
                            {
                                gateManager.topDoorState = DoorState.open;
                                gateManager.openRooms++;
                            }
                            break;
                        case Direction.Right:
                            if (gateManager.rightDoorState == DoorState.closed)
                            {
                                break;
                            }
                            if (collider.transform.parent.GetComponentInParent<GateManager>().leftDoorState != DoorState.open)
                            {
                                gateManager.rightDoorState = DoorState.closed;
                            }
                            else
                            {
                                gateManager.rightDoorState = DoorState.open;
                                gateManager.openRooms++;
                            }
                            break;
                        case Direction.Bottom:
                            if (gateManager.bottomDoorState == DoorState.closed)
                            {
                                break;
                            }
                            if (collider.transform.parent.GetComponentInParent<GateManager>().topDoorState != DoorState.open)
                            {
                                gateManager.bottomDoorState = DoorState.closed;
                            }
                            else
                            {
                                gateManager.bottomDoorState = DoorState.open;
                                gateManager.openRooms++;
                            }
                            break;
                        case Direction.Left:
                            if (gateManager.leftDoorState == DoorState.closed)
                            {
                                break;
                            }
                            if (collider.transform.parent.GetComponentInParent<GateManager>().rightDoorState != DoorState.open)
                            {
                                gateManager.leftDoorState = DoorState.closed;
                            }
                            else
                            {
                                gateManager.leftDoorState = DoorState.open;
                                gateManager.openRooms++;
                            }
                            break;
                    }
                    doorChecked = true;
                    gateManager.doorsChecked++;
                    return;
                }
            }
        }

        switch (doorSide)
        {
            case Direction.Top:
                gateManager.topDoorState = DoorState.closed;
                break;
            case Direction.Right:
                gateManager.rightDoorState = DoorState.closed;
                break;
            case Direction.Bottom:
                gateManager.bottomDoorState = DoorState.closed;
                break;
            case Direction.Left:
                gateManager.leftDoorState = DoorState.closed;
                break;
        }
        doorChecked = true;
        gateManager.doorsChecked++;
    }

    private void OnDestroy()
    {
        RoomManager.onRoomsGenerated -= CheckForDoors;
    }
}
