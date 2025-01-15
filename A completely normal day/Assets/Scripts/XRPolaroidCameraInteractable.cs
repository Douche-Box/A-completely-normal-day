using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(PolaroidCamera))]
public class XRPolaroidCameraInteractable : XRGrabInteractable
{
    [SerializeField] BeltCameraClip _cameraClip;
    public BeltCameraClip CameraClip
    { get { return _cameraClip; } set { _cameraClip = value; } }

    [SerializeField] GameObject _leftCameraHand;
    [SerializeField] GameObject _rightCameraHand;

    [SerializeField] PolaroidCamera _polaroidCamera;

    [SerializeField] Collider _collider;
    [SerializeField] Rigidbody _rigidbody;

    protected override void Awake()
    {
        base.Awake();

        _polaroidCamera = GetComponent<PolaroidCamera>();
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        transform.SetParent(null);
        _rigidbody.isKinematic = false;
        _collider.isTrigger = false;

        base.OnSelectEntered(args);

        if (args.interactorObject.transform.name == "LeftHand Controller")
        {
            _leftCameraHand.SetActive(true);
            args.interactorObject.transform.GetChild(0).gameObject.SetActive(false);

            if (_polaroidCamera != null)
            {
                _polaroidCamera.LeftHand = args.interactorObject.transform.gameObject;
            }

        }
        else if (args.interactorObject.transform.name == "RightHand Controller")
        {
            _rightCameraHand.SetActive(true);
            args.interactorObject.transform.GetChild(0).gameObject.SetActive(false);

            if (_polaroidCamera != null)
            {
                _polaroidCamera.RightHand = args.interactorObject.transform.gameObject;
            }
        }
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {

        base.OnSelectExited(args);

        transform.SetParent(null);
        _collider.isTrigger = false;
        _rigidbody.isKinematic = false;

        if (args.interactorObject.transform.name == "LeftHand Controller")
        {
            _leftCameraHand.SetActive(false);
            args.interactorObject.transform.GetChild(0).gameObject.SetActive(true);

            if (_polaroidCamera != null)
            {
                _polaroidCamera.LeftHand = null;
            }
        }
        else if (args.interactorObject.transform.name == "RightHand Controller")
        {
            _rightCameraHand.SetActive(false);
            args.interactorObject.transform.GetChild(0).gameObject.SetActive(true);

            if (_polaroidCamera != null)
            {
                _polaroidCamera.RightHand = null;
            }
        }

        if (_cameraClip != null)
        {
            _collider.isTrigger = true;
            _rigidbody.isKinematic = true;
            _cameraClip.AttachToBelt();
        }
    }
}
