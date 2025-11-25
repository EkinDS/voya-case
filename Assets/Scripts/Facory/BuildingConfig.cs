using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BuildingConfig", menuName = "ScriptableObjects/BuildingConfig")]
public class BuildingConfig : ScriptableObject
{
    public string buildingName;
    public ResourceType requiredInputResourceType;
    public ResourceType outputResourceType;
    public List<BuildingLevelData> levels = new();

    private BuildingLevelData GetLevelData(int level)
    {
        return levels[level];
    }

    public float GetProductionDuration(int level)
    {
        return levels[level].productionDuration;
    }

    public int GetOutputCount(int level)
    {
        return levels[level].outputCount;
    }

    public int GetRequiredInputCount(int level)
    {
        return levels[level].requiredInputCount;
    }

    public UpgradeRequirement GetUpgradeRequirement(int level)
    {
        return levels[level].upgradeRequirement;
    }

    public int GetCycleCount(int level)
    {
        return levels[level].cycleCount;
    }
}


[Serializable]
public class BuildingLevelData
{
    public int requiredInputCount;
    public int outputCount;
    public int cycleCount;
    public float productionDuration;
    public UpgradeRequirement upgradeRequirement;
}

[Serializable]
public class UpgradeRequirement
{
    public ResourceType resourceType;
    public int count;
}