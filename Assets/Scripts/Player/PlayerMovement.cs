using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float crouchSpeed = 1f;
    [SerializeField] float jumpHeight = 10f;
    [SerializeField] float jumpDistance = 2f;
    [SerializeField] LayerMask groundLayer;
    public float currentSpeed;

    [Header("Fall Through")]
    [SerializeField] float fallSpeed = -10f;
    [SerializeField] float platformFallSpeed = -20f;

    [Header("Dash")]
    public float dashSpeed = 24f;
    public float dashDistance = 2f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer trailRenderer;
    private bool canDash = true;
    private bool isDashing;

    Animator animator;

    bool canControlPlayer = true;

    Vector2 moveVector;
    Rigidbody2D rb;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction crouchAction;
    InputAction dashAction;

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Player/Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        crouchAction = InputSystem.actions.FindAction("Crouch");
        dashAction = InputSystem.actions.FindAction("Dash");

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = moveSpeed;
    }

    private void Update()
    {
        if (canControlPlayer == true)
        {
            if (isDashing)
            {
                return;
            }

            Movement();
            Jump();
            FallThrough();
            Dash();
            Animations();
        }
    }
    #region Movement
    private void Movement()
    {
        // Movement
        moveVector = moveAction.ReadValue<Vector2>();
        rb.linearVelocityX = moveVector.x * moveSpeed;

        // Crouch
        if (crouchAction.IsPressed())
        {
            currentSpeed = crouchSpeed;
            rb.rotation = 90; // temporary
        }
        else
        {
            currentSpeed = moveSpeed;
            rb.rotation = 0; // temporary
        }

        //Rotation
        if (moveVector.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (moveVector.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void FallThrough()
    {
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            Physics2D.gravity = new Vector3(0, platformFallSpeed, 0);
        }
        else if (Keyboard.current.sKey.wasReleasedThisFrame)
        {
            Physics2D.gravity = new Vector3(0, fallSpeed, 0);
        }
    }
    #endregion
    #region Jump & Coyote time
    private void Jump()
    {
        RaycastHit2D ground = Physics2D.Raycast(transform.position, Vector2.down, jumpDistance, groundLayer);

        if (ground.collider != null)
        {
            if (jumpAction.WasPressedThisFrame())
            {
                rb.linearVelocityY = jumpHeight;

                SoundManager.PlaySound(SoundType.Jump);
            }
        }
    }
    #endregion
    #region Dash
    private IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;

        if (Keyboard.current.dKey.isPressed)
        {
            rb.linearVelocity = new Vector2(transform.localScale.x * dashSpeed, 0f);
        }
        else if (Keyboard.current.aKey.isPressed)
        {
            rb.linearVelocity = new Vector2(transform.localScale.x * -dashSpeed, 0f);
        }

        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        trailRenderer.emitting = false;

        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void Dash()
    {
        if (canDash == true)
        {
            if (dashAction.WasPressedThisFrame())
            {
                StartCoroutine(DashCoroutine());
            }
        }
    }
    #endregion
    #region Animations
    private void Animations()
    {
        if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (rb.linearVelocity.y > 0.1f)
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
        }
        else if (rb.linearVelocity.y < -0.1f)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }
    }
    #endregion
    public void Death()
    {
        canControlPlayer = false;
    }
}