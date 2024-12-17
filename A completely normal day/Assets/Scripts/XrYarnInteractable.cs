using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Yarn))]
public class XrYarnInteractable : XRGrabInteractable
{
    [SerializeField] Collider _collider;

    [SerializeField] Yarn _yarn;

    protected override void Awake()
    {
        base.Awake();

        _yarn = GetComponent<Yarn>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (args.interactorObject.transform.name == "LeftHand Controller")
        {
            if (_yarn != null)
            {
                _yarn.LeftHand = args.interactorObject.transform.gameObject;
            }

        }
        else if (args.interactorObject.transform.name == "RightHand Controller")
        {
            if (_yarn != null)
            {
                _yarn.RightHand = args.interactorObject.transform.gameObject;
            }
        }

        _collider.isTrigger = true;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        if (args.interactorObject.transform.name == "LeftHand Controller")
        {
            if (_yarn != null)
            {
                _yarn.LeftHand = null;
            }

        }
        else if (args.interactorObject.transform.name == "RightHand Controller")
        {
            if (_yarn != null)
            {
                _yarn.RightHand = null;
            }
        }

        _collider.isTrigger = false;
    }
}
