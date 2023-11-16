using UnityEngine;

public enum RoomShape
{
    CenterFourWay,
    LineOneWay1,
    LineOneWay2,
    LineOneWay3,
    LineOneWay4,
    LShapeTwoWay12,
    LShapeTwoWay13,
    LShapeTwoWay14,
    LShapeTwoWay23,
    LShapeTwoWay24,
    LShapeTwoWay34,
    TShapeThreeWay123,
    TShapeThreeWay124,
    TShapeThreeWay134,
    TShapeThreeWay234,
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
    private readonly float offsetDistance = 12.0f;

    //===========================================================================
    private void Update()
    {
        if (SceneControlManager.Instance.CurrentActiveScene != SceneName.DemoSceneDungeon)
            return;

        if (SceneControlManager.Instance.IsLoadingScreenActive == true)
            return;

        if (Input.GetKeyDown(KeyCode.Tab))
            MapGUI.Instance.ToggleMapUI();

        // TODO DELETE
        if (Input.GetKeyDown(KeyCode.UpArrow))
            CreateRoomLayout(CreateDirection.Up, RoomShape.CenterFourWay);

        if (Input.GetKeyDown(KeyCode.DownArrow))
            CreateRoomLayout(CreateDirection.Down, RoomShape.CenterFourWay);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            CreateRoomLayout(CreateDirection.Left, RoomShape.CenterFourWay);

        if (Input.GetKeyDown(KeyCode.RightArrow))
            CreateRoomLayout(CreateDirection.Right, RoomShape.CenterFourWay);
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

    private void CreateRoom(RoomShape shape)
    {
        switch (shape)
        {
            case RoomShape.CenterFourWay:
                Instantiate(centerFourWay, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.LineOneWay1:
                Instantiate(lineOneWay1, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.LineOneWay2:
                Instantiate(lineOneWay2, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.LineOneWay3:
                Instantiate(lineOneWay3, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.LineOneWay4:
                Instantiate(lineOneWay4, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.LShapeTwoWay12:
                Instantiate(lShapeTwoWay12, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.LShapeTwoWay13:
                Instantiate(lShapeTwoWay13, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.LShapeTwoWay14:
                Instantiate(lShapeTwoWay14, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.LShapeTwoWay23:
                Instantiate(lShapeTwoWay23, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.LShapeTwoWay24:
                Instantiate(lShapeTwoWay24, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.LShapeTwoWay34:
                Instantiate(lShapeTwoWay34, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.TShapeThreeWay123:
                Instantiate(tShapeThreeWay123, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.TShapeThreeWay124:
                Instantiate(tShapeThreeWay124, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.TShapeThreeWay134:
                Instantiate(tShapeThreeWay134, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.TShapeThreeWay234:
                Instantiate(tShapeThreeWay234, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            default:
                break;
        }

        //
    }

    //===========================================================================
    public void CreateRoomLayout(CreateDirection direction, RoomShape shape)
    {
        switch (direction)
        {
            case CreateDirection.Up:
                MovePositionMarker(CreateDirection.Up);
                CreateRoom(shape);
                MovePositionMarker(CreateDirection.Down);
                break;
            case CreateDirection.Down:
                MovePositionMarker(CreateDirection.Down);
                CreateRoom(shape);
                MovePositionMarker(CreateDirection.Up);
                break;
            case CreateDirection.Left:
                MovePositionMarker(CreateDirection.Left);
                CreateRoom(shape);
                MovePositionMarker(CreateDirection.Right);
                break;
            case CreateDirection.Right:
                MovePositionMarker(CreateDirection.Right);
                CreateRoom(shape);
                MovePositionMarker(CreateDirection.Left);
                break;
            default:
                break;
        }
    }
}