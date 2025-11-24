using System;

public interface IBuildingView
{
    event Action OnUpgradeClicked;

    public void SetTitle(string title);
    public void SetLevel(int level);
    public void SetProductionInfo(int outputCount, float durationSeconds);
    public void SetProgress(float normalizedProgress);
    public void SetUpgradeCost(ResourceType resourceType, int count);
    public void ShowNotEnoughResourcesFeedback();
    public void PlayProducedFeedback();
    public void SetProcessingState(bool isProcessing);
}