using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    TextMeshProUGUI timeText;
    private GameObject key;
    private GameObject keyIcon;
    private GameObject pauseScreen;

    [Header("Pause")]
    [SerializeField] public Sprite[] changeDoor;
    [SerializeField] private float gamePlayLevelCount = 5;

    private float timeElapsed = 0f;
    public int enemiesAlive;
    bool keySpawned = false;
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
        playerHealth = FindFirstObjectByType<Health>();
        pauseMenu = InputSystem.actions.FindAction("Pause Menu");
    }
    
    void Update()
    {
        TimeCounter();
        Menu();
        NextLevel();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        timeText = GameObject.FindWithTag("TimeText")?.GetComponent<TextMeshProUGUI>();
        keyIcon = GameObject.FindWithTag("KeyIcon");
        key = GameObject.FindWithTag("Key");
        pauseScreen = GameObject.FindWithTag("PauseMenu");
        door = GameObject.FindWithTag("Door")?.GetComponent<SpriteRenderer>();

        playerHealth = FindFirstObjectByType<Health>();

        keySpawned = false;
        keyIconOn = false;

        if (pauseScreen != null)
        {
            pauseScreen.SetActive(false);
        }

        if (scene.buildIndex < gamePlayLevelCount)
        {
            if (keyIcon != null)
                keyIcon.SetActive(false);

            if (key != null)
                key.SetActive(false);

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
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    } 

    void Menu()
    {
        if (pauseMenu.WasPressedThisFrame())
        {
            if (pauseScreen.activeInHierarchy)
            {
                PauseGame(false);
            }
            else
            {
                PauseGame(true);
            }
        }
    }

    public void PauseGame(bool status)
    {
        pauseScreen.SetActive(status);

        if (status)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
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

                if (key != null)
                    key.SetActive(true);

                if (door != null && changeDoor.Length > 1)
                    door.sprite = changeDoor[1];

                if (keyIconOn == true)
                {
                    if (keyIcon != null)
                        keyIcon.SetActive(false);
                }
            }
        }
    }
    public void HasKey()
    {
        if (keyIcon != null)
            keyIcon.SetActive(true);
        if (key != null)
            key.SetActive(false);
    }
    #endregion
}