using System;
using UnityEngine;

public class BuildingModel
{
    public event Action<int> OnLevelChanged;
    public event Action<int> OnProduced;
    public event Action<bool> OnProcessingStateChanged;

    public BuildingConfig buildingConfig;
    public int level;
    public float progress;
    private float productionTimer;
    private bool isProcessing;


    public BuildingModel(BuildingConfig newBuildingConfig)
    {
        buildingConfig = newBuildingConfig;
    }

    public float GetProductionDuration()
    {
        return buildingConfig.GetProductionDuration(level);
    }

    public int GetOutputCount()
    {
        return buildingConfig.GetOutputCount(level);
    }

    public int GetRequiredInputCount()
    {
        return buildingConfig.GetRequiredInputCount(level);
    }

    public UpgradeRequirement GetUpgradeRequirement()
    {
        return buildingConfig.GetUpgradeRequirement(level);
    }

    public void Upgrade(InventoryModel inventory)
    {
        level++;

        OnLevelChanged?.Invoke(level);
    }

    public void Tick(float deltaTime, InventoryModel inventory)
    {
        float productionDuration = GetProductionDuration();

        if (isProcessing)
        {
            productionTimer += deltaTime;
            progress = Mathf.Clamp01(productionTimer / productionDuration);

            if (productionTimer >= productionDuration)
            {
                productionTimer -= productionDuration;

                int outputCount = GetOutputCount();
                ResourceType outputResourceType = buildingConfig.outputResourceType;

                inventory.GainResource(outputResourceType, outputCount);
                RaiseProduced(outputCount);
             
                SetProcessingState(false);
                progress = 0F;
            }
        }
    }

    public void SetProcessingState(bool newState)
    {
        if (isProcessing == newState)
        {
            return;
        }

        isProcessing = newState;
        OnProcessingStateChanged?.Invoke(isProcessing);
    }

    public bool GetProcessingState()
    {
        return isProcessing;
    }

    public ResourceType GetRequiredInputResourceType()
    {
        return buildingConfig.requiredInputResourceType;
    }

    private void RaiseProduced(int count)
    {
        OnProduced?.Invoke(count);
    }
}