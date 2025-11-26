using System;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    [SerializeField] private InventoryView inventoryView;
    [SerializeField] private Transform buildingContainerTransform;
    [SerializeField] private List<BuildingEntry> buildingEntries;

    private InventoryPresenter inventoryPresenter;
    private List<BuildingPresenter> buildingPresenters;
    
    private void Start()
    {
        Application.targetFrameRate = 60;
        
        var inventoryModel = new InventoryModel();
        inventoryPresenter = new InventoryPresenter(inventoryModel, inventoryView);
        buildingPresenters = new List<BuildingPresenter>();

        foreach (var entry in buildingEntries)
        {
            var view = Instantiate(entry.buildingViewPrefab, entry.spawnPosition, Quaternion.identity, buildingContainerTransform);
            var model = new BuildingModel(entry.buildingConfig);
            buildingPresenters.Add(  new BuildingPresenter(model, inventoryModel, view));
        }
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        foreach (var buildingPresenter in buildingPresenters)
        {
            buildingPresenter.Tick(deltaTime);
        }
    }

    private void OnDestroy()
    {
        inventoryPresenter?.Dispose();
        
        foreach (var buildingPresenter in buildingPresenters)
        {
            buildingPresenter.Dispose();
        }
    }
}

[Serializable]
public class BuildingEntry
{
    public BuildingConfig buildingConfig;
    public BuildingView buildingViewPrefab;
    public Vector3 spawnPosition;
}