using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Photo : MonoBehaviour
{
    [SerializeField] RawImage photoImage;
    [SerializeField] Texture photoTexture;

    [SerializeField] Texture testTexture;

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

        StartCoroutine(FadeFromBlackToWhite());
    }

    void InitializePhoto()
    {
        photoImage.color = Color.black;

        photoImage.texture = null;
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
}
