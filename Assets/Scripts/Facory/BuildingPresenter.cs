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
        buildingModel.Tick(deltaTime, inventoryModel);
        buildingView.SetProgress(buildingModel.progress);
    }

    private void InitializeView()
    {
        buildingView.SetTitle(buildingModel.buildingConfig.buildingName);
        buildingView.SetProgress(buildingModel.progress);

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

        buildingModel.Upgrade(inventoryModel);

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
            buildingModel.SetProcessingState(true);
            return;
        }

        if (inventoryModel.GetResourceCount(requiredInputResourceType) < requiredInputCount)
        {
            return;
        }

        inventoryModel.SpendResource(requiredInputResourceType, requiredInputCount);
        buildingModel.SetProcessingState(true);
    }


    private void OnLevelChanged(int newLevel)
    {
        RefreshView();
    }

    private void OnProduced(int producedCount)
    {
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

        buildingView.ArrangeInformation(buildingModel.level, buildingModel.GetOutputCount(),
            buildingModel.GetProductionDuration(), upgradeRequirement.resourceType, upgradeRequirement.count);

        buildingView.ArrangeUpgradeButton(
            inventoryModel.GetResourceCount(buildingModel.GetUpgradeRequirement().resourceType) >=
            buildingModel.GetUpgradeRequirement().count);

        buildingView.ArrangeStartProductionButton(
            inventoryModel.GetResourceCount(buildingModel.GetRequiredInputResourceType()) >=
            buildingModel.GetRequiredInputCount(), buildingModel.GetProcessingState());
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