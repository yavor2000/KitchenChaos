using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

public class PlateKitchenObject : KitchenObject
{
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
            return true;
        }

    }
}
