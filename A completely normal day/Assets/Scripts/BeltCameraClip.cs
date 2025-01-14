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
            _xrPolaroidCameraInteractable.CameraClip = null;
            _xrPolaroidCameraInteractable = null;
        }
    }

    public void AttachToBelt()
    {
        transform.SetParent(_xrPolaroidCameraInteractable.transform, true);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
