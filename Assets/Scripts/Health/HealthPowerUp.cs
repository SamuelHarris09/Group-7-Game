using UnityEngine;

public class HealthPowerUp : MonoBehaviour
{
    private float powerupValue = 10f;

    private Health playerHealth;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerHealth.currentHealth += powerupValue;
            Destroy(gameObject);
        }
    }
}