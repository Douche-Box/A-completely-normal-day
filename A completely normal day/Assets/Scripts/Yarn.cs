using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Yarn : MonoBehaviour
{
    [SerializeField] LineRenderer _lineRenderer;

    [SerializeField] int _currentLinerendererIndex;

    [SerializeField] Transform _currentThumbtack;
    [SerializeField] List<Transform> _connectedThumbtacks;

    [SerializeField] InputActionProperty _pinchValue;

    [SerializeField] float _pinch;
    [SerializeField] bool _isPinching;

    private void OnEnable()
    {
        _pinchValue.action.started += OnPinch;
        // _pinchValue.action.performed += OnPinch;
        // _pinchValue.action.canceled += OnPinch;
    }

    private void OnDisable()
    {
        _pinchValue.action.started -= OnPinch;
        // _pinchValue.action.performed -= OnPinch;
        // _pinchValue.action.canceled -= OnPinch;
    }

    private void OnPinch(InputAction.CallbackContext context)
    {
        // _pinch = context.ReadValue<float>();

        // _isPinching = _pinch > 0;

        ConnectYarn();
    }

    private void Update()
    {
        // _lineRenderer.SetPosition(0, transform.position);

        for (int i = 0; i < _connectedThumbtacks.Count; i++)
        {
            _lineRenderer.SetPosition(i, _connectedThumbtacks[i].position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Thumbtack"))
        {
            _currentThumbtack = other.transform;
        }
    }

    private void ConnectYarn()
    {
        if (_currentThumbtack != null)
        {
            _lineRenderer.positionCount++;

            _currentLinerendererIndex = _lineRenderer.positionCount - 1;

            _connectedThumbtacks.Add(_currentThumbtack);
        }
    }
}