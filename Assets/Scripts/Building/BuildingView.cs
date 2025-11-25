using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingView : MonoBehaviour, IBuildingView
{
    [SerializeField] protected TextMeshProUGUI buildingNameText;
    [SerializeField] protected TextMeshProUGUI levelText;
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
    [SerializeField] protected CanvasGroup popUpCanvasGroup;
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
        upgradeButton.gameObject.SetActive(!isMaxed);
    }

    public virtual void ArrangeStartProductionButton(bool thereAreEnoughResources, bool isProcessing)
    {
        startProductionButton.interactable = thereAreEnoughResources;
        startProductionButton.gameObject.SetActive(!isProcessing);
        requiredInputCountText.color = thereAreEnoughResources ? Color.white : Color.red;
    }

    public virtual void ArrangeInformation(int level, int outputCount, float duration, int upgradeRequirementCount,
        int requiredInputCount)
    {
        levelText.text = (level + 1).ToString();
        requiredInputCountText.text = requiredInputCount.ToString();
        inputValueText.text = requiredInputCount.ToString();
        outputValueText.text = outputCount.ToString();
        durationValueText.text = duration + "s";
        productionSpeedValueText.text = (outputCount / duration * 60F).ToString("00.00") + "/min";
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

    private void ShowPopUp()
    {
        StopAllCoroutines();
        StartCoroutine(AnimatePopUpReveal());
    }

    private void HidePopUp()
    {
        StopAllCoroutines();

        popUpCanvasGroup.gameObject.SetActive(false);
    }

    private IEnumerator AnimatePopUpReveal()
    {
        popUpCanvasGroup.gameObject.SetActive(true);

        float timer = 0f;
        float duration = 0.2f;

        Vector3 scaleStart = new Vector3(0.8f, 0.8f, 0.8f);
        Vector3 scalePeak = new Vector3(1.1f, 1.1f, 1.1f);
        Vector3 scaleEnd = Vector3.one;

        popUpCanvasGroup.transform.localScale = scaleStart;
        popUpCanvasGroup.alpha = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / duration);

            Vector3 targetScale = t < 0.5f
                ? Vector3.Lerp(scaleStart, scalePeak, t / 0.5f)
                : Vector3.Lerp(scalePeak, scaleEnd, (t - 0.5f) / 0.5f);

            popUpCanvasGroup.transform.localScale = targetScale;

            popUpCanvasGroup.alpha = t;

            yield return null;
        }

        popUpCanvasGroup.transform.localScale = scaleEnd;
        popUpCanvasGroup.alpha = 1f;
    }
}