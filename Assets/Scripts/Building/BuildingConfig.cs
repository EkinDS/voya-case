using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingConfig", menuName = "ScriptableObjects/BuildingConfig")]
public class BuildingConfig : ScriptableObject
{
    public string buildingName;
    public ResourceType outputResourceType;
    public List<BuildingLevelData> levels = new();

    private BuildingLevelData GetLevelData(int level)
    {
        return levels[level];
    }

    public float GetCycleTime(int level)
    {
        return levels[level].productionTime;
    }

    public int GetOutputAmount(int level)
    {
        return levels[level].outputAmount;
    }

    public int GetInputAmount(int level)
    {
        return levels[level].inputAmount;
    }

    public int GetUpgradeCostAmount(int level)
    {
        return levels[level].upgradeCostAmount;
    }

    public ResourceType GetUpgradeCostResourceType(int level)
    {
        return levels[level].upgradeResourceType;
    }
}


[Serializable]
public class BuildingLevelData
{
    public float productionTime;
    public int inputAmount;
    public int outputAmount;
    public ResourceType upgradeResourceType;
    public int upgradeCostAmount;
}