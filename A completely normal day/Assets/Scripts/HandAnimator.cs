using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimator : MonoBehaviour
{
    [SerializeField] Animator _handAnimator;

    [SerializeField] InputActionProperty _pinchValue;
    [SerializeField] InputActionProperty _gripValue;

    [SerializeField] float _pinch;
    [SerializeField] float _grip;

    [SerializeField] bool _isPinching;
    [SerializeField] bool _isGripping;

    private void OnEnable()
    {
        _pinchValue.action.started += OnPinch;
        _pinchValue.action.performed += OnPinch;
        _pinchValue.action.canceled += OnPinch;

        _gripValue.action.started += OnGrip;
        _gripValue.action.performed += OnGrip;
        _gripValue.action.canceled += OnGrip;
    }


    private void OnPinch(InputAction.CallbackContext context)
    {
        _pinch = context.ReadValue<float>();

        _isPinching = _pinch > 0;
    }

    private void OnGrip(InputAction.CallbackContext context)
    {
        _grip = context.ReadValue<float>();

        _isGripping = _grip > 0;
    }


    void Update()
    {
        float pichValue = _pinchValue.action.ReadValue<float>();
        _isPinching = _pinchValue.action.ReadValue<float>() > 0;
        _handAnimator.SetFloat("Trigger", pichValue);

        float gripValue = _gripValue.action.ReadValue<float>();
        _isGripping = _gripValue.action.ReadValue<float>() > 0;

        _handAnimator.SetFloat("Grip", gripValue);
    }
}
