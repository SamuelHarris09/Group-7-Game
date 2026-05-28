using UnityEngine;

public enum SoundType
{
    Jump
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip[] soundList;

    [Header("Music")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip menuMusic;

    [Header("Audio")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource backgroundSource;
    [SerializeField] private AudioSource menuSource;

    private void Awake()
    {
        if (Instance == null && Instance != this)
        {
            Destroy(Instance);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(Instance);
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }
    #region SoundAffects
    public static void PlaySound(SoundType sound, float volume = 1f)
    {
        Instance.sfxSource.PlayOneShot(Instance.soundList[(int)sound], volume);
    }
    #endregion
    #region Music
    public void PlayBackgroundMusic()
    {
        backgroundSource.clip = backgroundMusic;
        backgroundSource.loop = true;
        backgroundSource.Play();
    }

    public void PauseBackgroundMusic()
    {
        backgroundSource.Pause();
    }

    public void ResumeBackgroundMusic()
    {
        backgroundSource.UnPause();
    }

    public void PlayMenuMusic()
    {
        if (!menuSource.isPlaying)
        {
            menuSource.clip = menuMusic;
            menuSource.loop = true;
            menuSource.Play();
        }
    }

    public void StopMenuMusic()
    {
        menuSource.Stop();
    }
    #endregion
}