using UnityEngine;
using UnityEngine.InputSystem;

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
    public string GetInteractKey()
    {
        if (playerInput == null)
            return string.Empty;

        return playerInput.actions["Interact"].GetBindingDisplayString(0);
    }
}