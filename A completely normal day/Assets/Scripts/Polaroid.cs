using System;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] string _clueTag = "Clue";

    public event Action<GameObject> OnRightHandChanged;
    public event Action<GameObject> OnLeftHandChanged;
    public event Action<Photo> OnCurrentPhotoChanged;

    [SerializeField] Photo _currentPhoto;
    public Photo CurrentPhoto
    { get { return _currentPhoto; } set { _currentPhoto = value; OnCurrentPhotoChanged?.Invoke(value); } }

    [SerializeField] List<GameObject> _objectsInView = new List<GameObject>();

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
            newPhoto.transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
            newPhoto.CurrentPolaroid = this;

            UpdateObjectsInView();

            CheckForCluePoints(newPhoto);

            newPhoto.CreatePhoto(photoTexture);
        }
    }

    void CheckForCluePoints(Photo newPhoto) // CHECK IF THIS NEW VERSION WORKS
    {
        List<Clue> clues = new();
        List<int> clueAmounts = new();

        foreach (GameObject clueObject in _objectsInView)
        {
            if (clueObject != null)
            {
                Debug.Log("Clue object != null");
                Clue clue = clueObject.GetComponent<Clue>();


                if (clue != null)
                {
                    Debug.Log("Clue != null");

                    int clueAmount = 0;

                    clues.Add(clue);
                    Debug.Log("Clue added to Clues");

                    foreach (Transform cluePoint in clue.CluePoints)
                    {
                        Vector3 cluePointPosition = cluePoint.position;
                        Vector3 directionToCamera = (_polaroidCamera.transform.position - cluePointPosition).normalized;

                        Ray ray = new Ray(cluePointPosition, directionToCamera);
                        if (Physics.Raycast(ray, out RaycastHit hit))
                        {
                            if (hit.collider != null && hit.collider.gameObject == gameObject)
                            {
                                clueAmount++;
                            }
                        }
                    }

                    Debug.Log($"ClueAmount added to ClueAmounts with an amount of {clueAmount}");
                    clueAmounts.Add(clueAmount);
                }
            }
        }

        newPhoto.InitializePhoto(clues, clueAmounts);
    }


    public bool IsInCameraView(Camera camera, Vector3 worldPosition)
    {
        Vector3 viewportPos = camera.WorldToViewportPoint(worldPosition);
        return viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1 && viewportPos.z > 0;
    }

    public void UpdateObjectsInView()
    {
        _objectsInView.Clear();

        GameObject[] allTaggedObjects = GameObject.FindGameObjectsWithTag(_clueTag);

        foreach (GameObject obj in allTaggedObjects)
        {
            if (obj != null && IsInCameraView(_polaroidCamera, obj.transform.position))
            {
                _objectsInView.Add(obj);
            }
        }

        Debug.Log($"Objects in view: {_objectsInView.Count}");
    }
}