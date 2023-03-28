using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;
    
    private KitchenObject _kitchenObject;
    
    public void Interact(Player player)
    {
        if (_kitchenObject == null)
        {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint);
            _kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
            // KitchenObjectSO kso = _kitchenObject.GetKitchenObjectSO();
            // Debug.Log($"Spawn {kso.objectName}");
            _kitchenObject.SetKitchenObjectParent(this);
        }
        else // Give the object to the player
        {
            Debug.Log(_kitchenObject.GetKitchenObjectParent());
            _kitchenObject.SetKitchenObjectParent(player);
        }
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject ko)
    {
        _kitchenObject = ko;
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
