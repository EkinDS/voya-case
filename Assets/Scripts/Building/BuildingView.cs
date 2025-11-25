using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingView : MonoBehaviour, IBuildingView
{
    [SerializeField] protected TextMeshProUGUI buildingNameText;
    [SerializeField] protected TextMeshProUGUI levelText;
    [SerializeField] protected TextMeshProUGUI productionText;
    [SerializeField] protected TextMeshProUGUI upgradeText;
    [SerializeField] protected TextMeshProUGUI requiredInputCountText;
    [SerializeField] protected TextMeshProUGUI inputValueText;
    [SerializeField] protected TextMeshProUGUI outputValueText;
    [SerializeField] protected TextMeshProUGUI durationValueText;
    [SerializeField] protected TextMeshProUGUI productionSpeedValueText;
    [SerializeField] protected Image productionRequirementBackgroundImage;
    [SerializeField] protected Image progressBarFillerImage;
    [SerializeField] protected Image upgradeArrowImage;
    [SerializeField] protected Button upgradeButton;
    [SerializeField] protected Button startProductionButton;
    [SerializeField] protected List<Animator> animators;
    [SerializeField] protected Transform popUpTransform;

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

    public void HandleShowPopUpButtonClicked()
    {
        ShowPopUp();
    }

    public void HandleExitButtonClicked()
    {
        HidePopUp();
    }

    public virtual void SetTitle(string buildingName)
    {
        buildingNameText.text = buildingName;
    }

    public virtual void SetProgress(float normalizedProgress)
    {
        progressBarFillerImage.fillAmount = Mathf.Clamp01(normalizedProgress);
    }

    public virtual void ArrangeUpgradeButton(bool thereAreEnoughResources, bool isMaxed, bool isProcessing)
    {
        upgradeArrowImage.gameObject.SetActive(thereAreEnoughResources && !isMaxed && !isProcessing);
        upgradeButton.interactable = thereAreEnoughResources && !isMaxed && !isProcessing;
    }

    public virtual void ArrangeStartProductionButton(bool thereAreEnoughResources, bool isProcessing)
    {
        startProductionButton.interactable = thereAreEnoughResources;
        startProductionButton.gameObject.SetActive(!isProcessing);
    }

    public virtual void ArrangeInformation(int level, int outputCount, float duration, int upgradeRequirementCount, int requiredInputCount, bool hasEnoughResourcesToUpgrade)
    {
        levelText.text = (level + 1).ToString();
        productionText.text = $"{outputCount} / {duration:0.0}s";
        requiredInputCountText.text = requiredInputCount.ToString();
        requiredInputCountText.color = hasEnoughResourcesToUpgrade ? Color.white : Color.red;
        inputValueText.text = requiredInputCount.ToString();
        outputValueText.text = outputCount.ToString();
        durationValueText.text = duration + "s";
        productionSpeedValueText.text = (outputCount / duration * 60F).ToString("00.00");
        upgradeText.text = upgradeRequirementCount.ToString();

        productionRequirementBackgroundImage.gameObject.SetActive(requiredInputCount > 0);
    }

    public void ArrangeAnimations(bool isProcessing)
    {
        foreach (var animator in animators)
        {
            animator.enabled = isProcessing;
        }
    }

    public void ShowPopUp()
    {
        popUpTransform.gameObject.SetActive(true);
    }

    public void HidePopUp()
    {
        popUpTransform.gameObject.SetActive(false);
    }
}