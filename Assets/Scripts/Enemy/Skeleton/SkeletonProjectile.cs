using System.Collections;
using UnityEngine;

public class SkeletonProjectile : MonoBehaviour
{
    [Header("Base Variables")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float projectileLifetime = 5f;

    [SerializeField] private float baseFireRate = 0.2f;
    [SerializeField] private float minimumFireRate = 0.2f;
    [SerializeField] private float fireRateVariance = 0f;

    [HideInInspector] public bool isFiring;
    private float rightSpeed;
    private float leftSpeed;

    private Coroutine fireCoroutine;

    private SkeletonAI skeletonAI;

    private void Awake()
    {
        skeletonAI = GetComponent<SkeletonAI>();

        rightSpeed = projectileSpeed * 1;
        leftSpeed = projectileSpeed * -1;
    }

    private void Update()
    {
        Fire();
        Direction();
    }

    private void Direction()
    {
        if (skeletonAI.isFacingRight)
        {
            projectileSpeed = rightSpeed;
        }
    }

    private void Fire()
    {
        if (isFiring && fireCoroutine == null)
        {
            fireCoroutine = StartCoroutine(FireContinuously());
        }
        else if (!isFiring && fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }
    }

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            Rigidbody2D projectileRB = projectile.GetComponent<Rigidbody2D>();
            projectileRB.linearVelocityX = projectileSpeed;

            Destroy(projectile, projectileLifetime);

            float waitTime = Random.Range(
                baseFireRate - fireRateVariance,
                baseFireRate + fireRateVariance);

            waitTime = Mathf.Clamp(waitTime, minimumFireRate, float.MaxValue);

            yield return new WaitForSeconds(waitTime);
        }
    }
}