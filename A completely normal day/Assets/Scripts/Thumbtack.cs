using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thumbtack : MonoBehaviour
{
    [SerializeField] XrThumbtackInteractable _xrThumbtackInteractable;

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
        if (other.CompareTag("Photo"))
        {
            other.GetComponent<XrPhotoInteractable>().enabled = false;
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
