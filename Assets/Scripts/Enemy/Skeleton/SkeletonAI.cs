using Unity.VisualScripting;
using UnityEngine;

public class SkeletonAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform player;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float agroRange = 5f;
    [SerializeField] float runRange = 2.5f;

    [HideInInspector] public bool isFacingRight;

    private Rigidbody2D rb;
    private SkeletonProjectile projectile;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        projectile = GetComponent<SkeletonProjectile>();

        isFacingRight = true;
    }

    private void Update()
    {
        if (player == null)
            return;

        HandleAI();

        if (isFacingRight)
        {
            moveSpeed = rightSpeed;
        }
        if (isFacingLeft)
        {
            moveSpeed = leftSpeed;
        }
    }

    private void HandleAI()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < runRange)
        {
            Retreat();
            projectile.isFiring = true;
        }
        else if (distance < agroRange)
        {
            Chase();
            projectile.isFiring = true;
        }
        else
        {
            Idle();
            projectile.isFiring = false;
        }

        FacePlayer();
    }

    void Chanse()
    {

    }

    private void Run()
    {
        Vector2 directionAway = (transform.position - player.position).normalized;

        transform.position += (Vector3)(directionAway * moveSpeed * Time.deltaTime);

        if (directionAway.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void Aggro()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        transform.position = (Vector3)(direction * moveSpeed * Time.deltaTime);

        //rotate towards player
        if (direction.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            isFacingRight = true;
            isFacingLeft = false;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            isFacingRight = false;
            isFacingLeft = true;
        }
    }

    private void Idle()
    {

    }
}