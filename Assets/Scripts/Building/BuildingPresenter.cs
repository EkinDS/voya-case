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
        buildingView.SetProgress(buildingModel.GetTotalProgress());
    }

    private void InitializeView()
    {
        buildingView.SetTitle(buildingModel.GetBuildingName());
        buildingView.SetProgress(buildingModel.GetTotalProgress());

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
        inventoryModel.GainResource( buildingModel.GetOutputResourceType(), producedCount);

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
        bool hasEnoughResourcesToStartProduction = inventoryModel.GetResourceCount(buildingModel.GetRequiredInputResourceType()) >= buildingModel.GetRequiredInputCount();
        bool hasEnoughResourcesToUpgrade = inventoryModel.GetResourceCount(buildingModel.GetUpgradeRequirement().resourceType) >= buildingModel.GetUpgradeRequirement().count;
        var upgradeRequirement = buildingModel.GetUpgradeRequirement();

        buildingView.ArrangeInformation(buildingModel.GetLevel(), buildingModel.GetOutputCount(), buildingModel.GetProductionDuration(), upgradeRequirement.count, buildingModel.GetRequiredInputCount(),hasEnoughResourcesToUpgrade);
        buildingView.ArrangeUpgradeButton(hasEnoughResourcesToUpgrade, buildingModel.GetLevel() >= buildingModel.GetLevelCount() - 1, buildingModel.GetProcessingState());
        buildingView.ArrangeStartProductionButton(hasEnoughResourcesToStartProduction, buildingModel.GetProcessingState());
        buildingView.ArrangeAnimations(buildingModel.GetProcessingState());
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