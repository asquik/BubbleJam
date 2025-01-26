using UnityEngine;
using System;

[CreateAssetMenu(fileName = "DashPowerUp", menuName = "Scriptable Objects/DashPowerUp")]
public class DashPowerUp : ConfigPowerUpScriptableObject<DashData>
{
    public override void Apply(GameObject player)
    {
        var dashComp = player.GetComponent<PlayerHorizontalMovement>();
        dashComp.SetNewDashData(data);
    }
}

[Serializable]
public class DashData
{
    public bool useDash;
    public float dashSpeed;
    public float dashCooldown;
    public float dashTime;
}