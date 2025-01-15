using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltCameraClip : MonoBehaviour
{
    [SerializeField] XRPolaroidCameraInteractable _xrPolaroidCameraInteractable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PolaroidCamera"))
        {
            _xrPolaroidCameraInteractable = other.GetComponent<XRPolaroidCameraInteractable>();
            _xrPolaroidCameraInteractable.CameraClip = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PolaroidCamera"))
        {
            if (_xrPolaroidCameraInteractable == null)
            {
                return;
            }

            _xrPolaroidCameraInteractable.CameraClip = null;
            _xrPolaroidCameraInteractable = null;
        }
    }

    public void AttachToBelt()
    {
        _xrPolaroidCameraInteractable.transform.SetParent(transform);
        _xrPolaroidCameraInteractable.transform.position = transform.position;
        _xrPolaroidCameraInteractable.transform.up = transform.forward;
    }
}
