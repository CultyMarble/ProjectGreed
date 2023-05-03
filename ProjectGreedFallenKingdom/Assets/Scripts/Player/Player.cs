using UnityEngine;

public enum PlayerActionState
{
    IsUsingBasicAbility,
    IsUsingRangeAbility,
    IsUsingAreaAbility,
    none,
}

public class Player : SingletonMonobehaviour<Player>
{
    [HideInInspector] public PlayerActionState playerActionState;
    [SerializeField] private Transform fpromtText;

    //======================================================================
    protected override void Awake()
    {
        Singleton();
    }

    private void Start()
    {
        // HideFPromtText();

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
        fpromtText.gameObject.SetActive(true);
    }

    public void HideFPromtText()
    {
        fpromtText.gameObject.SetActive(false);
    }
}