using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Yarn : MonoBehaviour
{
    [SerializeField] Transform _currentThumbtack;
    [SerializeField] GameObject _currentYarnLine;

    [SerializeField] InputActionProperty _drawLineValue;
    [SerializeField] InputActionProperty _removeLineValue;

    [SerializeField] float _drawLine;
    [SerializeField] bool _isDrawingLine;

    [SerializeField] float _removeLine;
    [SerializeField] bool _isRemovingLine;

    [SerializeField] AudioSource _audioSource;

    [SerializeField] AudioClip _connectYarnClip;
    [SerializeField] AudioClip _disconnectYarnClip;

    [SerializeField] Transform _bulletinBoard;

    [SerializeField] GameObject _yarnLinePrefab;

    [SerializeField] GameObject _leftHand;
    public GameObject LeftHand
    { get { return _leftHand; } set { _leftHand = value; } }
    [SerializeField] GameObject _rightHand;
    public GameObject RightHand
    { get { return _rightHand; } set { _rightHand = value; } }

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
            _drawLine = context.ReadValue<float>();

            _isDrawingLine = _drawLine == 1f;
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

                    newYarnLine.GetComponent<YarnLine>().AttachThumbtack(_currentThumbtack);

                    _currentYarnLine = newYarnLine;
                }
                else
                {
                    _currentYarnLine.GetComponent<YarnLine>().AttachThumbtack(_currentThumbtack);
                }

                _currentThumbtack = null;

            }
            else
            {
                _currentYarnLine = _currentThumbtack.GetComponentInChildren<Thumbtack>().YarnLine.gameObject;

                _currentYarnLine.GetComponent<YarnLine>().AttachThumbtack(_currentThumbtack);

                _currentThumbtack = null;
            }
        }
    }

    // FIX LOGIC FOR THIS // // FIX LOGIC FOR THIS // // FIX LOGIC FOR THIS // // FIX LOGIC FOR THIS //
    // FIX LOGIC FOR THIS // // FIX LOGIC FOR THIS // // FIX LOGIC FOR THIS // // FIX LOGIC FOR THIS //
    private void DisconnectYarn()
    {
        if (_currentThumbtack != null)
        {
            if (_currentThumbtack.GetComponentInChildren<Thumbtack>().YarnLine == null)
            {
                if (_audioSource != null && _connectYarnClip != null)
                {
                    _audioSource.clip = _disconnectYarnClip;
                    _audioSource.Play();
                }

                if (_currentYarnLine == null)
                {
                    GameObject newYarnLine = Instantiate(_yarnLinePrefab, _bulletinBoard);

                    newYarnLine.GetComponent<YarnLine>().DetachThumbtack(_currentThumbtack);

                    _currentYarnLine = newYarnLine;
                }
                else
                {
                    _currentYarnLine.GetComponent<YarnLine>().DetachThumbtack(_currentThumbtack);
                }

                _currentThumbtack = null;

            }
            else
            {
                _currentYarnLine = _currentThumbtack.GetComponentInChildren<Thumbtack>().YarnLine.gameObject;

                _currentYarnLine.GetComponent<YarnLine>().DetachThumbtack(_currentThumbtack);

                _currentThumbtack = null;
            }
        }
    }
    // FIX LOGIC FOR THIS // // FIX LOGIC FOR THIS // // FIX LOGIC FOR THIS // // FIX LOGIC FOR THIS //
    // FIX LOGIC FOR THIS // // FIX LOGIC FOR THIS // // FIX LOGIC FOR THIS // // FIX LOGIC FOR THIS //
}