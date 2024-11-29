using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using System;
using UnityEngine.InputSystem;

[RequireComponent(typeof(XRPolaroidInteractable))]
public class Polaroid : MonoBehaviour
{
    [SerializeField] Camera _polaroidCamera;
    [SerializeField] GameObject _photoPrefab;
    [SerializeField] Transform _photoSpawnPoint;
    [SerializeField] int photoWidth = 512;
    [SerializeField] int photoHeight = 512;

    [SerializeField] GameObject _leftHand;
    public GameObject LeftHand
    { get { return _leftHand; } set { _leftHand = value; OnLeftHandChanged?.Invoke(value); } }
    [SerializeField] GameObject _rightHand;
    public GameObject RightHand
    { get { return _rightHand; } set { _rightHand = value; OnRightHandChanged?.Invoke(value); } }

    [SerializeField] InputActionProperty _pinchValue;

    [SerializeField] bool _canTakePhoto;

    public event Action<GameObject> OnRightHandChanged;
    public event Action<GameObject> OnLeftHandChanged;

    public event Action<Photo> OnCurrentPhotoChanged;

    [SerializeField] Photo _currentPhoto;
    public Photo CurrentPhoto
    { get { return _currentPhoto; } set { _currentPhoto = value; OnCurrentPhotoChanged?.Invoke(value); } }

    private void OnEnable()
    {
        OnRightHandChanged += OnRightHandChange;
        OnLeftHandChanged += OnLeftHandChange;

        OnCurrentPhotoChanged += OnCurrentPhotoChange;

        _pinchValue.action.started += OnPinch;
        _pinchValue.action.performed += OnPinch;
        _pinchValue.action.canceled += OnPinch;
    }

    private void OnDisable()
    {
        OnRightHandChanged -= OnRightHandChange;
        OnLeftHandChanged -= OnLeftHandChange;

        _pinchValue.action.started -= OnPinch;
        _pinchValue.action.performed -= OnPinch;
        _pinchValue.action.canceled -= OnPinch;
    }

    private void OnPinch(InputAction.CallbackContext context)
    {
        if (_rightHand != null)
        {
            float pinch = context.ReadValue<float>();

            bool takePhoto = pinch > 0;

            if (takePhoto)
            {
                TakePhoto();
            }
        }
    }

    void OnLeftHandChange(GameObject leftController)
    {

    }

    void OnRightHandChange(GameObject rightController)
    {

    }

    void OnCurrentPhotoChange(Photo photo)
    {
        if (photo == null)
        {
            _canTakePhoto = true;
        }
    }

    private void Awake()
    {
        _canTakePhoto = true;
    }

    public void TakePhoto()
    {
        if (_canTakePhoto)
        {
            _canTakePhoto = false;
            RenderTexture tempRenderTexture = new RenderTexture(photoWidth, photoHeight, 24);

            RenderTexture originalRenderTexture = _polaroidCamera.targetTexture;
            _polaroidCamera.targetTexture = tempRenderTexture;

            _polaroidCamera.Render();

            RenderTexture.active = tempRenderTexture;
            Texture2D photoTexture = new Texture2D(photoWidth, photoHeight, TextureFormat.RGB24, false);
            photoTexture.ReadPixels(new Rect(0, 0, photoWidth, photoHeight), 0, 0);
            photoTexture.Apply();

            _polaroidCamera.targetTexture = originalRenderTexture;
            RenderTexture.active = null;

            Destroy(tempRenderTexture);

            Photo newPhoto = Instantiate(_photoPrefab, _photoSpawnPoint.position, _photoPrefab.transform.rotation).GetComponent<Photo>();
            newPhoto.transform.SetParent(_photoSpawnPoint, true);
            newPhoto.CurrentPolaroid = this;
            newPhoto.CreatePhoto(photoTexture);
        }
    }
}
