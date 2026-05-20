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
    private float currentSpeed;

    [Header("Coyote time")]
    [SerializeField] float coyoteTime = 0.1f;
    [SerializeField] float jumpBufferTime = 0.1f;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    [Header("Fall Through")]
    [SerializeField] float fallSpeed = -10f;
    [SerializeField] float platformFallSpeed = -20f;

    [Header("Dash")]
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer trailRenderer;
    private bool canDash = true;
    private bool isDashing;

    [SerializeField] CameraController cameraController;

    bool canControlPlayer = true;

    Vector2 moveVector;
    Rigidbody2D rb;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction crouchAction;
    InputAction dashAction;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Player/Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        crouchAction = InputSystem.actions.FindAction("Crouch");
        dashAction = InputSystem.actions.FindAction("Dash");

        rb = GetComponent<Rigidbody2D>();
        cameraController = GetComponent<CameraController>();
        currentSpeed = moveSpeed;
    }

    void Update()
    {
        if (canControlPlayer == true)
        { 
            if (isDashing)
            {
                return;
            }

            Movement();
            Jump();
            JumpTimer();
            FallThrough();
            Dash();
        }
    }
    #region Movement
    void Movement()
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
        if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void FallThrough()
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
    void Jump()
    {
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            rb.linearVelocityY = jumpHeight;

            SoundManager.PlaySound(SoundType.Jump);

            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
        }
    }

    void JumpTimer()
    {
        RaycastHit2D ground = Physics2D.Raycast(transform.position, Vector2.down, jumpDistance, groundLayer);

        // Ground check
        if (ground.collider != null)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Jump buffer
        if (jumpAction.WasPressedThisFrame())
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
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
            rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        }
        else if (Keyboard.current.aKey.isPressed)
        {
            rb.linearVelocity = new Vector2(transform.localScale.x * -dashingPower, 0f);
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

    public void Death()
    {
        canControlPlayer = false;
    }

    public void Alive()
    {
        canControlPlayer = true;
    }
}