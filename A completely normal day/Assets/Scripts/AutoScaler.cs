using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class AutoScaler : MonoBehaviour
{
    [SerializeField] Transform cameraHeight;
    [SerializeField] XROrigin xrOrigin;
    [SerializeField] float _defaultheight = 1.8f; // Default human height in meters
    [SerializeField] InputActionProperty _heightButton;

    [SerializeField] bool _isHeightButtonPressed;

    private void Awake()
    {
        StartCoroutine(WaitAndAdjustHeight());
    }

    private void OnEnable()
    {
        _heightButton.action.started += OnHeight;
        _heightButton.action.performed += OnHeight;
        _heightButton.action.canceled += OnHeight;
    }

    private void OnDisable()
    {
        _heightButton.action.started -= OnHeight;
        _heightButton.action.performed -= OnHeight;
        _heightButton.action.canceled -= OnHeight;
    }

    private void OnHeight(InputAction.CallbackContext context)
    {
        _isHeightButtonPressed = context.ReadValueAsButton();

        AdjustHeight();
    }

    private void AdjustHeight()
    {
        if (xrOrigin == null || cameraHeight == null)
        {
            return;
        }

        float headHeight = cameraHeight.position.y;
        float scale = _defaultheight / headHeight;
        transform.localScale = Vector3.one * scale;
    }

    public IEnumerator WaitAndAdjustHeight()
    {
        yield return new WaitForSeconds(5f);
        AdjustHeight();
    }
}
