using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    #region Variables
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerHorizontalMovement horizontalMovementScript;
    private SpriteRenderer spriteRenderer;

    [SerializeField] bool useIdleAnimation;
    [SerializeField] bool useJumpingAnimation;
    [SerializeField] bool useRunningAnimation;
    [SerializeField] bool useFallingAnimation;
    [SerializeField] bool useDashingAnimation;
    [SerializeField] bool useWallAnimation;

    private Transform groundCheck;
    private Transform wallCheck;

    private LayerMask groundLayer;
    private LayerMask wallLayer;
    private LayerMask movingPlatformLayer;

    private Vector2 moveInput;
    #endregion


    void Start()
    {
        #region Variables
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        horizontalMovementScript = GetComponent<PlayerHorizontalMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        groundCheck = transform.Find("GroundCheck").transform;
        wallCheck = transform.Find("WallCheck").transform;

        groundLayer = LayerMask.GetMask("Ground Or Platform");
        wallLayer = LayerMask.GetMask("Wall");
        movingPlatformLayer = LayerMask.GetMask("Moving Platform");
        #endregion
}

    void Update()
    {
        #region If Statements
        if (useIdleAnimation)
        {
            Idle();
        }

        if (useJumpingAnimation)
        {
            Jumping();
        }

        if (useRunningAnimation)
        {
            Running();
        }

        if (useFallingAnimation)
        {
            Falling();
        }

        if (useDashingAnimation)
        {
            Dashing();
        }
        if (useWallAnimation)
        {
            WallSlide();
        }
        #endregion
    }

    #region Methods
    private void Idle()
    {
        if (GroundCheck() && !WallCheck() && moveInput.x == 0)
        {
            animator.SetBool("isIdle", true);

        }
        else
        {
            animator.SetBool("isIdle", false);
        }
    }

    private void Jumping()
    {
        if (!GroundCheck() && !WallCheck() && rb.linearVelocity.y > 0)
        {
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }
    }

    private void Falling()
    {
        if(!GroundCheck() && !WallCheck() && rb.linearVelocity.y < 0)
        {
            animator.SetBool("isFalling", true);
        }
        else
        {
            animator.SetBool("isFalling", false);
        }
    }



    private void Running()
    {
        if(GroundCheck() && !WallCheck() && Mathf.Abs(moveInput.x) > 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    private void Dashing()
    {

        if (horizontalMovementScript.GetIsDashing())
        {
            animator.SetBool("isDashing", true);
        }
        else
        {
            animator.SetBool("isDashing", false);
        }
    }

    private void WallSlide()
    {
        if (WallCheck() && !GroundCheck())
        {
            animator.SetBool("onWall", true);
            spriteRenderer.flipX = true;
        }
        else 
        {
            animator.SetBool("onWall", false);
            spriteRenderer.flipX = false;
        }
    }
    #endregion

    private void OnMove(InputValue input)
    {
        moveInput = input.Get<Vector2>();
    }

    #region Checks

    bool GroundCheck()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer) || Physics2D.OverlapCircle(groundCheck.position, 0.2f, wallLayer) || Physics2D.OverlapCircle(groundCheck.position, 0.2f, movingPlatformLayer);
    }

    bool WallCheck()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
    #endregion
}
