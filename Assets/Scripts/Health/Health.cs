using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public float maxHealth = 100f;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject deathMenu;
    public float currentHealth;
    
    EnemyDamageDealer enemyDamageDealer;
    PlayerMovement playerMovement;
    HealthBar healthBar;
    bool isDead = false;

    private void Start()
    {
        healthBar = FindFirstObjectByType<HealthBar>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        enemyDamageDealer = FindFirstObjectByType<EnemyDamageDealer>();

        if (deathMenu != null)
        {
            deathMenu.SetActive(false);
        }

        spriteRenderer.enabled = true;

        currentHealth = maxHealth;

        Debug.Log(currentHealth);
    }
    #region Damage
    private void OnTriggerEnter2D(Collider2D other)
    {
        enemyDamageDealer = other.GetComponent<EnemyDamageDealer>();

        if (enemyDamageDealer != null)
        { 
           TakeDamage(enemyDamageDealer.GetDamage());
        } 
    }

    void TakeDamage(int enemyDamage)
    {
        currentHealth -= enemyDamage;

        healthBar.UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        playerMovement.Death();
        spriteRenderer.enabled = false;

        if (deathMenu != null)
            deathMenu.SetActive(true);
    }
    #endregion
}