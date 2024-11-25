using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polaroid : MonoBehaviour
{
    [SerializeField] Camera _polaroidCamera;  // Camera used for taking photos
    [SerializeField] GameObject _photoPrefab; // Prefab for the photo object
    [SerializeField] Transform _photoSpawnPoint; // Where the photo appears
    [SerializeField] int photoWidth = 512; // Photo resolution width
    [SerializeField] int photoHeight = 512; // Photo resolution height

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            TakePhoto();
        }
    }

    public void TakePhoto()
    {
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

        Photo newPhoto = Instantiate(_photoPrefab, _photoSpawnPoint.position, Quaternion.identity).GetComponent<Photo>();
        newPhoto.CreatePhoto(photoTexture);
    }
}
