using UnityEngine;

public class BossDamage : MonoBehaviour
{
    public int damage;

    Health playerHealth;

    private void Start()
    {
        playerHealth = FindFirstObjectByType<Health>();

        damage = (int)playerHealth.maxHealth / 10;
    }
}