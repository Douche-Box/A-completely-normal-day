using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Yarn : MonoBehaviour
{
    [SerializeField] InputActionProperty _drawLineValue;
    [SerializeField] InputActionProperty _removeLineValue;

    [SerializeField] float _drawLine;
    [SerializeField] bool _isDrawingLine;

    [SerializeField] float _removeLine;
    [SerializeField] bool _isRemovingLine;

    [SerializeField] GameObject _leftHand;
    public GameObject LeftHand
    { get { return _leftHand; } set { _leftHand = value; } }
    [SerializeField] GameObject _rightHand;
    public GameObject RightHand
    { get { return _rightHand; } set { _rightHand = value; } }

    [SerializeField] Transform _currentThumbtack;
    [SerializeField] YarnLine _currentYarnLine;
    [SerializeField] GameObject _yarnLinePrefab;
    [SerializeField] Transform _bulletinBoard;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _connectYarnClip;
    [SerializeField] AudioClip _disconnectYarnClip;

    private void OnEnable()
    {
        _drawLineValue.action.started += OnDrawLine;
        _drawLineValue.action.performed += OnDrawLine;
        _drawLineValue.action.canceled += OnDrawLine;

        _removeLineValue.action.started += OnRemoveLine;
        _removeLineValue.action.performed += OnRemoveLine;
        _removeLineValue.action.canceled += OnRemoveLine;
    }

    private void OnDisable()
    {
        _drawLineValue.action.started -= OnDrawLine;
        _drawLineValue.action.performed -= OnDrawLine;
        _drawLineValue.action.canceled -= OnDrawLine;

        _removeLineValue.action.started -= OnRemoveLine;
        _removeLineValue.action.performed -= OnRemoveLine;
        _removeLineValue.action.canceled -= OnRemoveLine;
    }

    private void OnDrawLine(InputAction.CallbackContext context)
    {
        if (_rightHand != null)
        {
            _isDrawingLine = context.started || context.performed;

            if (context.canceled && _currentYarnLine != null)
            {
                _currentYarnLine.StopDrawing();
                _currentYarnLine = null;
            }
        }
    }

    private void OnRemoveLine(InputAction.CallbackContext context)
    {
        if (_leftHand != null)
        {
            _removeLine = context.ReadValue<float>();

            _isRemovingLine = _drawLine == 1f;
        }
    }

    private void Update()
    {
        if (_isDrawingLine)
        {
            ConnectYarn();
            if (_currentYarnLine != null)
            {
                _currentYarnLine.UpdatePreviewPosition(transform.position);
            }
        }
        else if (_isRemovingLine)
        {
            DisconnectYarn();
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
        if (_currentThumbtack == null) return;

        var thumbtack = _currentThumbtack.GetComponentInChildren<Thumbtack>();
        if (thumbtack == null) return;

        // Start new connection
        if (_currentYarnLine == null)
        {
            var newLine = Instantiate(_yarnLinePrefab, _bulletinBoard).GetComponent<YarnLine>();
            if (newLine.TryStartConnection(_currentThumbtack))
            {
                _currentYarnLine = newLine;
                PlayConnectSound();
            }
            else
            {
                Destroy(newLine.gameObject);
            }
        }
        // Add to existing connection
        else if (_currentYarnLine.TryCompleteConnection(_currentThumbtack))
        {
            PlayConnectSound();
        }

        _currentThumbtack = null;
    }

    private void DisconnectYarn()
    {
        if (_currentThumbtack == null) return;

        var thumbtack = _currentThumbtack.GetComponent<Thumbtack>();
        if (thumbtack?.YarnLine != null)
        {
            thumbtack.YarnLine.DetachThumbtack(_currentThumbtack);
            PlayDisconnectSound();
        }

        _currentThumbtack = null;
    }

    private void PlayConnectSound()
    {
        if (_audioSource && _connectYarnClip)
        {
            _audioSource.clip = _connectYarnClip;
            _audioSource.Play();
        }
    }

    private void PlayDisconnectSound()
    {
        if (_audioSource && _disconnectYarnClip)
        {
            _audioSource.clip = _disconnectYarnClip;
            _audioSource.Play();
        }
    }
}

