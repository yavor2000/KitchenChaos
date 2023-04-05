using ScriptableObjects;
using UnityEngine;

namespace Counters
{
    public class ClearCounter : BaseCounter
    {
        [SerializeField] private KitchenObjectSO kitchenObjectSO;
    
        public override void Interact(Player player)
        {
            if (!HasKitchenObject())
            {
                // There is no KitchenObject here
                if (player.HasKitchenObject())
                {
                    // Player is carrying something
                    player.GetKitchenObject().SetKitchenObjectParent(this);
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
                    else
                    {
                        // Player is not carrying Plate but something else
                        if (GetKitchenObject().TryGetPlate(out pko))
                        {
                            // Counter has a Plate
                            if (pko.TryAddIngredient( player.GetKitchenObject().GetKitchenObjectSO()))
                            {
                                player.GetKitchenObject().DestroySelf();
                            }
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

    }
}
