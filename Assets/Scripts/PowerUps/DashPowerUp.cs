using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "DashPowerUp", menuName = "Scriptable Objects/DashPowerUp")]
public class DashPowerUp : ActionPowerUpScriptableObject
{
    [Header("-----Dash-----")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashTime;
    private bool canDash = true;
    
    private IEnumerator Dash(PlayerAbilityActivator activator, Transform transform, Rigidbody2D rb)
    {
        if (!canDash) yield break;
        
        canDash = false;
        activator.SetStatus("isDashing", true);
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x + Mathf.Sign(transform.localScale.x) * dashSpeed, 0f);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = originalGravity;
        activator.SetStatus("isDashing", false);
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public override InputAction GetInputBinding(PlayerInput input)
    {
        return input.Player.Dash;
    }

    public override IEnumerator ActivateAbility(PlayerAbilityActivator activator, Transform transform, Rigidbody2D rb)
    {
        return Dash(activator, transform, rb);
    }

    public override void DeactivateAbility(GameObject player)
    {
        // Do nothing (When "DeactivateAbility" was
        //  virtual, it just wouldn't get called at all)
    }
}