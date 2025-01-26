// using UnityEngine;
// using UnityEngine.InputSystem;
//
// public class PlayerController : MonoBehaviour
// {
//     #region Input Variables
//     public bool IsEnabled
//     {
//         get => input.asset.enabled;
//         set {
//             if (value) input.Enable();
//             else input.Disable();
//         }
//     }
//     #endregion
//     
//     #region Horizontal Movement Variables
//     [Header("-----Horizontal Movement-----")]
//     [SerializeField] private float playerMaxSpeed = 5f;
//     [SerializeField] private float playerMaxAcceleration = 40f;
//     private Vector2 moveInput, targetVelocity;
//     private float maxSpeedChange;
//     private float movingPlatformSpeed;
//     #endregion
//     
//     #region Jump Variables
//     [Header("-----Jumping-----")]
//     [SerializeField] private float jumpHeight = 7f;
//     [SerializeField] private float jumpMultiplier = 1.5f;
//     [SerializeField] private float fallMultiplier = 3f;
//
//     [SerializeField] private float maxFallSpeed = 7f;
//     [SerializeField] private float coyoteTime = 0.3f;
//     [SerializeField] private float jumpBufferTime = 0.2f;
//
//     [Header("-----Wall Jump/Wall Slide-----")]
//     [SerializeField] bool useWallJumps;
//     [SerializeField] bool useWallSlide;
//     [SerializeField] float wallSlideSpeed;
//     [SerializeField] private float wallJumpBufferTime = 0.2f;
//     [SerializeField] private Vector2 wallJumpSpeed;
//
//     private bool isWallSliding;
//     private bool wasPreviouslyGrounded = false;
//
//     private float jumpSpeed;
//     private float gravityScale = 1f;
//     private float coyoteCounter;
//     private float jumpBufferCounter;
//     private float wallJumpBufferCounter;
//
//     private int jumpCount = 1;
//     #endregion
//
//     #region Other Variables
//     
//     [Header("-----Game Object References-----")]
//     PlayerInput input;
//     private Rigidbody2D rb;
//     private Transform groundCheck;
//     private Transform wallCheck;
//
//     [Header("-----Layer References-----")]
//     private LayerMask groundLayer;
//     private LayerMask wallLayer;
//     private LayerMask movingPlatformLayer;
//     
//     [Header("-----Script References-----")]
//     private PlayerAbilityActivator abilityActivatorScript;
//     #endregion
//
//     #region Initialization
//     private void Awake()
//     {
//         input = new PlayerInput();
//         input.Player.Jump.started += ctx => PressJump(ctx);
//         input.Player.Jump.canceled += ctx => ReleasedJump(ctx);
//     }
//     
//     private void OnEnable()
//     {
//         IsEnabled = true;
//     }
//
//     private void OnDisable()
//     {
//         IsEnabled = false;
//     }
//     
//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//
//         groundCheck = transform.Find("GroundCheck").transform;
//         wallCheck = transform.Find("WallCheck").transform;
//         abilityActivatorScript = GetComponent<PlayerAbilityActivator>();
//
//         groundLayer = LayerMask.GetMask("Ground");
//         wallLayer = LayerMask.GetMask("Wall");
//         movingPlatformLayer = LayerMask.GetMask("Moving Platform");
//     }
//     #endregion
//
//     private void Update()
//     {
//         #region Target Velocity
//         targetVelocity = new Vector2(moveInput.x * playerMaxSpeed + movingPlatformSpeed, 0f);
//         movingPlatformSpeed = 0f;
//         JumpMovementUpdate();
//         #endregion
//     }
//
//     private void FixedUpdate()
//     {
//         HorizontalMovementUpdate();
//
//         if (coyoteCounter > 0f && jumpBufferCounter > 0f)
//         {
//             ExecuteJump();
//         }
//     }
//
//     void HorizontalMovementUpdate()
//     {
//         #region Horizontal Movement 
//         maxSpeedChange = playerMaxAcceleration * Time.deltaTime;
//         rb.linearVelocity = new Vector2(Mathf.MoveTowards(rb.linearVelocity.x, targetVelocity.x, maxSpeedChange), rb.linearVelocity.y);
//
//
//         if (rb.linearVelocity.x < 0 && moveInput.x != 0)
//         {
//             transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1f, transform.localScale.y, transform.localScale.z);
//         }
//         else if (rb.linearVelocity.x > 0 && moveInput.x != 0)
//         {
//             transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
//         }
//         #endregion
//     }
//
//     void JumpMovementUpdate()
//     {
//         #region Jump
//         jumpBufferCounter -= Time.deltaTime;
//
//         var groundCheck = GroundCheck();
//         var wallCheck = WallCheck() && useWallJumps;
//
//         if ((groundCheck || wallCheck)  && !wasPreviouslyGrounded)
//         {
//             jumpCount = 0;
//         }
//
//         wasPreviouslyGrounded = (groundCheck || wallCheck);
//
//         if (groundCheck)
//         {
//             coyoteCounter = coyoteTime;
//         }
//         else 
//         {
//             coyoteCounter -= Time.deltaTime;
//         }
//         
//         if (!abilityActivatorScript.GetStatusOrElse("isDashing", false))
//         {
//             // TODO: Review if this is where we want to apply "glideModifier"
//             if (rb.linearVelocity.y > 0)
//             {
//                 rb.gravityScale = jumpMultiplier;
//             }
//             else if (rb.linearVelocity.y < 0 && rb.linearVelocity.y > -maxFallSpeed * glideModifier)
//             {
//                 rb.gravityScale = glideModifier != 1f ? glideModifier : fallMultiplier;
//             }
//             else if (rb.linearVelocity.y == 0)
//             {
//                 rb.gravityScale = gravityScale;
//             }
//         }
//         
//         #endregion
//
//         if (useWallJumps)
//         {
//             WallSlide();
//             WallJump();
//         }
//     }
//     
//     #region Jump Methods
//     void PressJump(InputAction.CallbackContext ctx)
//     {
//
//         jumpBufferCounter = jumpBufferTime;
//
//         if (jumpCount <= maxAirJumpsModifier && !WallCheck())
//         {
//
//             ExecuteJump();
//         }
//
//         if (wallJumpBufferCounter > 0f)
//         {
//             Vector2 speed = new Vector2(Mathf.Sign(transform.localScale.x) * -wallJumpSpeed.x, wallJumpSpeed.y);
//             WallJump(speed);
//             wallJumpBufferCounter = 0f;
//         }
//     }
//
//
//     void ReleasedJump(InputAction.CallbackContext ctx)
//     {
//         if (rb.linearVelocity.y > 0f)
//         {
//             coyoteCounter = 0f;
//             rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
//         }
//     }
//
//     void ExecuteJump()
//     {
//         rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
//         jumpBufferCounter = 0f;
//         jumpCount += 1;
//     }
//
//     void WallJump(Vector2 speed)
//     {
//         rb.linearVelocity = speed;
//         jumpBufferCounter = 0f;
//         jumpCount += 1;
//     }
//     #endregion
//     
//     #region Event Handlers
//     void OnMove(InputValue input)
//     {
//         moveInput = input.Get<Vector2>();
//     }
//     #endregion
//
//     #region External Methods
//     public void SetTargetVelocity(float x)
//     {
//         movingPlatformSpeed = x;
//     }
//     #endregion
// }