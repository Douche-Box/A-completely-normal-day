using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.transform.SetParent(GetComponentInParent<Transform>(), true);

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
