using System;

public interface IBuildingView
{
    public event Action OnUpgradeButtonClicked;
    public event Action OnStartProductionButtonClicked;

    public void Render(BuildingViewState state);
    public void SpawnResource();
    public void BounceScale();
    public void SetTitle(string buildingName);
    public void RefreshProcess(float progress);
}