using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Platform : MonoBehaviour
{

    private PlatformEffector2D effector;
    [SerializeField] private bool useOneWay;
    [Range(0, 360), SerializeField] private int platformSurfaceArc;
    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();

        if (!useOneWay)
        {
            effector.useOneWay = false;
        }

        if (useOneWay)
        {
            effector.useOneWay = true;
            effector.surfaceArc = platformSurfaceArc;

        }
    }
}
