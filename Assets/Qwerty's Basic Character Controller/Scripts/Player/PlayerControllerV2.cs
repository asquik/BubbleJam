using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerV2 : MonoBehaviour
{
    #region Variables
    
    [Header("-----Game Object References-----")]
    private PlayerInput input;
    // private PlayerHorizontalMovement horizontalMovementScript;
    private PlayerJump jumpScript;
    
    public bool IsEnabled
    {
        get => input.enabled;
        set => input.enabled = value;
    }
    #endregion
    
    #region Initialization
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        // horizontalMovementScript = GetComponent<PlayerHorizontalMovement>();
        // jumpScript = GetComponent<PlayerJump>();

        // horizontalMovementScript.Initialize(input);
        // jumpScript.Initialize(input);
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
}