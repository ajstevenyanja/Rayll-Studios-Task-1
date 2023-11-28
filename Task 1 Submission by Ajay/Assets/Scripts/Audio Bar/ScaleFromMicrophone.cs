using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Simple script to lerp the scale of image rect transform
// based on loudness of the microphone

public class ScaleFromMicrophone : MonoBehaviour
{
    [SerializeField] float minScale = 0.1f;
    [SerializeField] float maxScale = 1.0f;
    [SerializeField] float loudnessMultiplier = 10.0f;
    [SerializeField] float minLoudness = 0.05f;
    public AudioLoudnessDetection loudnessDetector;
    [SerializeField] Image barImage;
    [SerializeField] RectTransform rectTransform;

    [Header("Color Settings")]
    [SerializeField] Color lowColor = Color.white;
    [SerializeField] float mediumLimit = 0.5f;
    [SerializeField] Color mediumColor = Color.yellow;
    [SerializeField] float highLimit = 0.7f;
    [SerializeField] Color highColor = Color.red;

    private void Update()
    {
        float loudness = loudnessDetector.GetLoudnessFromMicrophone() * loudnessMultiplier;

        SetBarColor(loudness);

        Vector3 scale = rectTransform.localScale;

        if (loudness > minLoudness)
        {
            scale.y = Mathf.Lerp(minScale, maxScale, loudness);
        }
        else
        {
            // If lower than min, lerp to 0
            scale.y = Mathf.Lerp(scale.y, 0, 10 * Time.deltaTime);
        }

        rectTransform.localScale = scale;
    }

    private void SetBarColor(float loudness)
    {
        if (loudness > highLimit)
        {
            barImage.color = highColor;
        }
        else if (loudness > mediumLimit)
        {
            barImage.color = mediumColor;
        }
        else
        {
            barImage.color = lowColor;
        }
    }
}
