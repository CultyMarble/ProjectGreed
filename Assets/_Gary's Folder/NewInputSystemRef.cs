using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewInputSystemRef : MonoBehaviour
{
    private GreedControls input = null;

    private void Awake()
    {
        input = new GreedControls();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Move.performed += Movement_performed;
        input.Player.Move.canceled += Movement_canceled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Move.performed -= Movement_performed;
        input.Player.Move.canceled -= Movement_canceled;
    }

    private void Movement_canceled(InputAction.CallbackContext obj)
    {

    }

    private void Movement_performed(InputAction.CallbackContext obj)
    {

    }

}
