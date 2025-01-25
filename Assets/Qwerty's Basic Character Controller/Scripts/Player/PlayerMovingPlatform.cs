using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovingPlatform : MonoBehaviour
{
    #region Variables
    private Rigidbody2D currentPlatformRb;
    private Vector2 platformVelocity;
    private Transform groundCheck;

    private LayerMask movingPlatformLayer;

    PlayerHorizontalMovement horizontalMovementScript;
    #endregion

    private void Start()
    {
        groundCheck = transform.Find("GroundCheck");
        horizontalMovementScript = GetComponent<PlayerHorizontalMovement>();

        movingPlatformLayer = LayerMask.GetMask("Moving Platform");
    }

    void Update()
    {
        if (currentPlatformRb != null && PlatformCheck())
        {
            horizontalMovementScript.SetTargetVelocity(platformVelocity.x);
        }
    }

    #region OnCollision Methods

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (LayerMask.GetMask(LayerMask.LayerToName(other.gameObject.layer)) == movingPlatformLayer)
        {
            currentPlatformRb = other.gameObject.GetComponent<Rigidbody2D>();
            platformVelocity = currentPlatformRb.linearVelocity;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (LayerMask.GetMask(LayerMask.LayerToName(other.gameObject.layer)) == movingPlatformLayer && PlatformCheck())
        {
            platformVelocity = currentPlatformRb.linearVelocity;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (LayerMask.GetMask(LayerMask.LayerToName(other.gameObject.layer)) == movingPlatformLayer)
        {
            currentPlatformRb = null;
            platformVelocity = Vector2.zero;
        }

    }
    #endregion

    bool PlatformCheck()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, movingPlatformLayer);
    }
}
