using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PowerUpScriptableObject : ScriptableObject
{
    [SerializeField] public Color powerUpColor;
    
    public abstract InputAction getInputBinding(PlayerInput input);
    
    public abstract IEnumerator activateAbility(Transform transform, Rigidbody2D rb);
    
    // Subscribe to ability
    // public abstract void Apply(GameObject player);
    // Unsubscribe to ability
    // public abstract void Remove(GameObject player);
}

public abstract class ConfigPowerUpScriptableObject<TData> : PowerUpScriptableObject
    where TData : new()
{
    [SerializeField] protected TData data;
}