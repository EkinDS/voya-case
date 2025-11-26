using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingView : MonoBehaviour, IBuildingView
{
    [SerializeField] private Transform modelTransform;
    [SerializeField] private TextMeshProUGUI buildingNameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private TextMeshProUGUI requiredInputCountText;
    [SerializeField] private TextMeshProUGUI inputValueText;
    [SerializeField] private TextMeshProUGUI outputValueText;
    [SerializeField] private TextMeshProUGUI durationValueText;
    [SerializeField] private TextMeshProUGUI productionSpeedValueText;
    [SerializeField] private Image productionRequirementBackgroundImage;
    [SerializeField] private Image progressBarFillerImage;
    [SerializeField] private Image upgradeArrowImage;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button startProductionButton;
    [SerializeField] private CanvasGroup popUpCanvasGroup;
    [SerializeField] private GameObject resourceToClone;
    [SerializeField] private List<Animator> animators;

    private Coroutine popUpCoroutine;

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

    public virtual void ArrangeUpgradeButton(bool thereAreEnoughResources, bool isMaxed)
    {
        upgradeArrowImage.gameObject.SetActive(thereAreEnoughResources && !isMaxed);
        upgradeButton.interactable = thereAreEnoughResources && !isMaxed;
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

    public void SpawnResource()
    {
        StartCoroutine(SpawnAndMoveResource());
    }

    public void BounceScale()
    {
        StartCoroutine(AnimateBounceScale());
    }
    
    private void ShowPopUp()
    {
        if (popUpCoroutine != null)
        {
            StopCoroutine(popUpCoroutine);
        }

        StartCoroutine(AnimatePopUpReveal());
    }

    private void HidePopUp()
    {
        if (popUpCoroutine != null)
        {
            StopCoroutine(popUpCoroutine);
        }

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

    private IEnumerator SpawnAndMoveResource()
    {
        float timer = 0f;
        float duration = 0.8F;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + new Vector3(0F, 3F, 0F);

        GameObject newResource = Instantiate(resourceToClone, startPosition, Quaternion.identity, transform.parent);

        newResource.gameObject.SetActive(true);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(timer / duration);

            newResource.transform.position = Vector3.Lerp(startPosition, endPosition, normalizedTime);

            yield return null;
        }

        newResource.transform.position = endPosition;
        Destroy(newResource);
    }
    
    private IEnumerator AnimateBounceScale()
    {
        float duration = 0.2f;
        float timer = 0f;

        Vector3 scaleStart = Vector3.one;
        Vector3 scalePeak = new Vector3(1.1f, 1.1f, 1.1f);
        Vector3 scaleEnd = Vector3.one;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);

            Vector3 targetScale = t < 0.5f ? Vector3.Lerp(scaleStart, scalePeak, t / 0.5f) : Vector3.Lerp(scalePeak, scaleEnd, (t - 0.5f) / 0.5f);

            modelTransform.localScale = targetScale;

            yield return null;
        }

        modelTransform.localScale = scaleEnd;
    }
}