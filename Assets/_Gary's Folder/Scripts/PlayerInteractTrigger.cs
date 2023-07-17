using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractTrigger : MonoBehaviour
{
    //===========================================================================
    // NEW INPUT SYSTEM

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    //===========================================================================

    private void Update()
    {
        InteractInputHandler();
    }

    private void InteractInputHandler()
    {
        if (playerInput.actions["Interact"].triggered)
        {
            Debug.Log("Interact!!!");
        }
    }

}
