using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimator : MonoBehaviour
{
    [SerializeField] Animator _handAnimator;

    [SerializeField] InputActionProperty _pinchAnimation;
    [SerializeField] InputActionProperty _gripAnimation;

    void Update()
    {
        float pichValue = _pinchAnimation.action.ReadValue<float>();
        _handAnimator.SetFloat("Trigger", pichValue);

        float gripValue = _gripAnimation.action.ReadValue<float>();
        _handAnimator.SetFloat("Grip", gripValue);
    }
}
