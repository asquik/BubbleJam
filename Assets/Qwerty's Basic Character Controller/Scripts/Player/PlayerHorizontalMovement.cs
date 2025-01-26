using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerHorizontalMovement : MonoBehaviour
{
    #region Variables
    [Header("-----Horizontal Movement-----")]
    [SerializeField] private float playerMaxSpeed = 5f;
    [SerializeField] private float playerMaxAcceleration = 40f;
    private Vector2 moveInput, targetVelocity;
    private float maxSpeedChange;
    private float movingPlatformSpeed;

    [Header("-----Dash-----")]
    [SerializeField] private bool useDash;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashTime;
    private bool canDash = true;
    private bool isDashing = false;


    private Rigidbody2D rb;
    PlayerInput input;
    #endregion

    private void Awake()
    {
        input = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();


        input.Player.Dash.performed += ctx => StartCoroutine(Dash());

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

    #region Dash
    private IEnumerator Dash()
    {
        if (canDash && useDash)
        {
            canDash = false;
            isDashing = true;
            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x + Mathf.Sign(transform.localScale.x) * dashSpeed, 0f);
            yield return new WaitForSeconds(dashTime);
            rb.gravityScale = originalGravity;
            isDashing = false;
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }
        else
        {
            yield break;
        }
    }
    #endregion

    #region External Methods
    public void SetTargetVelocity(float x)
    {
        movingPlatformSpeed = x;
    }

    public bool GetIsDashing()
    {
        return isDashing;
    }

    public void SetNewDashData(DashData dashData)
    {
        useDash = dashData.useDash;
        dashSpeed = dashData.dashSpeed;
        dashCooldown = dashData.dashCooldown;
        dashTime = dashData.dashTime;
    }
    #endregion
}