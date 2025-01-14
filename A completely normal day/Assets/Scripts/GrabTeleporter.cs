using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabTeleporter : XRGrabInteractable
{
    [SerializeField] Transform _xrOrigin;

    [SerializeField] Transform _teleportLocation;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        _xrOrigin = args.interactorObject.transform.GetComponentInParent<XROrigin>().transform;

        if (_xrOrigin != null)
        {
            _xrOrigin.position = _teleportLocation.position;
        }
    }
}
