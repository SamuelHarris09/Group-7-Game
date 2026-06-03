using UnityEngine;

public class ProjectileDestroy : MonoBehaviour
{
    private readonly float enemyProjectilePrefabDelay = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        int layerIndex = LayerMask.NameToLayer("Wall");

        if (other.gameObject.layer == layerIndex)
        {
            Destroy(other.gameObject, enemyProjectilePrefabDelay);

            Debug.Log("Destroy");
        }
    }
}