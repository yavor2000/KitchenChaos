using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private ClearCounter _clearCounter;
    
    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetClearCounter(ClearCounter cc)
    {
        if (_clearCounter != null)
        {
            _clearCounter.ClearKitchenObject();
        }
        _clearCounter = cc;
        if (_clearCounter.HasKitchenObject()) {Debug.LogError("Counter already has a KitchenObject!");}
        _clearCounter.SetKitchenObject(this);
        
        transform.parent = _clearCounter.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public ClearCounter GetClearCounter()
    {
        return _clearCounter;
    }
}
