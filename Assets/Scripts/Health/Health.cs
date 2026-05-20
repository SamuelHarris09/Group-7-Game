using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public float maxHealth = 100f;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject deathMenu;
    public float currentHealth;
    private float bossTakeHit;

    EnemyDamageDealer enemyDamageDealer;
    PlayerMovement playerMovement;
    BossMovement bossMovement;
    HealthBar healthBar;
    bool isDead = false;

    private void Start()
    {
        healthBar = FindFirstObjectByType<HealthBar>();
        bossMovement = FindFirstObjectByType<BossMovement>();
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

        if (other.CompareTag("BossHand"))
        {
            if (bossTakeHit > 4f)
            {
                Die();
                bossMovement.StopAllCoroutines();
            }
            else if (bossTakeHit < 4f)
            {
                bossTakeHit++;
            }
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