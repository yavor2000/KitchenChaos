using System;
using ScriptableObjects;
using UnityEngine;

namespace Counters
{
    public class ContainerCounter : BaseCounter
    {
        public event EventHandler OnPlayerGrabbedObject;
    
        [SerializeField] private KitchenObjectSO kitchenObjectSO;
    
        public override void Interact(Player player)
        {
            if (!player.HasKitchenObject())
            {
                // Player is not carrying anything so give him the container kitchen object
                KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

                OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
            }
        }
    
    }
}
