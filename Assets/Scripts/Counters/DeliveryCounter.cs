using System;

namespace Counters
{
public class DeliveryCounter : BaseCounter
{
    private ServiceLocator _serviceLocator;
    private DeliveryManager _deliveryManager;

    private void Awake()
    {
        _serviceLocator = ServiceLocator.Current;
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
}
}
