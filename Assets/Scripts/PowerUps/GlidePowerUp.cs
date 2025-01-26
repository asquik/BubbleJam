using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "GlidePowerUp", menuName = "Scriptable Objects/GlidePowerUp")]
public class GlidePowerUp : ActionPowerUpScriptableObject
{
    [Header("-----Glide-----")]
    [SerializeField] private float glideGravity;
    private bool isGliding;

    public override InputAction GetInputBinding(PlayerInput input)
    {
        return input.Player.Jump;
    }

    public override IEnumerator ActivateAbility(PlayerAbilityActivator activator, Transform transform, Rigidbody2D rb)
    {
        if (isGliding) yield break;

        // Will fix the high computational cost later
        var playerJump = activator.gameObject.GetComponent<PlayerJump>();
        playerJump.glideModifier = glideGravity;
        
        isGliding = true;
    }
    
    public override void DeactivateAbility(GameObject player)
    {
        var playerJump = player.GetComponent<PlayerJump>();
        playerJump.glideModifier = 1;
        
        isGliding = false;
    }
}