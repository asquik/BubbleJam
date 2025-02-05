using UnityEngine;

public class PlayerSwitcher : MonoBehaviour
{
    private CameraScript cameraScript;
    [SerializeField] private PlayerController player1Controls;
    [SerializeField] private PlayerController player2Controls;
    
    private GlobalPlayerInput input;
    
    private int playerLayer;
    private int groundLayer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        cameraScript = FindFirstObjectByType<CameraScript>();
        
        input = new GlobalPlayerInput();
        input.Player.SwitchPlayer.performed += ctx => SwitchPlayer();
        
        playerLayer = LayerMask.NameToLayer("Player");
        groundLayer = LayerMask.NameToLayer("Ground");
    }
    
    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
    
    private void Start()
    {
        setPlayerEnabled(player2Controls, false);
    }

    // This current implementation is incomplete. It currently allows
    // the player to "freeze" the inactive character in mid air.
    private void setPlayerEnabled(PlayerController controls, bool isEnabled)
    {
        var playerObject = controls.gameObject;

        controls.IsEnabled = isEnabled;
        
        // Turns the inactive player into a ground object
        // so the active player can jump off of them
        var layer = isEnabled ? playerLayer : groundLayer;
        playerObject.layer = layer;

        // Turns off the dynamic rigidbody calculations of the inactive player so
        // the active player doesn't "push" them while they are standing on top
        var bodyType = isEnabled ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
        playerObject.GetComponent<Rigidbody2D>().bodyType = bodyType;
    }

    void SwitchPlayer()
    {
        if (player1Controls.IsEnabled)
        {
            if (!player1Controls.IsGrounded) return;
            
            setPlayerEnabled(player1Controls, false);
            setPlayerEnabled(player2Controls, true);
            cameraScript.player = player2Controls.gameObject;
        }
        else
        {
            if (!player2Controls.IsGrounded) return;
            
            setPlayerEnabled(player1Controls, true);
            setPlayerEnabled(player2Controls, false);
            cameraScript.player = player1Controls.gameObject;
        }
    }
}
