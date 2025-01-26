using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilityActivator : MonoBehaviour
{
        private Rigidbody2D rb;
        PlayerInput input;
        
        private PowerUpScriptableObject powerUp;
        
        // private InputAction currInputBinding; // Held within the power definition (This would be equal to: "input.Player.Dash")
        private Action<InputAction.CallbackContext> abilityCallback;

        private Dictionary<string, bool> statuses;

        public void Awake()
        {
                input = new PlayerInput();
                rb = GetComponent<Rigidbody2D>();
                
                statuses.Add("isDashing", false);
                
                
                // abilityCallback = ctx => StartCoroutine(Dash());
        }

        public void ApplyAbility(PowerUpScriptableObject newPowerUp)
        {
                // Clean up input event handling
                powerUp.getInputBinding(input).started -= abilityCallback;
                
                // New subscription
                abilityCallback = ctx => StartCoroutine(newPowerUp.activateAbility(transform, rb));
                newPowerUp.getInputBinding(input).started += abilityCallback;
                
                // Keep for next clean up
                powerUp = newPowerUp;
        }
}