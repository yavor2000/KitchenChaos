using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour, IGameService
{
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    
    private Vector2 _inputVector;
    private PlayerInputActions _playerInputActions;
    private ServiceLocator _serviceLocator;

    public bool IsWalking { get; private set; }

    public Vector2 GetMovementVectorNormalized()
    {
        _inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        _inputVector = _inputVector.normalized;
        
        return _inputVector;
    }
    
    private void Awake()
    {
        _serviceLocator = ServiceLocator.Current;
        _serviceLocator.Register(this);
        
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Interact.performed += Interact_performed;
        _playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        _playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void Pause_performed(InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        _serviceLocator.Unregister<GameInput>();
        _playerInputActions.Player.Interact.performed -= Interact_performed;
        _playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        _playerInputActions.Player.Pause.performed -= Pause_performed;
        
        _playerInputActions.Dispose();
    }
}
