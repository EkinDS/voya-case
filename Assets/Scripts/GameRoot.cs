using UnityEngine;

public class GameRoot : MonoBehaviour
{
    [SerializeField] private InventoryModel inventoryModel;
    [SerializeField] private InventoryView inventoryView;
    [SerializeField] private BuildingConfig earthMineConfig;
    [SerializeField] private BuildingConfig mudFactoryConfig;
    [SerializeField] private BuildingConfig clayFactoryConfig;
    [SerializeField] private EarthMineView earthMineViewPrefab;
    [SerializeField] private FactoryView mudFactoryViewPrefab;
    [SerializeField] private FactoryView clayFactoryViewPrefab;
    [SerializeField] private Transform viewParent;

    private InventoryPresenter inventoryPresenter;
    private EarthMinePresenter earthMinePresenter;
    private FactoryPresenter mudFactoryPresenter;
    private FactoryPresenter clayFactoryPresenter;

    private void Start()
    {
        inventoryPresenter = new InventoryPresenter(inventoryModel, inventoryView);

        var earthMineView = Instantiate(earthMineViewPrefab, new Vector3(0F, 2F, 0F), Quaternion.identity, viewParent);
        var mudFactoryView = Instantiate(mudFactoryViewPrefab, new Vector3(0F, 0F, 0F), Quaternion.identity, viewParent);
        var clayFactoryView = Instantiate(clayFactoryViewPrefab, new Vector3(0F, -2F, 0F), Quaternion.identity, viewParent);
        
        var earthMineModel = new EarthMineModel(earthMineConfig);
        var mudFactoryModel = new FactoryModel(mudFactoryConfig);
        var clayFactoryModel = new FactoryModel(clayFactoryConfig);

        earthMinePresenter = new EarthMinePresenter(earthMineModel, inventoryModel, earthMineView);
        mudFactoryPresenter = new FactoryPresenter(mudFactoryModel, inventoryModel, mudFactoryView);
        clayFactoryPresenter = new FactoryPresenter(clayFactoryModel, inventoryModel, clayFactoryView);
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