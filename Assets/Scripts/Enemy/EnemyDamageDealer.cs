using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    private readonly int enemyDamage;

    private void Start()
    {
        if (CompareTag("Gommba"))
        {
            DifficultyManager.instance.gombaDamage = enemyDamage;
        }
        if (CompareTag("Bat"))
        {
            DifficultyManager.instance.batDamage = enemyDamage;
        }
        if (CompareTag("Skeleton"))
        {
            DifficultyManager.instance.skeletonDamage = enemyDamage;
        }
    }

    public int GetDamage()
    {
        return enemyDamage;
    }
}