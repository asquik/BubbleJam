using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PowerUpScriptableObject : ScriptableObject
{
    [SerializeField] public Color powerUpColor;
}

public abstract class ActionPowerUpScriptableObject : PowerUpScriptableObject
{
    public abstract InputAction GetInputBinding(PlayerInput input);

    public abstract IEnumerator ActivateAbility(PlayerAbilityActivator activator, Transform transform, Rigidbody2D rb);

    // Optional (Not all abilities need this to be implemented)
    // For some reason when it was "virtual", the callback just wouldn't get called at all.
    public abstract void DeactivateAbility(GameObject player);
}

public abstract class ModifierPowerUpScriptableObject : PowerUpScriptableObject
{
    public abstract void Apply(GameObject player);
}