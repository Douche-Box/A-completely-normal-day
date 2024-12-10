using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Yarn : MonoBehaviour
{
    [SerializeField] LineRenderer _lineRenderer;

    [SerializeField] Transform _currentThumbtackToAdd;
    [SerializeField] Transform _currentThumbtackToRemove;

    [SerializeField] List<Transform> _connectedThumbtacks;

    [SerializeField] InputActionProperty _pinchValue;

    [SerializeField] float _pinch;
    [SerializeField] bool _isPinching;

    [SerializeField] AudioSource _audioSource;

    [SerializeField] AudioClip _connectYarnClip;
    [SerializeField] AudioClip _disconnectYarnClip;

    private void OnEnable()
    {
        _pinchValue.action.started += OnPinch;
    }

    private void OnDisable()
    {
        _pinchValue.action.started -= OnPinch;
    }

    private void OnPinch(InputAction.CallbackContext context)
    {
        ConnectYarn();
    }

    private void Update()
    {
        for (int i = 0; i < _connectedThumbtacks.Count; i++)
        {
            _lineRenderer.SetPosition(i, _connectedThumbtacks[i].position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Thumbtack"))
        {
            for (int i = 0; i < _connectedThumbtacks.Count; i++)
            {
                if (_connectedThumbtacks[i] == other.transform)
                {
                    _currentThumbtackToRemove = other.transform;
                    return;
                }
            }
            _currentThumbtackToAdd = other.transform;
        }
    }

    private void ConnectYarn()
    {
        if (_currentThumbtackToAdd != null)
        {
            if (_audioSource != null && _connectYarnClip != null)
            {
                _audioSource.clip = _connectYarnClip;
                _audioSource.Play();
            }

            _lineRenderer.positionCount++;

            _connectedThumbtacks.Add(_currentThumbtackToAdd);

        }
        else if (_currentThumbtackToRemove != null)
        {
            if (_audioSource != null && _disconnectYarnClip != null)
            {
                _audioSource.clip = _disconnectYarnClip;
                _audioSource.Play();
            }

            _lineRenderer.positionCount--;

            _connectedThumbtacks.Remove(_currentThumbtackToRemove);
        }
    }
}