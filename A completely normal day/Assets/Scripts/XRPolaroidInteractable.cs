using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRPolaroidInteractable : XRGrabInteractable
{
    [SerializeField] GameObject _leftCameraHand;
    [SerializeField] GameObject _rightCameraHand;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (args.interactorObject.transform.name == "Left Controller")
        {
            _leftCameraHand.SetActive(true);
            args.interactorObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        else if (args.interactorObject.transform.name == "Right Controller")
        {
            _rightCameraHand.SetActive(true);
            args.interactorObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        if (args.interactorObject.transform.name == "Left Controller")
        {
            _leftCameraHand.SetActive(false);
            args.interactorObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (args.interactorObject.transform.name == "Right Controller")
        {
            _rightCameraHand.SetActive(false);
            args.interactorObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
