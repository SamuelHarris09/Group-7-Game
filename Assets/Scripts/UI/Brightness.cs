using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Brightness : MonoBehaviour
{
    public Slider brightnessSlider;

    public Volume globalVolume;

    private ColorAdjustments colorAdjustments;

    private void Start()
    {
        if (globalVolume.profile.TryGet(out colorAdjustments))
        {
            float savedBrightness = PlayerPrefs.GetFloat("Brightness", 0f);

            brightnessSlider.value = savedBrightness;

            SetBrightness(savedBrightness);

            brightnessSlider.onValueChanged.AddListener(SetBrightness);
        }
        else
        {
            Debug.LogError("Color Adjustments override not found!");
        }
    }

    public void SetBrightness(float value)
    {
        colorAdjustments.postExposure.value = value;

        PlayerPrefs.SetFloat("Brightness", value);
        PlayerPrefs.Save();
    }

}