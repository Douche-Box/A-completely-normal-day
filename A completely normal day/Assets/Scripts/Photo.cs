using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(XrPhotoInteractable))]
public class Photo : MonoBehaviour
{
    [SerializeField] RawImage photoImage;
    [SerializeField] Texture photoTexture;

    [SerializeField] Texture testTexture;

    [SerializeField] Rigidbody _rigidbody;

    [SerializeField] Polaroid _currentPolaroid;
    public Polaroid CurrentPolaroid
    { get { return _currentPolaroid; } set { _currentPolaroid = value; } }


    [SerializeField] float fadeDuration;
    [SerializeField] float fadeTime;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            CreatePhoto(testTexture);
        }
    }

    public void CreatePhoto(Texture newPhotoTexture)
    {
        photoTexture = newPhotoTexture;

        photoImage.texture = photoTexture;

        _rigidbody.isKinematic = true;

        StartCoroutine(FadeFromBlackToWhite());
    }

    void InitializePhoto()
    {
        photoImage.color = Color.black;

        photoImage.texture = null;

        _rigidbody = GetComponent<Rigidbody>();
    }

    void Awake()
    {
        InitializePhoto();
    }

    private IEnumerator FadeFromBlackToWhite()
    {
        fadeTime = 0f;

        Color startColor = Color.black;

        Color endColor = Color.white;

        while (fadeTime < fadeDuration)
        {
            photoImage.color = Color.Lerp(startColor, endColor, fadeTime / fadeDuration);

            fadeTime += Time.deltaTime;

            yield return null;
        }

        fadeTime = fadeDuration;

        photoImage.color = endColor;
    }

    public void DetachFromPolaroid()
    {
        if (_currentPolaroid != null)
        {
            _currentPolaroid.CurrentPhoto = null;
            _currentPolaroid = null;
        }
    }
}
