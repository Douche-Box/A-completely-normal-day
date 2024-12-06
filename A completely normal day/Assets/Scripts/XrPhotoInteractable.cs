using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Polaroid))]
public class XrPhotoInteractable : XRGrabInteractable
{
    [SerializeField] Polaroid _polaroid;

    protected override void Awake()
    {
        base.Awake();

        _polaroid = GetComponent<Polaroid>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        _polaroid.DetachFromPolaroid();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        transform.SetParent(null);

        Rigidbody photoRigidbody = GetComponent<Rigidbody>();

        if (photoRigidbody != null)
        {
            photoRigidbody.isKinematic = false;
        }
    }
}