using System;

public interface IBuildingView
{
    event Action OnUpgradeButtonClicked;
    event Action OnStartProductionButtonClicked;

    public void SetTitle(string buildingName);
    public void SetProgress(float normalizedProgress);
    public void ArrangeUpgradeButton(bool thereAreEnoughResources, bool isMaxed, bool isProcessing);
    public void ArrangeStartProductionButton(bool thereAreEnoughResources, bool isProcessing);
    public void ArrangeInformation(int level, int outputCount, float durationSeconds, int upgradeRequirementCount, int requiredInputCount);
    public void ArrangeAnimations(bool isProcessing);
}