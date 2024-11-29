using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class ClueData
{
    public Clue clue;
    public int cluePoints;

    // Constructor for creating ClueData instances
    public ClueData(Clue clue, int cluePoints)
    {
        this.clue = clue;
        this.cluePoints = cluePoints;
    }
}

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

    [SerializeField] List<ClueData> clueDatas = new();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

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

    public void InitializePhoto(List<Clue> clues = null, List<int> cluePointsVisible = null)
    {
        photoImage.color = Color.black;
        photoImage.texture = null;

        if (clues != null && cluePointsVisible != null && clues.Count == cluePointsVisible.Count)
        {
            for (int i = 0; i < clues.Count; i++)
            {
                if (clues[i] != null)
                {
                    // Create a new ClueData instance and add it to the clueDatas list
                    ClueData newClueData = new ClueData(clues[i], cluePointsVisible[i]);
                    clueDatas.Add(newClueData);
                }
            }
        }
        else
        {
            Debug.LogError("Clues or cluePointsVisible list is null or mismatched in length.");
        }
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