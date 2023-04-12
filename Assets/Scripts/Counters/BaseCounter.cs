using System;
using UnityEngine;

namespace Counters
{
    public class BaseCounter : MonoBehaviour, IKitchenObjectParent
    {
        public static event EventHandler OnAnyObjectPlacedHere;
        
        [SerializeField] private Transform counterTopPoint;
    
        private KitchenObject _kitchenObject;
    
        public virtual void Interact(Player player) {}

        public virtual void InteractAlternate(Player player) {}

        public Transform GetKitchenObjectFollowTransform()
        {
            return counterTopPoint;
        }

        public void SetKitchenObject(KitchenObject ko)
        {
            _kitchenObject = ko;
            if (_kitchenObject != null)
            {
                OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
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
    }
}
