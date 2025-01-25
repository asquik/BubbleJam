using UnityEngine;

public class Despawn : MonoBehaviour
{
    // In seconds
    public float timeToLive;

    public void Awake()
    {
        Destroy(gameObject, timeToLive);
    }
}
