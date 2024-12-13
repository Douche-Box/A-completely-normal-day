using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Yarn : MonoBehaviour
{
    [SerializeField] Transform _currentThumbtack;
    [SerializeField] GameObject _currentYarnLine;

    [SerializeField] InputActionProperty _pinchValue;

    [SerializeField] float _pinch;
    [SerializeField] bool _isPinching;

    [SerializeField] AudioSource _audioSource;

    [SerializeField] AudioClip _connectYarnClip;
    [SerializeField] AudioClip _disconnectYarnClip;

    [SerializeField] Transform _bulletinBoard;

    [SerializeField] GameObject _yarnLinePrefab;

    private void OnEnable()
    {
        _pinchValue.action.started += OnPinch;
        _pinchValue.action.performed += OnPinch;
        _pinchValue.action.canceled += OnPinch;
    }

    private void OnDisable()
    {
        _pinchValue.action.started -= OnPinch;
        _pinchValue.action.performed -= OnPinch;
        _pinchValue.action.canceled -= OnPinch;
    }

    private void OnPinch(InputAction.CallbackContext context)
    {
        _pinch = context.ReadValue<float>();

        _isPinching = _pinch == 1f;


    }

    private void Update()
    {
        if (_isPinching)
        {
            ConnectYarn();
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
            if (_currentThumbtack.GetComponentInChildren<Thumbtack>().YarnLine == null)
            {
                if (_audioSource != null && _connectYarnClip != null)
                {
                    _audioSource.clip = _connectYarnClip;
                    _audioSource.Play();
                }

                if (_currentYarnLine == null)
                {
                    GameObject newYarnLine = Instantiate(_yarnLinePrefab, _bulletinBoard);

                    newYarnLine.GetComponent<YarnLine>().AttachOrDetachThumbtack(_currentThumbtack);

                    _currentYarnLine = newYarnLine;
                }
                else
                {
                    _currentYarnLine.GetComponent<YarnLine>().AttachOrDetachThumbtack(_currentThumbtack);
                }

                _currentThumbtack = null;

            }
            else
            {
                _currentYarnLine = _currentThumbtack.GetComponentInChildren<Thumbtack>().YarnLine.gameObject;

                _currentYarnLine.GetComponent<YarnLine>().AttachOrDetachThumbtack(_currentThumbtack);

                _currentThumbtack = null;
            }
        }
    }
}