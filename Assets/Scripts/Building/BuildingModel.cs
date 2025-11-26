using System;
using UnityEngine;

public class BuildingModel
{
    public event Action<int> OnLevelChanged;
    public event Action<int> OnProduced;
    public event Action<bool> OnProcessingStateChanged;

    private BuildingConfig buildingConfig;
    private int level;
    private float progress;
    private float productionTimer;
    private bool isProcessing;
    private int remainingCycleCount;
    private int totalCycleCount;
    private float totalProgress;
    private float currentProductionDurationPerOutput;

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

    public void AddNewCycle()
    {
        remainingCycleCount += buildingConfig.GetCycleCount(level);
        totalCycleCount += buildingConfig.GetCycleCount(level);
        currentProductionDurationPerOutput = buildingConfig.GetProductionDurationPerOutput(level);
    }

    public void Upgrade()
    {
        level++;
        OnLevelChanged?.Invoke(level);
    }

    public void Tick(float deltaTime)
    {
        if (!isProcessing || totalCycleCount <= 0)
        {
            progress = 0f;
            totalProgress = 0f;
            return;
        }
        
        productionTimer += deltaTime;
        progress = Mathf.Clamp01(productionTimer / currentProductionDurationPerOutput);
        totalProgress = Mathf.Clamp01((totalCycleCount - remainingCycleCount + progress) / totalCycleCount);

        if (productionTimer < currentProductionDurationPerOutput)
        {
            return;
        }

        productionTimer -= currentProductionDurationPerOutput;
        progress = 0f;

        if (remainingCycleCount > 0)
        {
            remainingCycleCount--;
            RaiseProduced(1);
        }

        if (remainingCycleCount <= 0)
        {
            SetProcessingState(false);
            remainingCycleCount = 0;
            totalCycleCount = 0;
            totalProgress = 0f;
            progress = 0f;
            productionTimer = 0f;
        }
    }

    public float GetTotalProgress()
    {
        return totalProgress;
    }

    public string GetBuildingName()
    {
        return buildingConfig.GetBuildingName();
    }

    public ResourceType GetOutputResourceType()
    {
        return buildingConfig.outputResourceType;
    }

    public int GetLevel()
    {
        return level;
    }

    public int GetLevelCount()
    {
        return buildingConfig.levels.Count;
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