using UnityEngine;
using System.Collections;

public class ConfigurablePowerUp : MonoBehaviour
{
    public PowerUpScriptableObject data;
    public SpriteRenderer spriteRenderer;
    public Collider2D collider;

    public void Start()
    {
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = data.sprite;
    }

    private void toggleActive(bool isActive)
    {
        spriteRenderer.enabled = isActive;
        collider.enabled = isActive;
    }
    
    private IEnumerator tempHide()
    {
        toggleActive(false);
        yield return new WaitForSeconds(3f);
        toggleActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var activator = other.gameObject.GetComponent<PlayerAbilityActivator>();
        activator.ApplyAbility(data);

        StartCoroutine(tempHide());
    }
}
