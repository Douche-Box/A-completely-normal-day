using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class ClueInfo
{
    public Clue clue;
    public ClueData clueData;
    public int cluePoints;
    public float cluePercentage;
    public bool validClue;

    public ClueInfo(Clue clue, ClueData clueData, int cluePoints)
    {
        this.clue = clue;
        this.clueData = clueData;
        this.cluePoints = cluePoints;
    }
}

public class Polaroid : MonoBehaviour
{
    [SerializeField] RawImage _photoImage;
    [SerializeField] Texture _photoTexture;

    [SerializeField] Texture _testTexture;

    [SerializeField] Rigidbody _rigidbody;

    [SerializeField] PolaroidCamera _currentPolaroidCamera;
    public PolaroidCamera CurrentPolaroidCamera
    { get { return _currentPolaroidCamera; } set { _currentPolaroidCamera = value; } }

    [SerializeField] float _fadeDuration;
    [SerializeField] float _fadeTime;

    [SerializeField] List<ClueInfo> _clueInfos = new();
    public List<ClueInfo> ClueInfos => _clueInfos;

    [SerializeField] AudioSource _audioSource;

    [SerializeField] AudioClip _createPhotoClip;

    [SerializeField] List<Polaroid> _connectedPolaroids = new();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void CreatePhoto(Texture newPhotoTexture)
    {
        _photoTexture = newPhotoTexture;
        _photoImage.texture = _photoTexture;
        _rigidbody.isKinematic = true;
        StartCoroutine(FadeFromBlackToWhite());
    }

    public void InitializePhoto(List<Clue> clues = null, List<int> cluePointsVisible = null)
    {
        if (_audioSource != null && _createPhotoClip != null)
        {
            _audioSource.clip = _createPhotoClip;
            _audioSource.Play();
        }

        _photoImage.color = Color.black;
        _photoImage.texture = null;

        if (clues.Count > 0 && cluePointsVisible.Count > 0)
        {
            for (int i = 0; i < clues.Count; i++)
            {
                if (clues[i] != null)
                {
                    ClueInfo newClueData = new ClueInfo(clues[i], clues[i].ClueData, cluePointsVisible[i]);
                    _clueInfos.Add(newClueData);
                }
            }
        }
        else
        {
            Debug.Log("Clues or cluePointsVisible list is null or mismatched in length.");

            for (int i = 0; i < clues.Count; i++)
            {
                Debug.Log($"{clues[i].name}");
            }
            Debug.Log($"{clues} and {cluePointsVisible}");
        }
    }

    private IEnumerator FadeFromBlackToWhite()
    {
        _fadeTime = 0f;
        Color startColor = Color.black;
        Color endColor = Color.white;

        while (_fadeTime < _fadeDuration)
        {
            _photoImage.color = Color.Lerp(startColor, endColor, _fadeTime / _fadeDuration);
            _fadeTime += Time.deltaTime;
            yield return null;
        }

        _fadeTime = _fadeDuration;
        _photoImage.color = endColor;
    }

    public void DetachFromPolaroid()
    {
        if (_currentPolaroidCamera != null)
        {
            _currentPolaroidCamera.CurrentPhoto = null;
            _currentPolaroidCamera.FrontCollider.isTrigger = false;
            _currentPolaroidCamera = null;
        }
    }
}