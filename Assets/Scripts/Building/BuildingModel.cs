using System;

public abstract class BuildingModel
{
    public event Action<int> OnLevelChanged;
    public event Action<int> OnProduced;
    public event Action<bool> OnProcessingStateChanged;

    public BuildingConfig buildingConfig;
    public int level;
    public float progress;
    protected float productionTimer;
    protected bool isProcessing;


    protected BuildingModel(BuildingConfig newBuildingConfig)
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
        if (inventory.GetResourceCount(GetUpgradeRequirement().resourceType) < GetUpgradeRequirement().count)
        {
            return;
        }

        level++;

        OnLevelChanged?.Invoke(level);
    }

    public void Tick(float deltaTime, InventoryModel inventory)
    {
        OnTick(deltaTime, inventory);
    }

    protected void RaiseProduced(int count)
    {
        OnProduced?.Invoke(count);
    }

    protected void SetProcessingState(bool newState)
    {
        if (isProcessing == newState)
        {
            return;
        }

        isProcessing = newState;
        OnProcessingStateChanged?.Invoke(isProcessing);
    }

    protected virtual void OnTick(float deltaTime, InventoryModel inventory)
    {
    }
}