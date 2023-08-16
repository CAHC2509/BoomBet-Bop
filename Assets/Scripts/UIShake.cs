using System.Collections;
using UnityEngine;

/// <summary>
/// /// This script needs to be attached to a canvas element to make the shake effect
/// </summary>
public class UIShake : MonoBehaviour
{
    public float shakeDuration = 0.1f;
    public float shakeMagnitude = 0.1f;

    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.localPosition;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Shake()
    {
        gameObject.SetActive(true);
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float timer = shakeDuration;

        while (timer > 0f)
        {
            if (Time.timeScale > 0f)
            {
                ShakeUIElement();
                timer -= Time.deltaTime;
            }

            yield return null;
        }

        rectTransform.localPosition = originalPosition;
        canvasGroup.alpha = 1f;
    }

    private void ShakeUIElement()
    {
        float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
        float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;
        rectTransform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0f);

        // Reduce la opacidad para simular el efecto de movimiento
        canvasGroup.alpha = Mathf.Lerp(1f, 0f, Mathf.InverseLerp(shakeDuration, 0f, shakeDuration - Time.deltaTime));
    }
}
