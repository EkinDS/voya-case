using UnityEngine;

public class GameRoot : MonoBehaviour
{
    [SerializeField] private InventoryView inventoryView;
    [SerializeField] private BuildingConfig earthMineConfig;
    [SerializeField] private BuildingConfig mudFactoryConfig;
    [SerializeField] private BuildingConfig clayFactoryConfig;
    [SerializeField] private BuildingView earthMineViewPrefab;
    [SerializeField] private BuildingView mudFactoryViewPrefab;
    [SerializeField] private BuildingView clayFactoryViewPrefab;
    [SerializeField] private Transform viewParent;

    private InventoryPresenter inventoryPresenter;
    private BuildingPresenter earthMinePresenter;
    private BuildingPresenter mudFactoryPresenter;
    private BuildingPresenter clayFactoryPresenter;

    private void Start()
    {
        var earthMineView = Instantiate(earthMineViewPrefab, new Vector3(0F, -3.5F, 0F), Quaternion.identity, viewParent);
        var mudFactoryView = Instantiate(mudFactoryViewPrefab, new Vector3(0F, -0.5F, 0F), Quaternion.identity, viewParent);
        var clayFactoryView = Instantiate(clayFactoryViewPrefab, new Vector3(0F, 2.5F, 0F), Quaternion.identity, viewParent);

        var inventoryModel = new InventoryModel();
        var earthMineModel = new BuildingModel(earthMineConfig);
        var mudFactoryModel = new BuildingModel(mudFactoryConfig);
        var clayFactoryModel = new BuildingModel(clayFactoryConfig);

        inventoryPresenter = new InventoryPresenter(inventoryModel, inventoryView);
        earthMinePresenter = new BuildingPresenter(earthMineModel, inventoryModel, earthMineView);
        mudFactoryPresenter = new BuildingPresenter(mudFactoryModel, inventoryModel, mudFactoryView);
        clayFactoryPresenter = new BuildingPresenter(clayFactoryModel, inventoryModel, clayFactoryView);
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        earthMinePresenter?.Tick(deltaTime);
        mudFactoryPresenter?.Tick(deltaTime);
        clayFactoryPresenter?.Tick(deltaTime);
    }

    private void OnDestroy()
    {
        inventoryPresenter?.Dispose();
        earthMinePresenter?.Dispose();
        mudFactoryPresenter?.Dispose();
        clayFactoryPresenter?.Dispose();
    }
}