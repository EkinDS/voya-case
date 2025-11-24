using TMPro;
using UnityEngine;

public class InventoryView : MonoBehaviour, IInventoryView
{
    [SerializeField] private TextMeshProUGUI earthText;
    [SerializeField] private TextMeshProUGUI mudText;
    [SerializeField] private TextMeshProUGUI clayText;

    public void SetResourceAmount(ResourceType type, int count)
    {
        switch (type)
        {
            case ResourceType.Earth:
                earthText.text = $"Earth: {count}";
                break;
            case ResourceType.Mud:
                mudText.text = $"Mud: {count}";
                break;
            case ResourceType.Clay:
                clayText.text = $"Clay: {count}";
                break;
        }
    }
}