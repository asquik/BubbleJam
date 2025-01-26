using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilityActivator : MonoBehaviour
{
        private Rigidbody2D rb;
        
        private PlayerActionInput input;
        
        private PowerUpScriptableObject powerUp;
        
        private Action<InputAction.CallbackContext> activationCallback;
        private Action<InputAction.CallbackContext> deactivationCallback;

        private Dictionary<string, bool> statuses;
        
        private PlayerJump playerJumpScript;

        public bool IsEnabled
        {
                get => input.asset.enabled;
                set
                {
                        // A naughty approach
                        // to stopping an error.
                        //
                        // Will have to be fixed eventually
                        if (input == null)
                        {
                                return;
                        }
                        
                        if (value) input.Enable();
                        else input.Disable();
                }
        }
        
        public void Awake()
        {
                rb = GetComponent<Rigidbody2D>();
                playerJumpScript = GetComponent<PlayerJump>();
                
                // Not the "correct" way of getting an instance
                // of this, but I'm short on time in the game jam
                input = new PlayerActionInput();

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
                switch (powerUp)
                {
                        case ActionPowerUpScriptableObject currActionPowerUp:
                                // Clean up input event handling
                                currActionPowerUp.GetInputBinding(input).started -= activationCallback;
                                currActionPowerUp.GetInputBinding(input).canceled -= deactivationCallback;
                                break;
                        case ModifierPowerUpScriptableObject:
                                // Reset the modifiers in all applicable input handling scripts
                                playerJumpScript.ResetModifiers();
                                break;
                }
                
                switch (newPowerUp)
                {
                        case ActionPowerUpScriptableObject actionPowerUp:
                                var binding = actionPowerUp.GetInputBinding(input);
                                
                                // New subscription
                                activationCallback = ctx => { StartCoroutine(actionPowerUp.ActivateAbility(this, transform, rb)); };
                                binding.started += activationCallback;
                        
                                deactivationCallback = ctx => actionPowerUp.DeactivateAbility(gameObject);
                                binding.canceled += deactivationCallback;
                                break;
                        case ModifierPowerUpScriptableObject modifierPowerUp:
                                modifierPowerUp.Apply(gameObject);
                                break;
                }
                
                // Keep for next clean up
                powerUp = newPowerUp;
        }
}