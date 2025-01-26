using UnityEngine;

public abstract class PowerUpScriptableObject : ScriptableObject
{
    [SerializeField] public Sprite sprite;
    public abstract void Apply(GameObject player);
}

public abstract class ConfigPowerUpScriptableObject<TData> : PowerUpScriptableObject
    where TData : new()
{
    [SerializeField] protected TData data;
}
