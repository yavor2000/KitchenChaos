using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent _kitchenObjectParent;
    
    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if (_kitchenObjectParent != null)
        {
            _kitchenObjectParent.ClearKitchenObject();
        }
        _kitchenObjectParent = kitchenObjectParent;
        if (_kitchenObjectParent.HasKitchenObject()) {Debug.LogError("IKitchenObjectParent already has a KitchenObject!");}
        _kitchenObjectParent.SetKitchenObject(this);
        
        transform.parent = _kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return _kitchenObjectParent;
    }

    public void DestroySelf()
    {
        _kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO,
        IKitchenObjectParent kitchenObjectParent)
    {
        Transform kot = Instantiate(kitchenObjectSO.prefab);
        KitchenObject ko = kot.GetComponent<KitchenObject>();
        ko.SetKitchenObjectParent(kitchenObjectParent);
        
        return ko;
    }

    public bool TryGetPlate(out PlateKitchenObject pko)
    {
        if (this is PlateKitchenObject)
        {
            pko = this as PlateKitchenObject;
            return true;
        }
        else
        {
            pko = null;
            return false;
        }
    }
}
