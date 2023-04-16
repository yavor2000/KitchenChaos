using System;
using UnityEngine;

namespace Counters
{
public class DeliveryCounter : BaseCounter, IGameService
{
    private ServiceLocator _serviceLocator;
    private DeliveryManager _deliveryManager;

    private void Awake()
    {
        Debug.Log("DeliveryCounter awake");
        _serviceLocator = ServiceLocator.Current;
        _serviceLocator.Register(this);
    }

    private void Start()
    {
        Debug.Log("DeliveryCounter start");
        _deliveryManager = _serviceLocator.Get<DeliveryManager>();
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            KitchenObject ko = player.GetKitchenObject();
            if (ko.TryGetPlate(out PlateKitchenObject pko))
            {
                // Only accepts Plates
                // _deliveryManager = _serviceLocator.Get<DeliveryManager>();
                _deliveryManager.DeliverRecipe(pko);
                ko.DestroySelf();
            }
        }
    }
    
    private void OnDestroy()
    {
        _serviceLocator.Unregister<DeliveryCounter>();
    }
}
}
