using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Polaroid))]
public class XRPolaroidInteractable : XRGrabInteractable
{
    [SerializeField] GameObject _leftCameraHand;
    [SerializeField] GameObject _rightCameraHand;

    [SerializeField] Polaroid _polaroid;

    protected override void Awake()
    {
        base.Awake();

        _polaroid = GetComponent<Polaroid>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (args.interactorObject.transform.name == "Left Controller")
        {
            _leftCameraHand.SetActive(true);
            args.interactorObject.transform.GetChild(0).gameObject.SetActive(false);
            if (_polaroid != null)
            {
                _polaroid.LeftHand = args.interactorObject.transform.gameObject;
            }

        }
        else if (args.interactorObject.transform.name == "Right Controller")
        {
            _rightCameraHand.SetActive(true);
            args.interactorObject.transform.GetChild(0).gameObject.SetActive(false);

            if (_polaroid != null)
            {
                _polaroid.RightHand = args.interactorObject.transform.gameObject;
            }
        }
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        if (args.interactorObject.transform.name == "Left Controller")
        {
            _leftCameraHand.SetActive(false);
            args.interactorObject.transform.GetChild(0).gameObject.SetActive(true);
            if (_polaroid != null)
            {
                _polaroid.LeftHand = null;
            }

        }
        else if (args.interactorObject.transform.name == "Right Controller")
        {
            _rightCameraHand.SetActive(false);
            args.interactorObject.transform.GetChild(0).gameObject.SetActive(true);
            if (_polaroid != null)
            {
                _polaroid.RightHand = null;
            }
        }
    }
}
