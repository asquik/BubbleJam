using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformInspector : MonoBehaviour
{
    [SerializeField] private float maxPlatformSpeed;
    [SerializeField] private float decelerationDistance;
    [SerializeField] private float accelerationDistance;
    [SerializeField] private float waitTime;

    public float getMaxPlatformSpeed()
    {
        return maxPlatformSpeed;
    }

    public float getDecelerationDistance()
    {
        return decelerationDistance;
    }

    public float getAccelerationDistance()
    {
        return accelerationDistance;
    }

    public float getWaitTime()
    {
        return waitTime;
    }

    


}
