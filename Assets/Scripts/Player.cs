 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    private Vector2 _inputVector;
    private bool _isWalking;
    private Vector3 _lastInteractionDir;

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
    [SerializeField] private LayerMask countersLayerMask;
    
    
    private void Awake()
    {
        _inputVector = new Vector2(0, 0);
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void HandleMovement()
    {
        _inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(_inputVector.x, 0f, _inputVector.y);
        _isWalking = moveDir != Vector3.zero;

        var ptransform = transform;
        Vector3 ppos = ptransform.position;
        var moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(ppos, ppos + Vector3.up * playerHeight,
            playerRadius, moveDir, out RaycastHit raycastHit, moveDistance);
        // if (!canMove)
        // {
        //     Debug.Log($"deb: moveDir {moveDir}, raycastHit {raycastHit.normal}");
        //     moveDir += raycastHit.normal;
        //     // moveDir += new Vector3(raycastHit.normal.x * .75f, 0, raycastHit.normal.z * 0.75f);
        //     Debug.Log($"deb: newDir  {moveDir}");
        //     ppos += moveDir * moveDistance;
        // }

        if (!canMove)
        {
            // Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0); //.normalized;
            canMove = !Physics.CapsuleCast(ppos, ppos + Vector3.up * playerHeight,
                playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                // Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z); //.normalized;
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
    
    private void HandleInteractions()
    {
        _inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(_inputVector.x, 0f, _inputVector.y);
        if (moveDir != Vector3.zero)
        {
            _lastInteractionDir = moveDir;
        }
        
        float interactionDistance = 2f;
        if (Physics.Raycast(transform.position, _lastInteractionDir, out RaycastHit raycastHit,
                interactionDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                // Has ClearCounter
                clearCounter.Interact();
            }
            
            
            
        }
    }
}
