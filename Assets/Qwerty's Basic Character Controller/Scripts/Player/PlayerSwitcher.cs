using UnityEngine;

public class PlayerSwitcher : MonoBehaviour
{
    // public CameraScript camera;
    [SerializeField] private PlayerControllerV2 player1Controls;
    [SerializeField] private PlayerControllerV2 player2Controls;
    
    private GlobalPlayerInput input;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        var cameraScript = FindFirstObjectByType<CameraScript>();
        Debug.Log("Camera: " + cameraScript);

        input.Player.SwitchPlayer.performed += ctx => SwitchPlayer();
    }
    
    void Start()
    {
        player2Controls.enabled = false;
    }

    void SwitchPlayer()
    {
        if (player1Controls.IsEnabled)
        {
            player1Controls.IsEnabled = false;
            player2Controls.IsEnabled = true;
        }
        else
        {
            player1Controls.IsEnabled = true;
            player2Controls.IsEnabled = false;
        }
    }
}
