using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTransport : MonoBehaviour
{
    [SerializeField] Transform _destination;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Polaroid"))
        {
            other.GetComponent<Rigidbody>().position = _destination.position;
        }

        if (other.CompareTag("PolaroidCamera"))
        {
            other.GetComponent<Rigidbody>().position = _destination.position;
        }

        if (other.CompareTag("Thumbtack"))
        {
            other.GetComponent<Rigidbody>().position = _destination.position;
        }

        if (other.CompareTag("Clue"))
        {
            other.GetComponent<Rigidbody>().position = _destination.position;
        }
    }
}
