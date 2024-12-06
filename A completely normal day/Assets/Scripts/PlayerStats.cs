using System;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [SerializeField] bool _snapOrSmoothTurn;

    [SerializeField] float _snapTurnAngle;

    [SerializeField] float _smoothTurnSpeed;

    [SerializeField] string _gameSceneName = "Game";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == _gameSceneName)
        {
            LoadPlayerStats();
        }
    }


    void LoadPlayerStats()
    {
        if (!_snapOrSmoothTurn)
        {
            GameObject xrOrigin = FindObjectOfType<XROrigin>().gameObject;

            if (xrOrigin != null)
            {
                xrOrigin.GetComponent<SnapTurnProviderBase>().enabled = true;
                xrOrigin.GetComponent<SnapTurnProviderBase>().turnAmount = _snapTurnAngle;
                xrOrigin.GetComponent<ContinuousTurnProviderBase>().enabled = false;
            }

        }
        else
        {
            GameObject xrOrigin = FindObjectOfType<XROrigin>().gameObject;

            if (xrOrigin != null)
            {
                xrOrigin.GetComponent<SnapTurnProviderBase>().enabled = false;
                xrOrigin.GetComponent<ContinuousTurnProviderBase>().enabled = true;
                xrOrigin.GetComponent<ContinuousTurnProviderBase>().turnSpeed = _smoothTurnSpeed;
            }
        }
    }

    /// <summary>
    /// Update current setting to not change settings use -1
    /// </summary>
    /// <param name="newSnapOrSmooth"></param>
    /// <param name="newSnapValue"></param>
    /// <param name="newSmoothValue"></param>
    public void UpdateSettings(int newSnapOrSmooth = -1, float newSnapValue = -1, float newSmoothValue = -1)
    {
        if (newSnapOrSmooth != -1)
        {
            _snapOrSmoothTurn = newSnapOrSmooth == 1;
        }

        if (newSnapValue != -1)
        {
            _snapTurnAngle = newSnapValue;
        }

        if (newSmoothValue != -1)
        {
            _smoothTurnSpeed = newSmoothValue;
        }
    }
}
