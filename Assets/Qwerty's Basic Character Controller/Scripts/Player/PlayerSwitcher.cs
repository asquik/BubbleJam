using UnityEngine;

public class PlayerSwitcher : MonoBehaviour
{
    private CameraScript cameraScript;
    [SerializeField] private PlayerController player1Controls;
    [SerializeField] private PlayerController player2Controls;
    
    private GlobalPlayerInput input;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        cameraScript = FindFirstObjectByType<CameraScript>();
        
        input = new GlobalPlayerInput();
        input.Player.SwitchPlayer.performed += ctx => SwitchPlayer();
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
        player2Controls.enabled = false;
    }

    void SwitchPlayer()
    {
        if (player1Controls.IsEnabled)
        {
            player1Controls.IsEnabled = false;
            player2Controls.IsEnabled = true;
            cameraScript.player = player2Controls.gameObject;
        }
        else
        {
            player1Controls.IsEnabled = true;
            player2Controls.IsEnabled = false;
            cameraScript.player = player1Controls.gameObject;
        }
    }
}
