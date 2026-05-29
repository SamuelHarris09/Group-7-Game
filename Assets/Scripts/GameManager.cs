using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Pause")]
    [SerializeField] public Sprite[] changeDoor;
    [SerializeField] private float gamePlayLevelCount = 5;

    private float timeElapsed = 0f;
    private bool keySpawned = false;
    public bool keyIconOn = false;

    public SpriteRenderer door;

    Health playerHealth;
    InputAction pauseMenu;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        pauseMenu = InputSystem.actions.FindAction("Pause Menu");
    }
    
    void Update()
    {
        TimeCounter();
        Menu();
        NextLevel();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;

        door = GameObject.FindWithTag("Door")?.GetComponent<SpriteRenderer>();

        playerHealth = FindFirstObjectByType<Health>();

        keySpawned = false;
        keyIconOn = false;

        if (UIManager.instance != null)
        {
            UIManager.instance.ShowPause(false);
            UIManager.instance.ShowKeyIcon(false);
        }

        if (scene.buildIndex < gamePlayLevelCount)
        {
            if (door != null && changeDoor.Length > 0)
                door.sprite = changeDoor[0];
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #region UI
    void TimeCounter()
    {
        timeElapsed += Time.deltaTime;
        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);

        string formatted = string.Format("{0:00}:{1:00}", minutes, seconds);
        
        if (UIManager.instance != null)
            UIManager.instance.SetTime(formatted);
    } 

    void Menu()
    {
        if (UIManager.instance == null || pauseMenu == null)
            return;

        if (pauseMenu.WasPressedThisFrame())
        {
            bool isOpen = UIManager.instance.pauseScreen.activeInHierarchy;
            UIManager.instance.ShowPause(!isOpen);

            Time.timeScale = isOpen ? 1 : 0;

            if (isOpen)
            {
                SoundManager.instance.PauseBackgroundMusic();
                SoundManager.instance.PlayMenuMusic();
            }
            else
            {
                SoundManager.instance.ResumeBackgroundMusic();
                SoundManager.instance.StopMenuMusic();
            }
        }
    }
    #endregion
    #region Next Level
    private void NextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex < gamePlayLevelCount)
        {
            if (!keySpawned && GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
            {
                keySpawned = true;
                
                if (keySpawned == true)
                {
                    UIManager.instance.ShowKey(true);
                    UIManager.instance.ShowSlotMachine(true);
                }
               
                if (door != null && changeDoor.Length > 1)
                    door.sprite = changeDoor[1];
                
                if (keyIconOn == false)
                {
                    if (UIManager.instance != null)
                    {
                        UIManager.instance.ShowKeyIcon(false);
                    }
                }
            }
        }
    }
    public void HasKey()
    {
        UIManager.instance.ShowKeyIcon(true);
        UIManager.instance.ShowKey(false);
    }
    #endregion
    public void RestartGameState()
    {
        Time.timeScale = 0;
        timeElapsed = 0;
        keySpawned = false;
        keyIconOn = false;
    }
}