using UnityEngine;

public abstract class ConfigPowerUp<TData> : ScriptableObject 
    where TData : new()
{
    [SerializeField] protected TData data;

    public abstract void Apply(GameObject player);
}
