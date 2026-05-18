using UnityEngine;
using System.Collections;

public class SkeletonProjectile : MonoBehaviour
{
    [Header("Base Variables")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float leftSpeed = 10f;
    [SerializeField] float rightSpeed = 10f;
    [SerializeField] float projectileLifetime = 5f;
    [SerializeField] float baseFireRate = 0.2f;

    [Header("AI Variables")]
    [SerializeField] bool useAI = false;
    [SerializeField] float minimumFireRate = 0.2f;
    [SerializeField] float fireRateVariance = 0f;

    [HideInInspector] public bool isFiring;

    Coroutine fireCoroutine;

    float direction;

    SkeletonAI skeletonAI;

    void Start()
    {
        if (useAI)
        {
            isFiring = true;
        }
    }

    void Update()
    {
        Fire();
        Direction();
    }

    void Direction()
    {
        if (skeletonAI.GetFacingRight())
        {
            direction = leftSpeed;
        }
        else if (skeletonAI.GetFacingLeft()) 
        {
            direction = rightSpeed;
        }
    }

    void Fire()
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

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            Rigidbody2D projectileRB = projectile.GetComponent<Rigidbody2D>();
            projectileRB.linearVelocityX = direction;

            Destroy(projectile, projectileLifetime);

            float waitTime = Random.Range(
                baseFireRate - fireRateVariance,
                baseFireRate + fireRateVariance);

            waitTime = Mathf.Clamp(waitTime, minimumFireRate, float.MaxValue);

            yield return new WaitForSeconds(waitTime);
        }
    }
}