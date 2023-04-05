namespace Counters
{
public class DeliveryCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            KitchenObject ko = player.GetKitchenObject();
            if (ko.TryGetPlate(out PlateKitchenObject pko))
            {
                // Only accepts Plates
                ko.DestroySelf();
            }
        }
    }
}
}
