using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class VrButton : MonoBehaviour
{

    [SerializeField] GameObject button;
    [SerializeField] UnityEvent onPress;
    [SerializeField] UnityEvent onRelease;
    GameObject presser;

    [SerializeField] float pressedPosition = 0.02f;
    [SerializeField] float releasedPosition = 0.04f;
    bool isPressed;
    Vector3 initialLocalPosition;

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip[] btnsounds;

    private void Start()
    {
        initialLocalPosition = button.transform.localPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            // Move button along its local up axis
            Vector3 newPosition = initialLocalPosition;
            newPosition.y = pressedPosition;
            button.transform.localPosition = newPosition;

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
            // Return to original position
            Vector3 newPosition = initialLocalPosition;
            newPosition.y = releasedPosition;
            button.transform.localPosition = newPosition;

            onRelease.Invoke();
            isPressed = false;
        }
    }

    private void BTNSound()
    {
        if (btnsounds.Length == 0) return;
        source.pitch = Random.Range(0.5f, 1.5f);
        source.PlayOneShot(btnsounds[Random.Range(0, btnsounds.Length)]);

    }
}
