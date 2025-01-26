using UnityEngine;

public class DeathPlatform : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    
    private Transform abyssPlatform;
    private LayerMask playerLayer;
    
    public void Start()
    {
        abyssPlatform = transform.Find("Abyss").transform;
        playerLayer = LayerMask.GetMask("Player");
    }
    
    // public void Update()
    // {
    //     if (abyssCheck())
    //     {
    //         respawn();
    //     }
    // }

    void respawn()
    {
        
        Debug.Log("Respawn Player");
    }
    
    bool abyssCheck()
    {
        return Physics2D.OverlapBox(abyssPlatform.position, new Vector2(0.2f, 0.2f), playerLayer);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Enter");
        Debug.Log(collision.gameObject);
    }
    
    public void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Tigger Enter");
        Debug.Log(collision.gameObject);
    }
}
