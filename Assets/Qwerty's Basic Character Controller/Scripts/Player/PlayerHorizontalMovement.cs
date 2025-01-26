using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHorizontalMovement : MonoBehaviour
{
    #region Variables
    [Header("-----Horizontal Movement-----")]
    [SerializeField] private float playerMaxSpeed = 5f;
    [SerializeField] private float playerMaxAcceleration = 40f;
    private Vector2 moveInput, targetVelocity;
    private float maxSpeedChange;
    private float movingPlatformSpeed;

    private Rigidbody2D rb;
    PlayerInput input;
    #endregion

    private void Awake()
    {
        input = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        input.Enable();
    }

    private void Update()
    {
        #region Target Velocity
        targetVelocity = new Vector2(moveInput.x * playerMaxSpeed + movingPlatformSpeed, 0f);
        movingPlatformSpeed = 0f;
        #endregion
    }

    private void FixedUpdate()
    {
        #region Horizontal Movement 
        maxSpeedChange = playerMaxAcceleration * Time.deltaTime;
        rb.linearVelocity = new Vector2(Mathf.MoveTowards(rb.linearVelocity.x, targetVelocity.x, maxSpeedChange), rb.linearVelocity.y);


        if (rb.linearVelocity.x < 0 && moveInput.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1f, transform.localScale.y, transform.localScale.z);
        }
        else if (rb.linearVelocity.x > 0 && moveInput.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        #endregion
    }

    void OnMove(InputValue input)
    {
        moveInput = input.Get<Vector2>();
    }

    #region External Methods
    public void SetTargetVelocity(float x)
    {
        movingPlatformSpeed = x;
    }
    #endregion
}