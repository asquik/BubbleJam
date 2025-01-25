using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    [SerializeField] private GameObject player;

    void Update()
    {
        CinemachineStateDrivenCamera cm = GetComponentInChildren<CinemachineStateDrivenCamera>();
        cm.m_Follow = player.transform;
        cm.m_LookAt = player.transform;
        cm.m_AnimatedTarget = player.GetComponent<Animator>();
    }
}
