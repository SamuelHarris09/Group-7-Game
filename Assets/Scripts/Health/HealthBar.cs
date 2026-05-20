using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] GameObject[] hearts;
    [SerializeField] GameObject[] background;
    private float playerHealthUI = 100f;

    Health playerHealth;
    
    private void Start()
    {
        playerHealth = GetComponent<Health>();
    }

    public void UpdateHealthUI()
    {
        if (playerHealth == null)
            return;

        float health = playerHealth.currentHealth;

        float healthPerHearth = playerHealthUI / hearts.Length;
        int heartsToShow = Mathf.CeilToInt(health / healthPerHearth);

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < health);
            background[i].SetActive(1 >= heartsToShow);
        }
    }
}