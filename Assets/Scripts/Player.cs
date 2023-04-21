using System;
using System.Collections;
using System.Collections.Generic;
using Counters;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour, IGameService, IKitchenObjectParent
{
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public event EventHandler OnPickedSomething;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter SelectedCounter;
    }

    private KitchenGameManager _gameManager;
    private Vector2 _inputVector;
    private bool _isWalking;
    private Vector3 _lastInteractionDir;
    private BaseCounter _selectedCounter;
    private ServiceLocator _serviceLocator;
    private KitchenObject _kitchenObject;

    public bool IsWalking
    {
        get => _isWalking;
        private set => _isWalking = value;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float playerRadius = .7f;
    [SerializeField] private float playerHeight = 2f;

    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;


    private void Start()
    {
        _gameManager = _serviceLocator.Get<KitchenGameManager>();
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (!_gameManager.IsGamePlaying()) return;
        
        if (_selectedCounter != null)
        {
            _selectedCounter.Interact(this);
        }
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!_gameManager.IsGamePlaying()) return;
        
        if (_selectedCounter != null)
        {
            _selectedCounter.InteractAlternate(this);
        }
    }

    // override
    protected void Awake()
    {
        Debug.Log("Player Awake");
        _serviceLocator = ServiceLocator.Current;
        _serviceLocator.Register(this);
        
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
            canMove = (moveDir.x < -.5f || moveDir.x >+.5f) && !Physics.CapsuleCast(ppos, ppos + Vector3.up * playerHeight,
                playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                // Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z); //.normalized;
                canMove = (moveDir.z < -.5f || moveDir.z >+.5f) && !Physics.CapsuleCast(ppos, ppos + Vector3.up * playerHeight,
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
        _isWalking = moveDir != Vector3.zero;
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
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // BaseCounter bc = baseCounter;
                // Debug.Log($"bc {baseCounter.transform.position} | sel {(_selectedCounter != null ? _selectedCounter.transform.position : "null")}");
                // Has a counter
                if (baseCounter != _selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                if (_selectedCounter != null)
                {
                    SetSelectedCounter(null);
                }
            }
        }
        else
        {
            if (_selectedCounter != null)
            {
                SetSelectedCounter(null);
            }

        }
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        _selectedCounter = selectedCounter;
        // Debug.Log($"invoke OnSelectedCounterChanged with sel {selectedCounter}");
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            SelectedCounter = selectedCounter
        });
    }
    
    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject ko)
    {
        _kitchenObject = ko;
        if (_kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return _kitchenObject != null;
    }
    
    private void OnDestroy()
    {
        _serviceLocator.Unregister<Player>();
    }
}
