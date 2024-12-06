using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yarn : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;

    [SerializeField] int _currentLinerendererIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Thumbtack"))
        {
            lineRenderer.positionCount++;
            _currentLinerendererIndex = lineRenderer.positionCount;
            lineRenderer.SetPosition(_currentLinerendererIndex, other.transform.position);
        }
    }
}
