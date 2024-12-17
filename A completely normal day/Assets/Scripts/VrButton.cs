using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class VrButton : MonoBehaviour
{

    [SerializeField] private GameObject button;
    [SerializeField] private UnityEvent onPress;
    [SerializeField] private UnityEvent onRelease;
    private GameObject presser;
    public float buttonHight;
    public float minHight;

    [SerializeField] private bool isPressed;

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] btnsounds;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            button.transform.localPosition = new Vector3(0, minHight, 0);
            presser = other.gameObject;
            onPress.Invoke();
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == presser.gameObject)
        {
            BTNSound();
            button.transform.localPosition = new Vector3(0, buttonHight, 0);
            onRelease.Invoke();
            isPressed = false;
        }
    }

    private void BTNSound()
    {
        source.pitch = Random.Range(0.5f, 1.5f);
        source.PlayOneShot(btnsounds[Random.Range(0, btnsounds.Length)]);
        
    }
}
