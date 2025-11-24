using System;
using System.Collections.Generic;

public class InventoryModel
{
    public event Action<ResourceType, int> OnInventoryChanged;

    private readonly Dictionary<ResourceType, int> resourceCounts = new();

    public InventoryModel()
    {
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            if (type == ResourceType.None) continue;
            resourceCounts[type] = 0;
        }
    }

    public int GetResourceCount(ResourceType type)
    {
        return resourceCounts.GetValueOrDefault(type, 0);
    }

    public void GainResource(ResourceType type, int count)
    {
        resourceCounts[type] += count;
        OnInventoryChanged?.Invoke(type, resourceCounts[type]);
    }

    public void SpendResource(ResourceType type, int count)
    {
        resourceCounts[type] -= count;
        OnInventoryChanged?.Invoke(type, resourceCounts[type]);
    }
}