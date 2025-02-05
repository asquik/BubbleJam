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

    [Header("-----Wall Jump/Wall Slide-----")]
    [SerializeField] bool useWallJumps;
    [SerializeField] bool useWallSlide;
    [SerializeField] float wallSlideSpeed; // Normal: 0.2f, No sliding: -0.6f (I suspect the negative value is to counteract the gravity multiplier)
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
    private int groundLayerMask;
    private int wallLayerMask;
    private Transform groundTransform;
    private Transform wallTransform;
    private Rigidbody2D rb;
    private PlayerAbilityActivator abilityActivatorScript;
    #endregion
    
    // Properties that are affected by power ups
    [Header("-----Modifier Properties-----")]
    [SerializeField] public int maxAirJumpsModifier;
    [SerializeField] public float glideModifier;

    #region Initialization

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        groundTransform = transform.Find("GroundCheck").transform;
        wallTransform = transform.Find("WallCheck").transform;
        abilityActivatorScript = GetComponent<PlayerAbilityActivator>();

        // Defining the layer masks for jumping & wall jumping mechanics
        var groundMask = 1 << LayerMask.NameToLayer("Ground");
        var wallMask = 1 << LayerMask.NameToLayer("Wall");
        var movingPlatformMask = 1 << LayerMask.NameToLayer("Moving Platform");
        groundLayerMask = groundMask + wallMask + movingPlatformMask;
        wallLayerMask = wallMask;
    }
    #endregion


    void Update()
    {
        #region Jump
        jumpBufferCounter -= Time.deltaTime;

        var grounded = IsGrounded;
        var attachedToWall = IsAttachedToWall && useWallJumps;

        if ((grounded || attachedToWall)  && !wasPreviouslyGrounded)
        {
            jumpCount = 0;
        }

        wasPreviouslyGrounded = (grounded || attachedToWall);

        if (grounded)
        {
            coyoteCounter = coyoteTime;
        }
        else if (coyoteCounter > 0f)
        {
            coyoteCounter -= Time.deltaTime;
        }
        
        if (!abilityActivatorScript.GetStatusOrElse("isDashing", false))
        {
            // TODO: Review if this is where we want to apply "glideModifier"
            if (rb.linearVelocity.y > 0)
            {
                rb.gravityScale = jumpMultiplier;
            }
            else if (rb.linearVelocity.y < 0 && rb.linearVelocity.y > -maxFallSpeed * glideModifier)
            {
                rb.gravityScale = glideModifier != 1f ? glideModifier : fallMultiplier;
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
        // CoyoteTime doesn't work as intended (It doesn't prevent 
        // jumping after being airborne for the specified amount of time)
        if (coyoteCounter > 0f && jumpBufferCounter > 0f)
        {
            ExecuteJump();
        }
    }

    void OnJump(InputValue input)
    {
        if (input.isPressed)
        {
            PressJump();
        }
        else
        {
            ReleasedJump();
        }
    }

    #region Jump Methods
    void PressJump()
    {
        jumpBufferCounter = jumpBufferTime;

        if (jumpCount <= maxAirJumpsModifier && !IsAttachedToWall)
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


    void ReleasedJump()
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

        if (IsAttachedToWall && !IsGrounded && useWallSlide)
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

    public bool IsGrounded => Physics2D.OverlapCircle(groundTransform.position, 0.2f, groundLayerMask);

    private bool IsAttachedToWall => Physics2D.OverlapCircle(wallTransform.position, 0.4f, wallLayerMask);
    #endregion
    
    #region Modifiers
    public void ResetModifiers()
    {
        maxAirJumpsModifier = 0;
        glideModifier = 1f;
    }
    #endregion
}
