using UnityEngine;

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
            CreateRoomLayout(CreateDirection.Up, RoomShape.Centre);

        if (Input.GetKeyDown(KeyCode.DownArrow))
            CreateRoomLayout(CreateDirection.Down, RoomShape.Centre);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            CreateRoomLayout(CreateDirection.Left, RoomShape.Centre);

        if (Input.GetKeyDown(KeyCode.RightArrow))
            CreateRoomLayout(CreateDirection.Right, RoomShape.Centre);
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
            case RoomShape.Centre:
                Instantiate(centerFourWay, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.T:
                Instantiate(lineOneWay1, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.R:
                Instantiate(lineOneWay2, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.B:
                Instantiate(lineOneWay3, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.L:
                Instantiate(lineOneWay4, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.TR:
                Instantiate(lShapeTwoWay12, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.TB:
                Instantiate(lShapeTwoWay13, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.TL:
                Instantiate(lShapeTwoWay14, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.RB:
                Instantiate(lShapeTwoWay23, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.LR:
                Instantiate(lShapeTwoWay24, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.LB:
                Instantiate(lShapeTwoWay34, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.TRB:
                Instantiate(tShapeThreeWay123, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.TRL:
                Instantiate(tShapeThreeWay124, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.TBL:
                Instantiate(tShapeThreeWay134, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            case RoomShape.RBL:
                Instantiate(tShapeThreeWay234, parent).GetComponent<RectTransform>().localPosition = positionMarker.localPosition;
                break;
            default:
                break;
        }
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