using System;
using System.Collections;
using ScriptableObjects;
using UnityEngine;

namespace Counters
{
    public class StoveCounter : BaseCounter, IHasProgress
    {

        public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
        public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

        public class OnStateChangedEventArgs : EventArgs
        {
            public State state;
        }
        
        public enum State
        {
            Idle,
            Frying,
            Fried,
            Burned,
        }
        
        [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
        [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

        private float _fryingTimer;
        private float _burningTimer;

        private FryingRecipeSO _fryingRecipeSO;
        private BurningRecipeSO _burningRecipeSO;
        private State _state;

        public State CurrentState
        {
            get => _state;
            set
            {
                _state = value;
                SendStateChangedEvent();
            }
        }
        
        public float FryingTimer
        {
            get => _fryingTimer;
            set
            {
                _fryingTimer = value;
                SendFryingProgressChangedEvent();
            }
        }
        
        public float BurningTimer
        {
            get => _burningTimer;
            set
            {
                _burningTimer = value;
                SendBurningProgressChangedEvent();
            }
        }


        private void Start()
        {
            CurrentState = State.Idle;
            _fryingTimer = 0f;
            _burningTimer = 0f;
        }

        private void Update()
        {
            switch (_state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    FryingTimer += Time.deltaTime;
                    if (_fryingTimer > _fryingRecipeSO.fryingTimerMax)
                    {
                        // Fried
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(_fryingRecipeSO.output, this);
                        CurrentState = State.Fried;
                        FryingTimer = 0f;
                        _burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    }

                    break;
                case State.Fried:
                    BurningTimer += Time.deltaTime;
                    if (_burningTimer > _burningRecipeSO.burningTimerMax)
                    {
                        // Burned
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(_burningRecipeSO.output, this);
                        CurrentState = State.Burned;
                        BurningTimer = 0f;
                        FryingTimer = 0f;
                    }

                    break;
                case State.Burned:
                    break;
            }

            // Debug.Log(_state);
        }

        public override void Interact(Player player)
        {
            if (!HasKitchenObject())
            {
                // There is no KitchenObject here
                if (player.HasKitchenObject())
                {
                    // Player is carrying something
                    KitchenObjectSO koo = player.GetKitchenObject().GetKitchenObjectSO();
                    if (HasRecipeWithInput(koo)) {
                        player.GetKitchenObject().SetKitchenObjectParent(this);
                        _fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject()?.GetKitchenObjectSO());
                        // SendProgressEvent(_fryingProgress, cuttingRecipeSO);
                        CurrentState = State.Frying;
                        FryingTimer = 0f;
                    }
                }
                else
                {
                    // Player not carrying anything
                }
            }
            else
            {
                // There is a kitchen object here
                if (player.HasKitchenObject())
                {
                    // Player is carrying something
                }
                else
                {
                    // Player is not carrying anything - give him the kitchen object
                    GetKitchenObject().SetKitchenObjectParent(player);
                    CurrentState = State.Idle;
                    FryingTimer = 0f;
                    BurningTimer = 0f;
                }
            }
        }
        
        private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
        {
            return GetFryingRecipeSOWithInput(inputKitchenObjectSO) != null;
        }
    
        private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
        {
            FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
            if (fryingRecipeSO != null)
            {
                return fryingRecipeSO.output;
            }
            return null;
        }

        private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
        {
            foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
            {
                if (fryingRecipeSO.input == inputKitchenObjectSO)
                {
                    return fryingRecipeSO;
                }

            }
            return null;
        }
        
        private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
        {
            foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
            {
                if (burningRecipeSO.input == inputKitchenObjectSO)
                {
                    return burningRecipeSO;
                }

            }
            return null;
        }

        // private void Start()
        // {
        //     StartCoroutine(HandleFryTimer());
        // }
        //
        // private IEnumerable HandleFryTimer()
        // {
        //     yield return new WaitForSeconds(1f);
        // }

        private void SendStateChangedEvent()
        {
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
            {
                state = _state
            });
        }

        private void SendFryingProgressChangedEvent()
        {
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
            {
                progressNormalized = _fryingTimer / _fryingRecipeSO.fryingTimerMax
            });
        }
        
        private void SendBurningProgressChangedEvent()
        {
            if (OnProgressChanged != null)
            {
                OnProgressChanged.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
                {
                    progressNormalized = _burningTimer / _burningRecipeSO.burningTimerMax
                });
            }
        }
    }
}
