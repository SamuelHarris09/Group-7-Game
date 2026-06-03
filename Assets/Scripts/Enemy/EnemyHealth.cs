using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private float currentHealth = 10f;

    private void Start()
    {
        if (CompareTag("Gommba"))
        {
            DifficultyManager.instance.gombaHealth = currentHealth;
        }
        if (CompareTag("Bat"))
        {
            DifficultyManager.instance.gombaHealth = currentHealth;
        }
        if (CompareTag("Skeleton"))
        {
            DifficultyManager.instance.skeletonHealth = currentHealth;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();

        if (damageDealer != null)
        {
            int layerIndex = LayerMask.NameToLayer("HitBox");

            if (other.gameObject.layer == layerIndex)
            {
                TakeDamage(damageDealer.GetDamage());
            }
        }
    }

    public void Die()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);

            Debug.Log("Dead");
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
}