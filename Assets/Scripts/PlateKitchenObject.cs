using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;

    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO KitchenObjectSO;
    }
    
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectOSList;
    
    private List<KitchenObjectSO> _kitchenObjectSOList;

    private void Awake()
    {
        _kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO koso)
    {
        if (_kitchenObjectSOList.Contains(koso) || !validKitchenObjectOSList.Contains(koso))
        { // Already has that type
            return false;
        }
        else
        {
            _kitchenObjectSOList.Add(koso);
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs()
            {
                KitchenObjectSO = koso
            });
            return true;
        }

    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return _kitchenObjectSOList;
    }
}
