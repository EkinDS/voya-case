public class FactoryPresenter : BuildingPresenter
{
    public FactoryPresenter(BuildingModel newBuildingModel, InventoryModel newInventoryModel, IBuildingView newInventoryView) : base(newBuildingModel, newInventoryModel, newInventoryView)
    {
    }
}