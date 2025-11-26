using System;

public class InventoryPresenter : IDisposable
{
    private readonly InventoryModel inventoryModel;
    private readonly IInventoryView inventoryView;

    public InventoryPresenter(InventoryModel newInventoryModel, IInventoryView newInventoryView)
    {
        inventoryModel = newInventoryModel;
        inventoryView = newInventoryView;

        inventoryModel.OnInventoryChanged += HandleInventoryChanged;

        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            inventoryView.SetResourceAmount(resourceType, inventoryModel.GetResourceCount(resourceType));
        }
    }

    private void HandleInventoryChanged(ResourceType type, int count)
    {
        inventoryView.SetResourceAmount(type, count);
    }

    public void Dispose()
    {
        inventoryModel.OnInventoryChanged -= HandleInventoryChanged;
    }
}