using UnityEngine;

public class InputDebugScript : MonoBehaviour
{
    PlayerActionInput input;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        var input = new PlayerActionInput();
        var smth = input.Player.Get();
        smth.actionTriggered += (x) => Debug.Log(x);
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
