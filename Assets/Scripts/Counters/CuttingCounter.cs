using System;
using ScriptableObjects;
using UnityEngine;

namespace Counters
{
    public class CuttingCounter : BaseCounter, IHasProgress
    {
        public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    
        public event EventHandler OnCut;
    
        [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

        private int _cuttingProgress;
    
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
                        _cuttingProgress = 0;
                        CuttingRecipeSO cuttingRecipeSO =
                            GetCuttingRecipeSOWithInput(GetKitchenObject()?.GetKitchenObjectSO());
                        SendProgressEvent(_cuttingProgress, cuttingRecipeSO);
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
                    if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject pko))
                    {
                        if (pko.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                        {
                            GetKitchenObject().DestroySelf();
                        }
                    }
                }
                else
                {
                    // Player is not carrying anything - give him the kitchen object
                    GetKitchenObject().SetKitchenObjectParent(player);
                }
            }
        }

        public override void InteractAlternate(Player player)
        {
            KitchenObjectSO inputKitchenObjectSO = GetKitchenObject()?.GetKitchenObjectSO();
            if (HasKitchenObject() && HasRecipeWithInput(inputKitchenObjectSO))
            {
                // If there is a kitchen object and it can be sliced
                // Get the recipe output kitchen object based on the current one on the counter
                _cuttingProgress++;
                CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
                SendProgressEvent(_cuttingProgress, cuttingRecipeSO);
                OnCut?.Invoke(this, EventArgs.Empty);

                if (_cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
                {
                    KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(inputKitchenObjectSO);
                    // remove current kitchen object
                    GetKitchenObject().DestroySelf();
                    // Spawn sliced kitchen object
                    if (outputKitchenObjectSO != null)
                    {
                        KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
                    }
                    else
                    {
                        Debug.LogError("Output kitchen object SO is null!");
                    }
                }
            }
        }

        private void SendProgressEvent(int cuttingProgress, CuttingRecipeSO cuttingRecipeSO)
        {
            float cuttingProgressMax = cuttingRecipeSO != null ? cuttingRecipeSO.cuttingProgressMax : 0f;
            if (cuttingProgressMax > 0f)
            {
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
                {
                    progressNormalized = cuttingProgress / cuttingProgressMax,
                });
            }
        }

        private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
        {
            return GetCuttingRecipeSOWithInput(inputKitchenObjectSO) != null;
        }
    
        private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
        {
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
            if (cuttingRecipeSO != null)
            {
                return cuttingRecipeSO.output;
            }
            return null;
        }

        private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
        {
            foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
            {
                if (cuttingRecipeSO.input == inputKitchenObjectSO)
                {
                    return cuttingRecipeSO;
                }

            }
            return null;
        }
    }
}
