using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager instance;

    [SerializeField] private float optionsScene;

    public TMP_Dropdown dropdown;
    public Difficulty currentDifficulty;

    [HideInInspector] public float gombaHealth;
    [HideInInspector] public float skeletonHealth;
    [HideInInspector] public float batHealth;
    [HideInInspector] public int gombaDamage;
    [HideInInspector] public int skeletonDamage;
    [HideInInspector] public int batDamage;

    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == optionsScene)
        {
            if (PlayerPrefs.HasKey("Difficulty"))
            {
                int savedIndex = PlayerPrefs.GetInt("Difficulty");
                currentDifficulty = (Difficulty)savedIndex;
                dropdown.value = savedIndex;
            }
        }
        
        ApplyDifficulty();
    }

    public void SetDifficulty(int index)
    {
        currentDifficulty = (Difficulty)index;

        PlayerPrefs.SetInt("Difficulty", index);
        PlayerPrefs.Save();

        ApplyDifficulty();
    }

    void ApplyDifficulty()
    {
        switch (currentDifficulty)
        {
            case Difficulty.Easy:
                batHealth = 10f;
                batDamage = 5;
                gombaHealth = 10f;
                gombaDamage = 5;
                skeletonHealth = 2f;
                skeletonDamage = 5;
                break;

            case Difficulty.Normal:
                batHealth = 4f;
                batDamage = 4;
                gombaHealth = 10f;
                gombaDamage = 2;
                skeletonHealth = 5f;
                skeletonDamage = 10;
                break;

            case Difficulty.Hard:
                batHealth = 8f;
                batDamage = 8;
                gombaHealth = 20f;
                gombaDamage = 4;
                skeletonHealth = 10f;
                skeletonDamage = 20;
                break;
        }
    }

    // how to set the vaule in the script, which will sit under Start()
    // for example: damage = DifficultyManager.instance.gombaDamage; 
    // how to get the currect difficulty from the "PlayerPrefs.GetInt("Difficulty");"
}