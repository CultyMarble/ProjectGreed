using UnityEngine;
using TMPro;
public class Player : SingletonMonobehaviour<Player>
{
    [HideInInspector] public PlayerActionState actionState;

    [SerializeField] private PlayerDataManager playerData = default;
    [SerializeField] private PlayerMovement playerMovement = default;

    public PlayerDataManager PlayerData => playerData;
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
        //if (Input.GetKeyDown(KeyCode.End))
        //    GetComponent<PlayerHeart>().UpdateCurrentHeart(-1);
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

        gameObject.SetActive(true);
    }

    //======================================================================
    public void SetInteractPromtTextActive(bool newBool)
    {
        if (fpromtText == null)
            return;

        fpromtText.gameObject.GetComponent<TextMeshPro>().text = "[" + PlayerInteractTrigger.Instance.GetInteractKey(KeyAction.Interact) + "]";
        fpromtText.gameObject.SetActive(newBool);
    }
}