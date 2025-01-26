using UnityEngine;
using System;
using System.Collections;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "DashPowerUp", menuName = "Scriptable Objects/DashPowerUp")]
public class DashPowerUp : ConfigPowerUpScriptableObject<DashData>
{
    [Header("-----Dash-----")]
    [SerializeField] private bool useDash;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashTime;
    private bool canDash = true;
    private bool isDashing = false;
    
    private IEnumerator Dash(Transform transform, Rigidbody2D rb)
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

    public override InputAction getInputBinding(PlayerInput input)
    {
        return input.Player.Dash;
    }

    public override IEnumerator activateAbility(Transform transform, Rigidbody2D rb)
    {
        return Dash(transform, rb);
    }
    
    // public override void Apply(GameObject player) // Maybe not required
    // {
    //     // var dashComp = player.GetComponent<PlayerHorizontalMovement>();
    //     rb = player.GetComponent<Rigidbody2D>();
    //     transform = player.GetComponent<Transform>();
    //     // dashComp.SetNewDashData(data);
    // }
}

[Serializable]
public class DashData
{
    public bool useDash;
    public float dashSpeed;
    public float dashCooldown;
    public float dashTime;
}