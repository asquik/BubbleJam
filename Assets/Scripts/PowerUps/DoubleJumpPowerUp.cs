using UnityEngine;

[CreateAssetMenu(fileName = "DoubleJumpPowerUp", menuName = "Scriptable Objects/DoubleJumpPowerUp")]
public class DoubleJumpPowerUp : ModifierPowerUpScriptableObject
{
    [SerializeField] private int maxAirJumps;
    
    public override void Apply(GameObject player)
    {
        var dashComp = player.GetComponent<PlayerJump>();
        dashComp.maxAirJumpsModifier = maxAirJumps;
    }
}
