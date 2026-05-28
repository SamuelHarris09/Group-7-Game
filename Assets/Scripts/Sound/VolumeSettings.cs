using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider jumpSoundSlider;
    [SerializeField] private Slider backgroundVolumeSlider;
    [SerializeField] private Slider menuVolumeSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("BackgroundVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetJumpVolume();
            SetBackgroundVolume();
            SetMenuVolume();
        }
    }

    public void SetJumpVolume()
    {
        float volume = jumpSoundSlider.value;
        audioMixer.SetFloat("Jump", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat("JumpVolume", volume);
    }

    public void SetBackgroundVolume()
    {
        float volume = backgroundVolumeSlider.value;
        audioMixer.SetFloat("Background", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat("BackgroundVolume", volume);
    }

    public void SetMenuVolume()
    {
        float volume = menuVolumeSlider.value;
        audioMixer.SetFloat("Menu", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat("MenuVolume", volume);
    }

    private void LoadVolume()
    {
        menuVolumeSlider.value = PlayerPrefs.GetFloat("MenuVolume");
        jumpSoundSlider.value = PlayerPrefs.GetFloat("JumpVolume");
        backgroundVolumeSlider.value = PlayerPrefs.GetFloat("BackgroundVolume");

        SetMenuVolume();
        SetJumpVolume();
        SetBackgroundVolume();
    }
}