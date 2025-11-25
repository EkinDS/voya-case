using System;
using UnityEngine;

public abstract class BuildingPresenter : IDisposable
{
    private readonly BuildingModel buildingModel;
    private readonly InventoryModel inventoryModel;
    private readonly IBuildingView buildingView;

    protected BuildingPresenter(BuildingModel newBuildingModel, InventoryModel newInventoryModel,
        IBuildingView newInventoryView)
    {
        buildingModel = newBuildingModel;
        inventoryModel = newInventoryModel;
        buildingView = newInventoryView;

        buildingView.OnUpgradeButtonClicked += OnUpgradeButtonClicked;
        buildingView.OnStartProductionButtonClicked += OnStartProductionButtonClicked;
        buildingModel.OnLevelChanged += OnLevelChanged;
        buildingModel.OnProduced += OnProduced;
        buildingModel.OnProcessingStateChanged += OnProcessingStateChanged;

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
        RefreshStaticInfo();
        buildingView.SetProgress(buildingModel.progress);
        buildingView.SetProcessingState(false);
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
            buildingView.ShowNotEnoughResourcesFeedback();
            return;
        }

        inventoryModel.SpendResource(upgradeRequirement.resourceType, upgradeRequirement.count);

        buildingModel.Upgrade(inventoryModel);

        RefreshStaticInfo();
    }

    private void OnStartProductionButtonClicked()
    {
        ResourceType requiredInputResourceType = buildingModel.buildingConfig.requiredInputResourceType;
        int requiredInputCount = buildingModel.GetRequiredInputCount();

        if (requiredInputResourceType == ResourceType.None || requiredInputCount <= 0)
        {
            buildingModel.SetProcessingState(true);
            return;
        }

        if (inventoryModel.GetResourceCount(requiredInputResourceType) < requiredInputCount)
        {
            buildingView.ShowNotEnoughResourcesFeedback();
            return;
        }

        inventoryModel.SpendResource(requiredInputResourceType, requiredInputCount);
        buildingModel.SetProcessingState(true);
    }


    private void OnLevelChanged(int newLevel)
    {
        RefreshStaticInfo();
    }

    private void OnProduced(int producedCount)
    {
        buildingView.PlayProducedFeedback();
    }

    private void OnProcessingStateChanged(bool isProcessing)
    {
        buildingView.SetProcessingState(isProcessing);
    }

    private void RefreshStaticInfo()
    {
        buildingView.SetLevel(buildingModel.level);
        buildingView.SetProductionInfo(buildingModel.GetOutputCount(), buildingModel.GetProductionDuration());

        var upgradeRequirement = buildingModel.GetUpgradeRequirement();

        buildingView.SetUpgradeCost(upgradeRequirement.resourceType, upgradeRequirement.count);
    }

    public virtual void Dispose()
    {
        buildingView.OnStartProductionButtonClicked -= OnStartProductionButtonClicked;
        buildingView.OnUpgradeButtonClicked -= OnUpgradeButtonClicked;
        buildingModel.OnLevelChanged -= OnLevelChanged;
        buildingModel.OnProduced -= OnProduced;
        buildingModel.OnProcessingStateChanged -= OnProcessingStateChanged;
    }
}