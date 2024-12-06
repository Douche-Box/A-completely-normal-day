using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] TMP_InputField _masterInput;
    [SerializeField] TMP_InputField _musicInput;
    [SerializeField] TMP_InputField _sfxInput;

    [SerializeField] Slider _masterSlider;
    [SerializeField] Slider _musicSlider;
    [SerializeField] Slider _sfxSlider;

    [SerializeField] GameObject _audioPanel;

    [Header("VR")]

    [SerializeField] bool _snapOrSmooth;

    [SerializeField] float _snapAngle;
    [SerializeField] float _smoothSpeed;


    void Start()
    {
        #region Audio Start

        // Master Volume
        float masterVol = PlayerPrefs.GetFloat("MasterVol", 1f);
        InitializeVolume("MasterVol", masterVol, _masterSlider, _masterInput);

        // Music Volume
        float musicVol = PlayerPrefs.GetFloat("MusicVol", 1f);
        InitializeVolume("MusicVol", musicVol, _musicSlider, _musicInput);

        // SFX Volume
        float sfxVol = PlayerPrefs.GetFloat("SfxVol", 1f);
        InitializeVolume("SFXVol", sfxVol, _sfxSlider, _sfxInput);

        _snapOrSmooth = PlayerPrefs.GetInt("SnapOrSmooth") == 1;
        
        #endregion
    }

    void InitializeVolume(string volumeParameter, float volumeLevel, Slider slider, TMP_InputField inputField)
    {
        // Set volume based on whether volume level is zero or non-zero
        if (volumeLevel > 0)
        {
            _audioMixer.SetFloat(volumeParameter, Mathf.Log10(volumeLevel) * 20);
        }
        else
        {
            _audioMixer.SetFloat(volumeParameter, -80f); // Minimum dB level when volume level is 0
        }

        // Set slider and input field values
        if (slider != null)
        {
            slider.value = volumeLevel;
        }
        if (inputField != null)
        {
            inputField.text = (volumeLevel * 100).ToString("0");
        }
    }

    #region Audio
    public void SetMasterVol(float masterLvl)
    {
        if (masterLvl > 0)
        {
            _audioMixer.SetFloat("MasterVol", Mathf.Log10(masterLvl) * 20);
        }
        else
        {
            _audioMixer.SetFloat("MasterVol", -80f); // Set to a minimum dB level when slider is at 0
        }
        PlayerPrefs.SetFloat("MasterVol", masterLvl);
        _masterInput.text = (_masterSlider.value * 100).ToString("0");
    }

    public void SetMusicVol(float musicLvl)
    {
        if (musicLvl > 0)
        {
            _audioMixer.SetFloat("MusicVol", Mathf.Log10(musicLvl) * 20);
        }
        else
        {
            _audioMixer.SetFloat("MusicVol", -80f); // Set to a minimum dB level when slider is at 0
        }
        PlayerPrefs.SetFloat("MusicVol", musicLvl);
        _musicInput.text = (_musicSlider.value * 100).ToString("0");
    }

    public void SetSFXVol(float sfxLvl)
    {
        if (sfxLvl > 0)
        {
            _audioMixer.SetFloat("SFXVol", Mathf.Log10(sfxLvl) * 20);
        }
        else
        {
            _audioMixer.SetFloat("SFXVol", -80f); // Set to a minimum dB level when slider is at 0
        }
        PlayerPrefs.SetFloat("SfxVol", sfxLvl);
        _sfxInput.text = (_sfxSlider.value * 100).ToString("0");
    }

    public void SetMasterVolInput()
    {
        float f;

        float.TryParse(_masterInput.text, out f);
        f /= 100;
        if (f < _masterSlider.minValue)
        {
            f = _masterSlider.minValue;
            _masterSlider.value = f;
        }
        else if (f > _masterSlider.maxValue)
        {
            f = _masterSlider.maxValue;
            _masterSlider.value = f;
        }
        else
        {
            _masterSlider.value = f;
        }

        _audioMixer.SetFloat("MasterVol", Mathf.Log10(f) * 20);
        _masterInput.text = (f * 100).ToString("0");
    }

    public void SetMusicVolInput()
    {
        float f;

        float.TryParse(_musicInput.text, out f);
        f /= 100;
        if (f < _musicSlider.minValue)
        {
            f = _musicSlider.minValue;
            _musicSlider.value = f;
        }
        else if (f > _musicSlider.maxValue)
        {
            f = _musicSlider.maxValue;
            _musicSlider.value = f;
        }
        else
        {
            _musicSlider.value = f;
        }
        _audioMixer.SetFloat("MusicVol", Mathf.Log10(f) * 20);
        _musicInput.text = (f * 100).ToString("0");
    }

    public void SetSfxVolInput()
    {
        float f;

        float.TryParse(_sfxInput.text, out f);
        f /= 100;
        if (f < _sfxSlider.minValue)
        {
            f = _sfxSlider.minValue;
            _sfxSlider.value = f;
        }
        else if (f > _sfxSlider.maxValue)
        {
            f = _sfxSlider.maxValue;
            _sfxSlider.value = f;
        }
        else
        {
            _sfxSlider.value = f;
        }
        _audioMixer.SetFloat("SFXVol", Mathf.Log10(f) * 20);

        _sfxInput.text = (f * 100).ToString("0");
    }

    #endregion

    public void AudioBtn()
    {
        _audioPanel.SetActive(true);
    }

}