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

    public void Upgrade()
    {
        level++;

        OnLevelChanged?.Invoke(level);
    }

    public void Tick(float deltaTime)
    {
        if (!isProcessing)
        {
            progress = 0f;
            return;
        }

        float productionDuration = GetProductionDuration();
        if (productionDuration <= 0f)
        {
            progress = 0f;
            return;
        }

        productionTimer += deltaTime;
        progress = Mathf.Clamp01(productionTimer / productionDuration);

        if (productionTimer < productionDuration)
        {
            return;
        }

        productionTimer -= productionDuration;

        int outputCount = GetOutputCount();
        if (outputCount > 0)
        {
            RaiseProduced(outputCount);
        }

        SetProcessingState(false);
        progress = 0f;
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