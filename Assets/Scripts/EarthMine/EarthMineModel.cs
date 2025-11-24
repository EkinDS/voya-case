using UnityEngine;

public class EarthMineModel : BuildingModel
{
    public EarthMineModel(BuildingConfig newBuildingConfig) : base(newBuildingConfig)
    {
      
    }

    protected override void OnTick(float deltaTime, InventoryModel inventory)
    {
        float productionDuration = GetProductionDuration();
        
        if (!isProcessing)
        {
            SetProcessingState(true);
        }

        productionTimer += deltaTime;

        progress = Mathf.Clamp01(productionTimer / productionDuration);

        if (!(productionTimer >= productionDuration))
        {
            return;
        }
        
        productionTimer -= productionDuration;

        int outputCount = GetOutputCount();
        ResourceType outputResourceType = buildingConfig.outputResourceType;

        inventory.GainResource(outputResourceType, outputCount);
        RaiseProduced(outputCount);
    }
}