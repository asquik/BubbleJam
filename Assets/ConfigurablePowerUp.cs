using UnityEngine;

public class ConfigurablePowerUp : MonoBehaviour
{
    public PowerUpScriptableObject data;

    public void Start()
    {
        var sRenderer = GetComponent<SpriteRenderer>();
        sRenderer.color = data.powerUpColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var activator = other.gameObject.GetComponent<PlayerAbilityActivator>();
        activator.ApplyAbility(data);
        
        // TBD how what the happy path will be
        // GameObject.Destroy(gameObject);
    }
}
