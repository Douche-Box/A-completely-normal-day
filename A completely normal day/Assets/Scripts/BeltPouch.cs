using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltPouch : MonoBehaviour
{
    [SerializeField] Transform _destination;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Polaroid"))
        {
            other.GetComponent<Rigidbody>().position = _destination.position;
        }
    }
}
