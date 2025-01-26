using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DoubleJumpPowerUp", menuName = "Scriptable Objects/DoubleJumpPowerUp")]
public class DoubleJumpPowerUp : ConfigPowerUpScriptableObject<DoubleJumpData>
{
    public override void Apply(GameObject player)
    {
        var dashComp = player.GetComponent<PlayerJump>();
        dashComp.SetNewDoubleJumpData(data);
    }
}

[Serializable]
public class DoubleJumpData
{
    public bool useAirJumps;
    public int maxAirJumps;
}
