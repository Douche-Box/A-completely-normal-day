using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XrThumbtackInteractable : XRGrabInteractable
{
    [SerializeField] Thumbtack _thumbtack;

    [SerializeField] Rigidbody _rigidbody;

    [SerializeField] Collider _tackCollider;

    [SerializeField] Transform _boardToAttachTo;
    public Transform BoardToAttachTo
    { get { return _boardToAttachTo; } set { _boardToAttachTo = value; } }

    [SerializeField] Transform _photoToAttachTo;
    public Transform PhotoToAttachTo
    { get { return _photoToAttachTo; } set { _photoToAttachTo = value; } }

    protected override void Awake()
    {
        base.Awake();

        _thumbtack = GetComponentInChildren<Thumbtack>();

        _rigidbody = GetComponent<Rigidbody>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = false;
        }

        if (_tackCollider != null)
        {
            _tackCollider.enabled = true;
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        if (_boardToAttachTo != null)
        {
            if (_rigidbody != null)
            {
                _rigidbody.isKinematic = true;
            }

            if (_tackCollider != null)
            {
                _tackCollider.enabled = false;
            }
        }
        else
        {
            if (_rigidbody != null)
            {
                _rigidbody.isKinematic = false;
            }

            if (_tackCollider != null)
            {
                _tackCollider.enabled = true;
            }
        }

        if (_photoToAttachTo != null)
        {
            _photoToAttachTo.GetComponent<XrPhotoInteractable>().enabled = true;
        }
        else
        {

        }

        base.OnSelectExited(args);
    }
}