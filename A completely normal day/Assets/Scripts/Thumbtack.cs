using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

[RequireComponent(typeof(Rigidbody))]
public class Thumbtack : MonoBehaviour
{
    [SerializeField] XrThumbtackInteractable _xrThumbtackInteractable;

    [SerializeField] List<Transform> _connectedThumbtacks = new();
    public List<Transform> ConnectedThumbtacks
    { get { return _connectedThumbtacks; } set { _connectedThumbtacks = value; } }

    [SerializeField] YarnLine _yarnLine;
    public YarnLine YarnLine
    { get { return _yarnLine; } }

    [SerializeField] float _polaroidConnectOffset = 0.1f;

    [SerializeField] Polaroid _attachedPolaroid;
    public Polaroid AttachedPolaroid => _attachedPolaroid;

    private void Awake()
    {
        _xrThumbtackInteractable = GetComponentInParent<XrThumbtackInteractable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BulletinBoard"))
        {
            _xrThumbtackInteractable.BoardToAttachTo = other.transform;
        }
        if (other.CompareTag("Polaroid"))
        {
            other.GetComponent<XrPolaroidInteractable>().enabled = false;
            other.GetComponent<XRGeneralGrabTransformer>().enabled = false;
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.GetComponent<Collider>().enabled = false;

            other.transform.SetParent(GetComponentInParent<Transform>(), true);

            Vector3 newPosition = other.transform.position;
            float worldOffset = transform.TransformDirection(Vector3.up * _polaroidConnectOffset).y;
            newPosition.y = transform.position.y - worldOffset;

            Debug.Log($"Thumbtack Y: {transform.position.y}, Offset: {worldOffset}, New Y: {newPosition.y}");
            other.transform.position = newPosition;

            _attachedPolaroid = other.GetComponent<Polaroid>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BulletinBoard"))
        {
            if (_xrThumbtackInteractable.BoardToAttachTo = other.transform)
            {
                _xrThumbtackInteractable.BoardToAttachTo = null;
            }
        }
    }
}
