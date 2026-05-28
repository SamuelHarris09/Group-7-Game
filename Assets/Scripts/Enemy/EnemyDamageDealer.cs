using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    private int enemyDamage;

    private bool gomba;
    private bool bat;

    private void Start()
    {
        if (gomba)
        {
            DifficultyManager.instance.gombaDamage = enemyDamage;
        }
        if (bat)
        {
            DifficultyManager.instance.batDamage = enemyDamage;
        }
    }

    public int GetDamage()
    {
        return enemyDamage;
    }
}