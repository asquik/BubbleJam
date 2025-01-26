using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilityActivator : MonoBehaviour
{
        private Rigidbody2D rb;
        InputActionAsset input;
        
        private PowerUpScriptableObject powerUp;
        
        private Action<InputAction.CallbackContext> activationCallback;
        private Action<InputAction.CallbackContext> deactivationCallback;

        private Dictionary<string, bool> statuses;
        
        // private PlayerJump playerJumpScript;

        public void Initialize(PlayerInput playerInput)
        {
                foreach (var actionsBinding in playerInput.actions.bindings)
                {
                        Debug.Log("Action: " + actionsBinding.action + " | " + "Name: " + actionsBinding.name);
                }
        }
        
        public void Awake()
        {
                var playerInput = GetComponent<PlayerInput>();
                Initialize(playerInput);
                rb = GetComponent<Rigidbody2D>();
                // playerJumpScript = GetComponent<PlayerJump>();

                statuses = new Dictionary<string, bool>();
        }
        
        private void OnEnable()
        {
                input.Enable();
        }
        
        private void OnDisable()
        {
                input.Disable();
        }
        
        public void SetStatus(string key, bool value)
        {
                statuses[key] = value;
        }

        public bool GetStatusOrElse(string key, bool fallbackValue)
        {
                var isPresent = statuses.TryGetValue(key, out var status);
                return isPresent ? status : fallbackValue;
        }

        public void ApplyAbility(PowerUpScriptableObject newPowerUp)
        {
                // switch (powerUp)
                // {
                //         case ActionPowerUpScriptableObject currActionPowerUp:
                //                 // Clean up input event handling
                //                 currActionPowerUp.GetInputBinding(input).started -= activationCallback;
                //                 currActionPowerUp.GetInputBinding(input).canceled -= deactivationCallback;
                //                 break;
                //         case ModifierPowerUpScriptableObject:
                //                 // Reset the modifiers in all applicable input handling scripts
                //                 playerJumpScript.ResetModifiers();
                //                 break;
                // }
                //
                // switch (newPowerUp)
                // {
                //         case ActionPowerUpScriptableObject actionPowerUp:
                //                 var binding = actionPowerUp.GetInputBinding(input);
                //                 
                //                 // New subscription
                //                 activationCallback = ctx => { StartCoroutine(actionPowerUp.ActivateAbility(this, transform, rb)); };
                //                 binding.started += activationCallback;
                //         
                //                 deactivationCallback = ctx => actionPowerUp.DeactivateAbility(gameObject);
                //                 binding.canceled += deactivationCallback;
                //                 break;
                //         case ModifierPowerUpScriptableObject modifierPowerUp:
                //                 modifierPowerUp.Apply(gameObject);
                //                 break;
                // }
                //
                // // Keep for next clean up
                // powerUp = newPowerUp;
        }
}