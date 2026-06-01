using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [Header("Next level wait time")]
    [SerializeField] float nextLevelWaitTime = 1.5f;

    private bool HasKey;

    GameManager gameManager;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        HasKey = false;
    }

    private void Update()
    {
        if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            LoadNextScene();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Key") && HasKey == false)
        {
            HasKey = true;
            gameManager.HasKey();
        }
        else if (other.CompareTag("Door") && HasKey == true)
        {
            HasKey = false;
            gameManager.keyIconOn = false;
            StartCoroutine(SceneDelay());
        }
    }

    void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    IEnumerator SceneDelay()
    {
        yield return new WaitForSeconds(nextLevelWaitTime);
        LoadNextScene();
    }
}