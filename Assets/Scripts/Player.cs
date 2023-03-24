using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 _inputVector;
    private bool _isWalking;

    public bool IsWalking
    {
        get => _isWalking;
        private set => _isWalking = value;
    }

    [SerializeField]
    private float moveSpeed = 7f;
    [SerializeField]
    private float rotateSpeed = 10f;

    [SerializeField] private GameInput _gameInput;
    
    private void Awake()
    {
        _inputVector = new Vector2(0, 0);
    }

    private void Update()
    {
        _inputVector = _gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(_inputVector.x, 0f, _inputVector.y);
        _isWalking = moveDir != Vector3.zero;
        
        var ptransform = transform;
        ptransform.position += moveDir * (moveSpeed * Time.deltaTime);
        ptransform.forward = Vector3.Slerp(ptransform.forward, moveDir, rotateSpeed * Time.deltaTime);
    }

}
