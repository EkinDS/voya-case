using System;

public interface IBuildingView
{
    event Action OnUpgradeButtonClicked;
    event Action OnStartProductionButtonClicked;

    public void SetTitle(string title);
    public void SetProgress(float normalizedProgress);
    public void ArrangeUpgradeButton(bool thereAreEnoughResources);
    public void ArrangeStartProductionButton(bool thereAreEnoughResources, bool isProcessing);
    public void ArrangeInformation(int level, int outputCount, float durationSeconds, ResourceType resourceType, int count, int requiredInputCount);
    public void ArrangeAnimations(bool isProcessing);

}