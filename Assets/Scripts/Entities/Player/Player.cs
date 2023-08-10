using UnityEngine;

public class Player : SingletonMonobehaviour<Player>
{
    [SerializeField] private Transform fpromtText;

    [HideInInspector] public PlayerActionState playerActionState;

    [SerializeField] private PlayerStat playerStat = default;
    public PlayerStat PlayerStat => playerStat;

    //======================================================================
    private void Start()
    {
        SetInteractPromtTextActive(false);

        EventManager.AfterSceneLoadEvent += EventManager_AfterSceneLoadEventHandler;

        playerActionState = PlayerActionState.none;
    }

    //======================================================================
    private void EventManager_AfterSceneLoadEventHandler()
    {
        SetInteractPromtTextActive(false);

        this.gameObject.SetActive(true);
    }

    //======================================================================
    public void SetInteractPromtTextActive(bool newBool)
    {
        if (fpromtText == null)
            return;

        fpromtText.gameObject.SetActive(newBool);
    }
}