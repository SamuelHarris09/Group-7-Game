using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI References")]
    public TextMeshProUGUI timeText;
    public GameObject pauseScreen;
    public GameObject keyIcon;
    public GameObject key;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void SetTime(string time)
    {
        if (timeText != null)
            timeText.text = time;
    }

    public void ShowPause(bool value)
    {
        if (pauseScreen != null)
            pauseScreen.SetActive(value);
    }

    public void ShowKeyIcon(bool value)
    {
        if (keyIcon != null)
            keyIcon.SetActive(value);
    }

    public void ShowKey(bool value)
    {
        if (key != null) 
            key.SetActive(value);
    }
}