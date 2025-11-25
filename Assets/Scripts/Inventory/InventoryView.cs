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
                earthText.text = count.ToString();
                break;
            case ResourceType.Mud:
                mudText.text = count.ToString();
                break;
            case ResourceType.Clay:
                clayText.text = count.ToString();
                break;
        }
    }
}