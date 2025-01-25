using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    #region Jump Variables
    [Header("-----Jump-----")]
    [SerializeField] private float jumpHeight = 7f;
    [SerializeField] private float jumpMultiplier = 1.5f;
    [SerializeField] private float fallMultiplier = 3f;

    [SerializeField] private float maxFallSpeed = 7f;
    [SerializeField] private float coyoteTime = 0.3f;
    [SerializeField] private float jumpBufferTime = 0.2f;

    [SerializeField] private bool useAirJumps;
    [SerializeField] private int maxAirJumps;


    [Header("-----Wall Jump/Wall Slide-----")]
    [SerializeField] bool useWallJumps;
    [SerializeField] bool useWallSlide;
    [SerializeField] float wallSlideSpeed;
    [SerializeField] private float wallJumpBufferTime = 0.2f;
    [SerializeField] private Vector2 wallJumpSpeed;


    private bool isWallSliding;
    private bool wasPreviouslyGrounded = false;

    private float jumpSpeed;
    private float gravityScale = 1f;
    private float coyoteCounter;
    private float jumpBufferCounter;
    private float wallJumpBufferCounter;

    private int jumpCount = 1;
    #endregion

    #region Other Variables
    PlayerInput input;

    private LayerMask groundLayer;
    private LayerMask wallLayer;
    private LayerMask movingPlatformLayer;
    private Transform groundCheck;
    private Transform wallCheck;
    private Rigidbody2D rb;
    private PlayerHorizontalMovement horizontalMovementScript;
    #endregion

    #region Initialization
    private void Awake()
    {
        input = new PlayerInput();
        input.Player.Jump.started += ctx => PressJump(ctx);
        input.Player.Jump.canceled += ctx => ReleasedJump(ctx);

    }
    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        groundCheck = transform.Find("GroundCheck").transform;
        wallCheck = transform.Find("WallCheck").transform;
        horizontalMovementScript = GetComponent<PlayerHorizontalMovement>();

        groundLayer = LayerMask.GetMask("Ground");
        wallLayer = LayerMask.GetMask("Wall");
        movingPlatformLayer = LayerMask.GetMask("Moving Platform");

    }
    #endregion


    void Update()
    {

        #region Jump
        jumpBufferCounter -= Time.deltaTime;

        var groundCheck = GroundCheck();
        var wallCheck = WallCheck() && useWallJumps;

        if ((groundCheck || wallCheck)  && !wasPreviouslyGrounded)
        {
            Debug.Log("We are in the if statement");
            jumpCount = 0;
        }

        wasPreviouslyGrounded = (groundCheck || wallCheck);

        if (groundCheck)
        {
            coyoteCounter = coyoteTime;
        }
        else 
        {
            coyoteCounter -= Time.deltaTime;
        }

        if (!horizontalMovementScript.GetIsDashing())
        {
            if (rb.linearVelocity.y > 0)
            {
                rb.gravityScale = jumpMultiplier;
            }
            else if (rb.linearVelocity.y < 0 && rb.linearVelocity.y > -maxFallSpeed)
            {
                rb.gravityScale = fallMultiplier;
            }
            else if (rb.linearVelocity.y == 0)
            {
                rb.gravityScale = gravityScale;
            }
        }
        

        #endregion

        if (useWallJumps)
        {
            WallSlide();
            WallJump();
        }
    }


    void FixedUpdate()
    {
        if (coyoteCounter > 0f && jumpBufferCounter > 0f)
        {
            ExecuteJump();
        }
    }

    #region Jump Methods
    void PressJump(InputAction.CallbackContext ctx)
    {

        jumpBufferCounter = jumpBufferTime;

        if (jumpCount <= maxAirJumps && useAirJumps && !WallCheck())
        {

            ExecuteJump();
        }

        if (wallJumpBufferCounter > 0f)
        {
            Vector2 speed = new Vector2(Mathf.Sign(transform.localScale.x) * -wallJumpSpeed.x, wallJumpSpeed.y);
            WallJump(speed);
            wallJumpBufferCounter = 0f;
        }
    }


    void ReleasedJump(InputAction.CallbackContext ctx)
    {
        if (rb.linearVelocity.y > 0f)
        {
            coyoteCounter = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

    }

    void ExecuteJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
        jumpBufferCounter = 0f;
        jumpCount += 1;
    }

    void WallJump(Vector2 speed)
    {
        rb.linearVelocity = speed;
        jumpBufferCounter = 0f;
        jumpCount += 1;
    }
    #endregion

    #region Wall Sliding/Jumping Methods
    void WallSlide()
    {

        if (WallCheck() && !GroundCheck() && useWallSlide)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlideSpeed, float.MaxValue));

        }
        else
        {
            isWallSliding = false;
        }
    }

    void WallJump()
    {
        if (isWallSliding)
        {
            
            wallJumpBufferCounter = wallJumpBufferTime;
            
        }
        else
        {
            wallJumpBufferCounter -= Time.deltaTime;
        }
    }
    #endregion

    #region Checks
    bool GroundCheck()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer) || useWallJumps && Physics2D.OverlapCircle(groundCheck.position, 0.2f, wallLayer) || Physics2D.OverlapCircle(groundCheck.position, 0.2f, movingPlatformLayer);
    }

    bool WallCheck()   
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.4f, wallLayer);
    }
    #endregion
}
