using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
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
    }
}
