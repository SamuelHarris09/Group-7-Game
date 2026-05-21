using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public float maxHealth = 100f;
    [SerializeField] private int bossDamage = 10;
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
        BossDamage hand = other.GetComponent<BossDamage>();

        if (hand != null)
        {
            TakeDamage(hand.damage);
        }

        EnemyDamageDealer enemyDamageDealer = other.GetComponent<EnemyDamageDealer>();

        if (enemyDamageDealer != null)
        { 
            TakeDamage(enemyDamageDealer.GetDamage());
            Debug.Log(currentHealth);
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

        Time.timeScale = 0f;

        playerMovement.Death();
        spriteRenderer.enabled = false;

        if (deathMenu != null)
            deathMenu.SetActive(true);
    }
    #endregion
}