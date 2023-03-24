using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    private Vector2 _inputVector;
    private bool _isWalking;

    public bool IsWalking
    {
        get => _isWalking;
        private set => _isWalking = value;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float playerRadius = .65f;
    [SerializeField] private float playerHeight = 2f;

    [SerializeField] private GameInput gameInput;
    
    private void Awake()
    {
        _inputVector = new Vector2(0, 0);
    }

    private void Update()
    {
        _inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(_inputVector.x, 0f, _inputVector.y);
        _isWalking = moveDir != Vector3.zero;
        
        var ptransform = transform;
        Vector3 ppos = ptransform.position;
        var moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(ppos, ppos + Vector3.up * playerHeight,
            playerRadius, moveDir, moveDistance);
        if (!canMove)
        {
            // Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0); //.normalized();
            canMove = !Physics.CapsuleCast(ppos, ppos + Vector3.up * playerHeight,
                playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                // Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z); //.normalized();
                canMove = !Physics.CapsuleCast(ppos, ppos + Vector3.up * playerHeight,
                    playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    moveDir = moveDirZ;
                } // else can't move in any direction                
            }
        }
        if (canMove)
        {
            ptransform.position += moveDir * moveDistance;
        }
        
        ptransform.forward = Vector3.Slerp(ptransform.forward, moveDir, rotateSpeed * Time.deltaTime);
    }

}
