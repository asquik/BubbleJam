using UnityEngine;

public class DeathPlatform : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
		collision.gameObject.transform.position = respawnPoint.position;
    }
}
