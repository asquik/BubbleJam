using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables
    
    [Header("-----Game Object References-----")]
    private PlayerInput input;
    // private PlayerHorizontalMovement horizontalMovementScript;
    private PlayerJump jumpScript;
    private PlayerAbilityActivator abilityActivatorScript;
    
    public bool IsEnabled
    {
        get => input.enabled || abilityActivatorScript.IsEnabled;
        set {
            input.enabled = value;
            ToggleAbilityInputs(value);
        }
    }
    #endregion
    
    #region Initialization
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        abilityActivatorScript = GetComponent<PlayerAbilityActivator>();
    }
    
    private void OnEnable()
    {
        IsEnabled = true;
    }

    private void OnDisable()
    {
        IsEnabled = false;
    }
    #endregion

    private void ToggleAbilityInputs(bool isEnabled)
    {
        abilityActivatorScript.IsEnabled = isEnabled;
    }
}