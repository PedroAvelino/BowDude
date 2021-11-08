using UnityEngine;
using Cinemachine;
using System;

public class ArrowCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cam;
    private void OnEnable()
    {
        Arrow.OnArrowCreatedAction += SetCameraToArrow;
        Arrow.OnArrowStoppedAction += DisableCamera;
    }

    private void SetCameraToArrow( Arrow arrow )
    {
        _cam.enabled = true;
        _cam.m_Follow = arrow.transform;
    }
    private void DisableCamera( Arrow character )
    {
        _cam.enabled = false;
    }

    private void OnDisable()
    {
        Arrow.OnArrowCreatedAction -= SetCameraToArrow;
        Arrow.OnArrowStoppedAction -= DisableCamera;
    }

}