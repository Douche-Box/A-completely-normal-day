using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WalkieTalkie : MonoBehaviour
{
    [SerializeField] InputActionProperty _talkValueRightHand;
    [SerializeField] InputActionProperty _talkValueLeftHand;

    [SerializeField] GameObject _leftHand;
    public GameObject LeftHand
    { get { return _leftHand; } set { _leftHand = value; } }
    [SerializeField] GameObject _rightHand;
    public GameObject RightHand
    { get { return _rightHand; } set { _rightHand = value; } }

    [SerializeField] bool _isTalkButtonPressed;

    [SerializeField] bool _canWin;
    public bool CanWin
    { get { return _canWin; } set { _canWin = value; } }

    [SerializeField] bool _hasWon;

    private void OnEnable()
    {
        _talkValueRightHand.action.started += OnTalkRight;
        _talkValueRightHand.action.performed += OnTalkRight;
        _talkValueRightHand.action.canceled += OnTalkRight;

        _talkValueLeftHand.action.started += OnTalkLeft;
        _talkValueLeftHand.action.performed += OnTalkLeft;
        _talkValueLeftHand.action.canceled += OnTalkLeft;
    }

    private void OnDisable()
    {
        _talkValueRightHand.action.started -= OnTalkRight;
        _talkValueRightHand.action.performed -= OnTalkRight;
        _talkValueRightHand.action.canceled -= OnTalkRight;

        _talkValueLeftHand.action.started -= OnTalkLeft;
        _talkValueLeftHand.action.performed -= OnTalkLeft;
        _talkValueLeftHand.action.canceled -= OnTalkLeft;
    }

    private void OnTalkRight(InputAction.CallbackContext context)
    {
        if (_rightHand != null && !_hasWon)
        {
            float talkValue = context.ReadValue<float>();

            _isTalkButtonPressed = talkValue == 1f;
        }
    }

    private void OnTalkLeft(InputAction.CallbackContext context)
    {
        if (_leftHand != null && !_hasWon)
        {
            float talkValue = context.ReadValue<float>();

            _isTalkButtonPressed = talkValue == 1f;
        }
    }

    private void Update()
    {
        if (_isTalkButtonPressed)
        {
            if (_canWin)
            {
                _canWin = false;
                _hasWon = true;
                Debug.Log("You win!");
            }
        }
    }

}
