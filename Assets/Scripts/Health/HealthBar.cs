using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] GameObject[] hearts;
    [SerializeField] GameObject[] background;
    private float playerHealthUI = 100f;

    Health playerHealth;
    
    private void Start()
    {
        playerHealth = FindFirstObjectByType<Health>();
    }

    public void UpdateHealthUI()
    {
        if (playerHealth == null)
        {
            Debug.Log("There is no health to update");
            return;
        }
            
        float health = playerHealth.currentHealth;
        if (health > 0)
        {
            Debug.Log("There is health");
        }

        float healthPerHearth = playerHealthUI / hearts.Length;
        int heartsToShow = Mathf.CeilToInt(health / healthPerHearth);


        for (int i = 0; i < hearts.Length; i++)
        {
            Debug.Log("Yippe it works");
            hearts[i].SetActive(i < health);
            background[i].SetActive(1 >= heartsToShow);
        }
    }
}