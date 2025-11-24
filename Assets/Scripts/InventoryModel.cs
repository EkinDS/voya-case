using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryModel", menuName = "ScriptableObjects/InventoryModel")]
public class InventoryModel : ScriptableObject
{
    public event Action<ResourceType, int> OnInventoryChanged;

    [SerializeField] private Dictionary<ResourceType, int> resourceCounts = new();

    private void OnEnable()
    {
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            if (type == ResourceType.None)
            {
                continue;
            }

            resourceCounts.TryAdd(type, 0);
        }
    }

    public int GetResourceCount(ResourceType type)
    {
        return resourceCounts.GetValueOrDefault(type, 0);
    }

    public void GainResource(ResourceType type, int count)
    {
        resourceCounts.TryAdd(type, 0);

        resourceCounts[type] += count;
        OnInventoryChanged?.Invoke(type, resourceCounts[type]);
    }

    public void SpendResource(ResourceType type, int count)
    {
        resourceCounts[type] -= count;
        OnInventoryChanged?.Invoke(type, resourceCounts[type]);
    }
}