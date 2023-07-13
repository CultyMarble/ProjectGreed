using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TEST : MonoBehaviour
{
    private GreedControls input = null;
    private Vector2 moveVector = Vector2.zero;

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

    private void FixedUpdate()
    {
        Debug.Log(moveVector);
    }

    private void Movement_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        moveVector = Vector2.zero;
    }

    private void Movement_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        moveVector = obj.ReadValue<Vector2>();
    }

}
