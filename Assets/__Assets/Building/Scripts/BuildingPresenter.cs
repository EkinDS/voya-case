using System;
using UnityEngine;

public class BuildingPresenter : IDisposable
{
    private readonly BuildingModel buildingModel;
    private readonly InventoryModel inventoryModel;
    private readonly IBuildingView buildingView;

    public BuildingPresenter(BuildingModel newBuildingModel, InventoryModel newInventoryModel,
        IBuildingView newBuildingView)
    {
        buildingModel = newBuildingModel;
        inventoryModel = newInventoryModel;
        buildingView = newBuildingView;

        buildingView.OnUpgradeButtonClicked += OnUpgradeButtonClicked;
        buildingView.OnStartProductionButtonClicked += OnStartProductionButtonClicked;
        buildingModel.OnLevelChanged += OnLevelChanged;
        buildingModel.OnProduced += OnProduced;
        buildingModel.OnProcessingStateChanged += OnProcessingStateChanged;

        inventoryModel.OnInventoryChanged += OnInventoryChanged;

        InitializeView();
    }

    public void Tick(float deltaTime)
    {
        buildingModel.Tick(deltaTime);
        buildingView.RefreshProcess(buildingModel.GetTotalProgress());
    }

    private void InitializeView()
    {
        RefreshView();
    }

    private void OnUpgradeButtonClicked()
    {
        var upgradeRequirement = buildingModel.GetUpgradeRequirement();

        if (upgradeRequirement.resourceType == ResourceType.None)
        {
            return;
        }

        int currentResourceAmount = inventoryModel.GetResourceCount(upgradeRequirement.resourceType);

        if (currentResourceAmount < upgradeRequirement.count)
        {
            return;
        }

        inventoryModel.SpendResource(upgradeRequirement.resourceType, upgradeRequirement.count);

        buildingModel.Upgrade();

        RefreshView();
    }

    private void OnStartProductionButtonClicked()
    {
        if (buildingModel.GetProcessingState())
        {
            return;
        }

        ResourceType requiredInputResourceType = buildingModel.GetRequiredInputResourceType();
        int requiredInputCount = buildingModel.GetRequiredInputCount();

        if (requiredInputResourceType == ResourceType.None || requiredInputCount <= 0)
        {
            buildingModel.AddNewCycle();
            buildingModel.SetProcessingState(true);
            return;
        }

        if (inventoryModel.GetResourceCount(requiredInputResourceType) < requiredInputCount)
        {
            return;
        }

        inventoryModel.SpendResource(requiredInputResourceType, requiredInputCount);
        buildingModel.AddNewCycle();
        buildingModel.SetProcessingState(true);
    }


    private void OnLevelChanged(int newLevel)
    {
        RefreshView();
    }

    private void OnProduced(int producedCount)
    {
        inventoryModel.GainResource(buildingModel.GetOutputResourceType(), producedCount);

        buildingView.SpawnResource();
        buildingView.BounceScale();
        RefreshView();
    }

    private void OnProcessingStateChanged(bool isProcessing)
    {
        RefreshView();
    }


    private void OnInventoryChanged(ResourceType type, int newAmount)
    {
        RefreshView();
    }


    private void RefreshView()
    {
        var upgradeRequirement = buildingModel.GetUpgradeRequirement();
        var requiredInputCount = buildingModel.GetRequiredInputCount();
        var level = buildingModel.GetLevel();
        var isMaxLevel = level >= buildingModel.GetLevelCount() - 1;
        var hasEnoughToUpgrade = upgradeRequirement.resourceType != ResourceType.None && inventoryModel.GetResourceCount(upgradeRequirement.resourceType) >= upgradeRequirement.count;
        var hasEnoughToStart = inventoryModel.GetResourceCount(buildingModel.GetRequiredInputResourceType()) >= requiredInputCount;

        var state = new BuildingViewState
        {
            name = buildingModel.GetBuildingName(),
            level = level,
            outputCount = buildingModel.GetOutputCount(),
            durationSeconds = buildingModel.GetProductionDuration(),
            upgradeRequirementCount = upgradeRequirement.count,
            requiredInputCount = requiredInputCount,
            canUpgrade = hasEnoughToUpgrade && !isMaxLevel,
            isMaxLevel = isMaxLevel,
            canStartProduction = hasEnoughToStart && !buildingModel.GetProcessingState(),
            isProcessing = buildingModel.GetProcessingState(),
        };

        buildingView.Render(state);
    }

    public virtual void Dispose()
    {
        buildingView.OnStartProductionButtonClicked -= OnStartProductionButtonClicked;
        buildingView.OnUpgradeButtonClicked -= OnUpgradeButtonClicked;
        buildingModel.OnLevelChanged -= OnLevelChanged;
        buildingModel.OnProduced -= OnProduced;
        buildingModel.OnProcessingStateChanged -= OnProcessingStateChanged;
        inventoryModel.OnInventoryChanged -= OnInventoryChanged;
    }
}