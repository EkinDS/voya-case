using UnityEngine;

public class FactoryModel : BuildingModel
{
    public FactoryModel(BuildingConfig newBuildingConfig) : base(newBuildingConfig)
    {
    }

    protected override void OnTick(float deltaTime, InventoryModel inventory)
    {
        float productionDuration = GetProductionDuration();

        if (!isProcessing)
        {
            int requiredInputCount = GetRequiredInputCount();
            
            if (inventory.GetResourceCount(buildingConfig.requiredInputResourceType) >= requiredInputCount)
            {
                inventory.SpendResource(buildingConfig.requiredInputResourceType, requiredInputCount);

                productionTimer = 0F;
                progress = 0F;
                SetProcessingState(true);
            }
            else
            {
                progress = 0F;
                SetProcessingState(false);
            }
        }
        else
        {
            productionTimer += deltaTime;
            progress = Mathf.Clamp01(productionTimer / productionDuration);

            if (productionTimer >= productionDuration)
            {
                productionTimer -= productionDuration;

                int outputCount = GetOutputCount();
                ResourceType outputResourceType = buildingConfig.outputResourceType;

                inventory.GainResource(outputResourceType, outputCount);
                RaiseProduced(outputCount);
             
                SetProcessingState(false);
                progress = 0F;
            }
        }
    }
}