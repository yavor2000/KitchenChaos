using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    private Vector2 _inputVector;
    private PlayerInputActions _playerInputActions;

    public bool IsWalking { get; private set; }

    public Vector2 GetMovementVectorNormalized()
    {
        _inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        _inputVector = _inputVector.normalized;
        
        return _inputVector;
    }
    
    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Interact.performed += Interact_performed;
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }
}
