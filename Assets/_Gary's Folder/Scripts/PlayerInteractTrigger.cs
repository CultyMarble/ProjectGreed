using UnityEngine;
using UnityEngine.InputSystem;

public enum KeyAction
{
    Dash,
    Interact,
    LeftClick,
    RightClick,
    Bomb,
}

public class PlayerInteractTrigger : SingletonMonobehaviour<PlayerInteractTrigger>
{
    public event System.EventHandler OnPlayerInteractTrigger;

    //===========================================================================
    private PlayerInput playerInput;

    //===========================================================================
    protected override void Awake()
    {
        base.Awake();
        playerInput = FindObjectOfType<PlayerInput>();
    }

    private void Update()
    {
        if (playerInput.actions["Interact"].triggered)
            OnPlayerInteractTrigger?.Invoke(this, System.EventArgs.Empty);
    }

    //===========================================================================
    public string GetInteractKey(KeyAction key)
    {
        string name = playerInput.actions[key.ToString()].GetBindingDisplayString(0);
        return name;
    }
}