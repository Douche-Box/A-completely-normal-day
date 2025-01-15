using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belt : MonoBehaviour
{
    [SerializeField] Transform _camera;
    [SerializeField] float _heightOffset = -0.65f;
    [SerializeField] float _backwardOffset = -0.2f; // Adjust in inspector

    private void Update()
    {
        if (_camera == null) return;

        // Get flattened forward direction
        Vector3 flatForward = _camera.forward;
        flatForward.y = 0;
        flatForward.Normalize();

        // Position with both offsets
        Vector3 newPosition = _camera.position;
        newPosition.y += _heightOffset;
        newPosition += flatForward * _backwardOffset;
        transform.position = newPosition;

        // Rotation
        float angle = Mathf.Atan2(flatForward.x, flatForward.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}
