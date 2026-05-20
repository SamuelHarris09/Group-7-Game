using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float currentHealth = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();

        if (damageDealer != null)
        {
            int layerIndexC = LayerMask.NameToLayer("HitBox");

            if (other.gameObject.layer == layerIndexC)
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

            GameManager.instance.enemiesAlive--;
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
}