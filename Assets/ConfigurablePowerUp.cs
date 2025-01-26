using UnityEngine;
using System.Collections;

public class ConfigurablePowerUp : MonoBehaviour
{
    public PowerUpScriptableObject data;

    public void Start()
    {
        var sRenderer = GetComponent<SpriteRenderer>();
        sRenderer.sprite = data.sprite;
    }
    
    // private IEnumerator tempHide()
    // {
    //     gameObject.SetActive(false);
    //     yield return new WaitForSeconds(3f);
    //     gameObject.SetActive(true);
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var activator = other.gameObject.GetComponent<PlayerAbilityActivator>();
        activator.ApplyAbility(data);

        // StartCoroutine(tempHide());

        // TBD how what the happy path will be
        // GameObject.Destroy(gameObject);
    }
}
