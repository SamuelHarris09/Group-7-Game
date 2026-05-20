using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    [SerializeField] int enemyDamage = 5;

    public int GetDamage()
    {
        return enemyDamage;
    }
}