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
            other.transform.position = _destination.position;
        }

        if (other.CompareTag("PolaroidCamera"))
        {
            other.transform.position = _destination.position;
        }

        if (other.CompareTag("Thumbtack"))
        {
            other.transform.position = _destination.position;
        }

        if (other.CompareTag("Clue"))
        {
            other.transform.position = _destination.position;
        }
    }
}
