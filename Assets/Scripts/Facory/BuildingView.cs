using System;
using System.Collections.Generic;
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
    [SerializeField] protected TextMeshProUGUI productionRequirementText;
    [SerializeField] protected Image progressBarFillerImage;
    [SerializeField] protected Button upgradeButton;
    [SerializeField] protected Button startProductionButton;
    [SerializeField] protected List<Animator> animators;

    public event Action OnUpgradeButtonClicked;
    public event Action OnStartProductionButtonClicked;

    public void HandleUpgradeButtonClicked()
    {
        OnUpgradeButtonClicked?.Invoke();
    }

    public void HandleStartProductionButtonClicked()
    {
        OnStartProductionButtonClicked?.Invoke();
    }

    public virtual void SetTitle(string title)
    {
        titleText.text = title;
    }

    public virtual void SetProgress(float normalizedProgress)
    {
        progressBarFillerImage.fillAmount = Mathf.Clamp01(normalizedProgress);
    }

    public virtual void ArrangeUpgradeButton(bool thereAreEnoughResources)
    {
        upgradeButton.gameObject.SetActive(thereAreEnoughResources);
    }

    public virtual void ArrangeStartProductionButton(bool thereAreEnoughResources, bool isProcessing)
    {
        startProductionButton.interactable = thereAreEnoughResources;
        startProductionButton.gameObject.SetActive(!isProcessing);
    }

    public virtual void ArrangeInformation(int level, int outputCount, float durationSeconds, ResourceType resourceType,
        int count)
    {
        levelText.text = level.ToString();
        productionText.text = $"{outputCount} / {durationSeconds:0.0}s";
        upgradeCostText.text = $"{count} {resourceType}";
    }

    public void ArrangeAnimations(bool isProcessing)
    {
        foreach (var animator in animators)
        {
            animator.enabled = isProcessing;
        }
    }
}