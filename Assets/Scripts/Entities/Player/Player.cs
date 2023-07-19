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

    public PlayerActionState playerActionState;
    //======================================================================
    private void Start()
    {
        HideFPromtText();

        EventManager.AfterSceneLoadEvent += EventManager_AfterSceneLoadEventHandler;

        playerActionState = PlayerActionState.none;
    }

    //======================================================================
    private void EventManager_AfterSceneLoadEventHandler()
    {
        HideFPromtText();

        this.gameObject.SetActive(true);
    }

    //======================================================================
    public void ShowFPromtText()
    {
        if (fpromtText == null)
            return;

        fpromtText.gameObject.SetActive(true);
    }

    public void HideFPromtText()
    {
        if (fpromtText == null)
            return;

        fpromtText.gameObject.SetActive(false);
    }
}