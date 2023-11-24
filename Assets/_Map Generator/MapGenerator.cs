using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public enum RoomShape
{
    Centre,
    T,
    R,
    B,
    L,
    TR,
    TB,
    TL,
    RB,
    LR,
    LB,
    TRB,
    TRL,
    TBL,
    RBL,
}

public enum CreateDirection
{
    Up, Down, Left, Right
}

public class MapGenerator : SingletonMonobehaviour<MapGenerator>
{
    [SerializeField] private Transform parent = default;
    [SerializeField] private RectTransform positionMarker = default;

    [Header("Room Shape:")]
    [SerializeField] private Transform centerFourWay = default;

    [SerializeField] private Transform lineOneWay1 = default;
    [SerializeField] private Transform lineOneWay2 = default;
    [SerializeField] private Transform lineOneWay3 = default;
    [SerializeField] private Transform lineOneWay4 = default;

    [SerializeField] private Transform lShapeTwoWay12 = default;
    [SerializeField] private Transform lShapeTwoWay13 = default;
    [SerializeField] private Transform lShapeTwoWay14 = default;
    [SerializeField] private Transform lShapeTwoWay23 = default;
    [SerializeField] private Transform lShapeTwoWay24 = default;
    [SerializeField] private Transform lShapeTwoWay34 = default;

    [SerializeField] private Transform tShapeThreeWay123 = default;
    [SerializeField] private Transform tShapeThreeWay124 = default;
    [SerializeField] private Transform tShapeThreeWay134 = default;
    [SerializeField] private Transform tShapeThreeWay234 = default;


    // Map Offset
    private readonly float offsetDistance = 36.0f;

    //===========================================================================
    private void Update()
    {
        if (SceneControlManager.Instance.CurrentActiveScene != SceneName.DemoSceneDungeon)
            return;

        if (SceneControlManager.Instance.IsLoadingScreenActive == true)
            return;

        if (Input.GetKeyDown(KeyCode.Tab))
            MapGUI.Instance.ToggleMapUI();

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            parent.transform.localScale *= 0.5f;
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            parent.transform.localScale *= 1.5f;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
            parent.transform.position = new Vector3(parent.transform.position.x, parent.transform.position.y + offsetDistance);
            if (Input.GetKeyDown(KeyCode.DownArrow))
            parent.transform.position = new Vector3(parent.transform.position.x, parent.transform.position.y - offsetDistance);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            parent.transform.position = new Vector3(parent.transform.position.x - offsetDistance, parent.transform.position.y);

        if (Input.GetKeyDown(KeyCode.RightArrow))
            parent.transform.position = new Vector3(parent.transform.position.x + offsetDistance, parent.transform.position.y);

    }

    //===========================================================================
    public void MovePositionMarker(CreateDirection direction)
    {
        switch (direction)
        {
            case CreateDirection.Up:
                positionMarker.localPosition = new Vector2(positionMarker.localPosition.x, positionMarker.localPosition.y + offsetDistance);
                break;
            case CreateDirection.Down:
                positionMarker.localPosition = new Vector2(positionMarker.localPosition.x, positionMarker.localPosition.y - offsetDistance);
                break;
            case CreateDirection.Left:
                positionMarker.localPosition = new Vector2(positionMarker.localPosition.x - offsetDistance, positionMarker.localPosition.y);
                break;
            case CreateDirection.Right:
                positionMarker.localPosition = new Vector2(positionMarker.localPosition.x + offsetDistance, positionMarker.localPosition.y);
                break;
            default:
                break;
        }
    }

    private void CreateRoom(RoomShape shape,Color color)
    {
        Transform newRoom;
        switch (shape)
        {
            case RoomShape.Centre:
                newRoom = Instantiate(centerFourWay, parent);
                newRoom.GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                newRoom.GetComponent<Image>().color = color;
                break;
            case RoomShape.T:
                newRoom = Instantiate(lineOneWay1, parent);
                newRoom.GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                newRoom.GetComponent<Image>().color = color;
                break;
            case RoomShape.R:
                newRoom = Instantiate(lineOneWay2, parent);
                newRoom.GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                newRoom.GetComponent<Image>().color = color;
                break;
            case RoomShape.B:
                newRoom = Instantiate(lineOneWay3, parent);
                newRoom.GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                newRoom.GetComponent<Image>().color = color;
                break;
            case RoomShape.L:
                newRoom = Instantiate(lineOneWay4, parent);
                newRoom.GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                newRoom.GetComponent<Image>().color = color;
                break;
            case RoomShape.TR:
                newRoom = Instantiate(lShapeTwoWay12, parent);
                newRoom.GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                newRoom.GetComponent<Image>().color = color;
                break;
            case RoomShape.TB:
                newRoom = Instantiate(lShapeTwoWay13, parent);
                newRoom.GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                newRoom.GetComponent<Image>().color = color;
                break;
            case RoomShape.TL:
                newRoom = Instantiate(lShapeTwoWay14, parent);
                newRoom.GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                newRoom.GetComponent<Image>().color = color;
                break;
            case RoomShape.RB:
                newRoom = Instantiate(lShapeTwoWay23, parent);
                newRoom.GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                newRoom.GetComponent<Image>().color = color;
                break;
            case RoomShape.LR:
                newRoom = Instantiate(lShapeTwoWay24, parent);
                newRoom.GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                newRoom.GetComponent<Image>().color = color;
                break; 
            case RoomShape.LB:
                newRoom = Instantiate(lShapeTwoWay34, parent);
                newRoom.GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                newRoom.GetComponent<Image>().color = color;
                break;
            case RoomShape.TRB:
                newRoom = Instantiate(tShapeThreeWay123, parent);
                newRoom.GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                newRoom.GetComponent<Image>().color = color;
                break;
            case RoomShape.TRL:
                newRoom = Instantiate(tShapeThreeWay124, parent);
                newRoom.GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                newRoom.GetComponent<Image>().color = color;
                break;
            case RoomShape.TBL:
                newRoom = Instantiate(tShapeThreeWay134, parent);
                newRoom.GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                newRoom.GetComponent<Image>().color = color;
                break;
            case RoomShape.RBL:
                newRoom = Instantiate(tShapeThreeWay234, parent);
                newRoom.GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                newRoom.GetComponent<Image>().color = color;
                break;
            default:
                break;
        }
        
    }

    //===========================================================================
    public void CreateRoomLayout(CreateDirection direction, RoomShape shape, Color color)
    {
        switch (direction)
        {
            case CreateDirection.Up:
                MovePositionMarker(CreateDirection.Up);
                CreateRoom(shape, color);
                MovePositionMarker(CreateDirection.Down);
                break;
            case CreateDirection.Down:
                MovePositionMarker(CreateDirection.Down);
                CreateRoom(shape, color);
                MovePositionMarker(CreateDirection.Up);
                break;
            case CreateDirection.Left:
                MovePositionMarker(CreateDirection.Left);
                CreateRoom(shape, color);
                MovePositionMarker(CreateDirection.Right);
                break;
            case CreateDirection.Right:
                MovePositionMarker(CreateDirection.Right);
                CreateRoom(shape, color);
                MovePositionMarker(CreateDirection.Left);
                break;
            default:
                break;
        }
    }
}