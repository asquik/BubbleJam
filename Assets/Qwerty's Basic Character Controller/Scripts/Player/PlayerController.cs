using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables
    
    [Header("-----Game Object References-----")]
    private PlayerInput input;
    private PlayerJump jumpScript;
    private PlayerAbilityActivator abilityActivatorScript;

    // This being an indirect getter is probably a bad practice though I'm using
    // it anyway to get around a more significant refactor of the scripts.
    public bool IsGrounded => jumpScript && jumpScript.IsGrounded;
    
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
        jumpScript = GetComponent<PlayerJump>();
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