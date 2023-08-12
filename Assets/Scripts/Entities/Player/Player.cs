using UnityEngine;

public class Player : SingletonMonobehaviour<Player>
{
    [HideInInspector] public PlayerActionState actionState;

    [SerializeField] private PlayerData playerData = default;
    [SerializeField] private PlayerMovement playerMovement = default;

    public PlayerData PlayerData => playerData;
    public PlayerMovement PlayerMovement => playerMovement;

    [Header("Misc:")]
    [SerializeField] private Transform fpromtText;

    //======================================================================
    private void OnEnable()
    {
        EventManager.AfterSceneLoadEvent += EventManager_AfterSceneLoadEventHandler;
    }

    private void Update()
    {
        Debug.Log(actionState.ToString());
    }

    private void OnDisable()
    {
        EventManager.AfterSceneLoadEvent -= EventManager_AfterSceneLoadEventHandler;
    }

    //======================================================================
    private void EventManager_AfterSceneLoadEventHandler()
    {
        SetInteractPromtTextActive(false);

        actionState = PlayerActionState.none;

        transform.position = SceneControlManager.Instance.StartingPosition.position;
        gameObject.SetActive(true);
    }

    //======================================================================
    public void SetInteractPromtTextActive(bool newBool)
    {
        if (fpromtText == null)
            return;

        fpromtText.gameObject.SetActive(newBool);
    }
}