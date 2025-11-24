using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingView : MonoBehaviour, IBuildingView
{
    [SerializeField] protected TextMeshProUGUI titleText;
    [SerializeField] protected TextMeshProUGUI levelText;
    [SerializeField] protected TextMeshProUGUI productionText;
    [SerializeField] protected TextMeshProUGUI upgradeCostText;
    [SerializeField] protected TextMeshProUGUI maxText;
    [SerializeField] protected Image progressBarFillerImage;
    [SerializeField] protected Button upgradeButton;

    public event Action OnUpgradeClicked;

    protected Color defaultUpgradeTextColor;

    protected virtual void Awake()
    {
        defaultUpgradeTextColor = upgradeCostText.color;
    }

    public void HandleUpgradeButtonClicked()
    {
        OnUpgradeClicked?.Invoke();
    }

    public virtual void SetTitle(string title)
    {
        titleText.text = title;
    }

    public virtual void SetLevel(int level)
    {
        levelText.text = "Level " +  level;
    }

    public virtual void SetProductionInfo(int outputCount, float durationSeconds)
    {
        productionText.text = $"{outputCount} / {durationSeconds:0.0}s";
    }

    public virtual void SetProgress(float normalizedProgress)
    {
        progressBarFillerImage.fillAmount = Mathf.Clamp01(normalizedProgress);
    }

    public virtual void SetUpgradeCost(ResourceType resourceType, int count)
    {
        upgradeButton.interactable = true;

        upgradeCostText.text = $"{count} {resourceType}";
        upgradeCostText.color = defaultUpgradeTextColor;
    }

    public virtual void ShowNotEnoughResourcesFeedback()
    {
        upgradeCostText.color = Color.red;
    }

    public virtual void PlayProducedFeedback()
    {
        
    }

    public virtual void SetProcessingState(bool isProcessing)
    {
        
    }
}