using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractTrigger : MonoBehaviour
{
    public event System.EventHandler OnPlayerInteractTrigger;

    //===========================================================================
    private PlayerInput playerInput;

    //===========================================================================
    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    private void Update()
    {
        if (playerInput.actions["Interact"].triggered)
            OnPlayerInteractTrigger?.Invoke(this, System.EventArgs.Empty);
    }
}