using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted,
    }

    [SerializeField] private Mode mode;


    private void LateUpdate()
    {
        Transform mainCameraTransform = Camera.main.transform;
        switch (mode)
        {
            case Mode.LookAt:
                transform.LookAt(mainCameraTransform);
                break;
            case Mode.LookAtInverted:
                var position = transform.position;
                Vector3 dirFromCamera = position - mainCameraTransform.position;
                transform.LookAt(position + dirFromCamera);
                break;
            case Mode.CameraForward:
                transform.forward = mainCameraTransform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -mainCameraTransform.forward;
                break;
        }
    }
}