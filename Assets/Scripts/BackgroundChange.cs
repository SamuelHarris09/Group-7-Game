using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundChange : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] background;
    [SerializeField] int firstLevelValue;
    private int currentLevel;
    int currentSceneIndex;

    private void Start()
    {
        currentLevel = currentSceneIndex - firstLevelValue;
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
       
    }
}
