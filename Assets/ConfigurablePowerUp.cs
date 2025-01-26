using UnityEngine;

public class ConfigurablePowerUp : MonoBehaviour
{
    public PowerUpScriptableObject data;

    private void OnTriggerEnter2D(Collider2D other)
    {
        data.Apply(other.gameObject);
        GameObject.Destroy(gameObject);
    }
}
