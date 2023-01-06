using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicPlayerController : MonoBehaviour
{
    [InputPath] [SerializeField] private string _movementInput;
    [SerializeField] private int _playerNumber;
    [SerializeField] private float _speed = 5f;

    void Update()
    {
        InputAction.CallbackContext movementContext = InputManager.Instance.GetContext(_movementInput, _playerNumber);
        HandleMovement(movementContext);
    }

    private void HandleMovement(InputAction.CallbackContext movementContext)
    {
        Vector2 moveInput = movementContext.ReadValue<Vector2>();
        transform.position += (Vector3)moveInput * _speed * Time.deltaTime;
    }
}
