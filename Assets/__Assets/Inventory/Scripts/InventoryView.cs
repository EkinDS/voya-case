using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour, IInventoryView
{
    [SerializeField] private TextMeshProUGUI earthText;
    [SerializeField] private TextMeshProUGUI mudText;
    [SerializeField] private TextMeshProUGUI clayText;
    [SerializeField] private Image earthImage;
    [SerializeField] private Image mudImage;
    [SerializeField] private Image clayImage;

    public void SetResourceAmount(ResourceType type, int count)
    {
        switch (type)
        {
            case ResourceType.Earth:
                earthText.text = count.ToString();
                StartCoroutine(AnimateBounceScale(earthText, earthImage));
                break;
            case ResourceType.Mud:
                mudText.text = count.ToString();
                StartCoroutine(AnimateBounceScale(mudText, mudImage));
                break;
            case ResourceType.Clay:
                clayText.text = count.ToString();
                StartCoroutine(AnimateBounceScale(clayText, clayImage));
                break;
        }
    }
    
    private IEnumerator AnimateBounceScale(TextMeshProUGUI text, Image image)
    {
        float duration = 0.2f;
        float timer = 0f;

        Vector3 scaleStart = Vector3.one;
        Vector3 scalePeak = new Vector3(1.2f, 1.2f, 1.2f);
        Vector3 scaleEnd = Vector3.one;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);

            Vector3 targetScale = t < 0.5f ? Vector3.Lerp(scaleStart, scalePeak, t / 0.5f) : Vector3.Lerp(scalePeak, scaleEnd, (t - 0.5f) / 0.5f);

            text.transform.localScale = targetScale;
            image.transform.localScale = targetScale;
            yield return null;
        }

        text.transform.localScale = scaleEnd;
        image.transform.localScale = scaleEnd;

    }
}