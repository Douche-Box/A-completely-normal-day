using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XrWalkieInteractable : XRGrabInteractable
{
    [SerializeField] WalkieTalkie _walkieTalkie;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (args.interactorObject.transform.name == "LeftHand Controller")
        {
            if (_walkieTalkie != null)
            {
                _walkieTalkie.LeftHand = args.interactorObject.transform.gameObject;
            }

        }
        else if (args.interactorObject.transform.name == "RightHand Controller")
        {
            if (_walkieTalkie != null)
            {
                _walkieTalkie.RightHand = args.interactorObject.transform.gameObject;
            }
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        if (args.interactorObject.transform.name == "LeftHand Controller")
        {
            if (_walkieTalkie != null)
            {
                _walkieTalkie.LeftHand = null;
            }

        }
        else if (args.interactorObject.transform.name == "RightHand Controller")
        {
            if (_walkieTalkie != null)
            {
                _walkieTalkie.RightHand = null;
            }
        }
    }
}
