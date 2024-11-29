using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Photo))]
public class XrPhotoInteractable : XRGrabInteractable
{
    [SerializeField] Photo _photo;

    protected override void Awake()
    {
        base.Awake();

        _photo = GetComponent<Photo>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        _photo.DetachFromPolaroid();
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