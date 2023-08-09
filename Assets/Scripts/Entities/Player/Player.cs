using UnityEngine;

public enum PlayerActionState
{
    IsUsingBasicAbility,
    IsUsingRangeAbility,
    IsUsingAreaAbility,
    IsUsingBombAbility,
    IsDashing,
    none,
}

public class Player : SingletonMonobehaviour<Player>
{
    [SerializeField] private Transform fpromtText;

    [HideInInspector] public PlayerActionState playerActionState;
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