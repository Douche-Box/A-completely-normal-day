using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json.Linq;

public class VoiceRecognition : MonoBehaviour
{
    private AudioClip recording;
    private string witAiToken = "YOUR_WIT_AI_ACCESS_TOKEN";  // Your Wit.ai access token

    // Start recording when a key is pressed (or however you want to trigger it)
    public void StartRecording()
    {
        recording = Microphone.Start(null, false, 10, 16000);  // 10 seconds recording
    }

    // Stop recording and send it to Wit.ai
    public void StopRecordingAndSend()
    {
        Microphone.End(null);
        byte[] audioData = WavUtility.FromAudioClip(recording);  // Convert AudioClip to byte array
        StartCoroutine(SendToWitAi(audioData));
    }

    // Send the recorded audio to Wit.ai for recognition
    private IEnumerator SendToWitAi(byte[] audioData)
    {
        string url = "https://api.wit.ai/speech?v=20221109";  // Wit.ai Speech API endpoint
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(audioData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + witAiToken);
        request.SetRequestHeader("Content-Type", "audio/wav");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            Debug.Log("Wit.ai Response: " + response);  // Handle the response here
            ProcessResponse(response);  // Process the recognized text and intent
        }
        else
        {
            Debug.LogError("Error with Wit.ai request: " + request.error);
        }
    }

    // Process the response from Wit.ai (e.g., check for specific words and intents)
    private void ProcessResponse(string response)
    {
        // Parse the JSON response from Wit.ai
        JObject jsonResponse = JObject.Parse(response);
        string recognizedText = jsonResponse["text"].ToString();  // This is what the user said
        Debug.Log("Recognized Text: " + recognizedText);

        // Optionally, you can extract the intent
        string intentName = jsonResponse["intents"]?[0]?["name"]?.ToString() ?? "";
        Debug.Log("Recognized Intent: " + intentName);

        // Optionally, you can extract entities (e.g., objects or actions)
        string entityName = jsonResponse["entities"]?["device"]?[0]?["value"]?.ToString() ?? "";
        Debug.Log("Recognized Entity: " + entityName);

        // Check if the recognized text contains the expected command
        if (recognizedText.ToLower().Contains("turn on the lights"))
        {
            Debug.Log("Command to turn on the lights recognized!");
            TurnOnLights();
        }
        else if (recognizedText.ToLower().Contains("turn off the lights"))
        {
            Debug.Log("Command to turn off the lights recognized!");
            TurnOffLights();
        }
        else
        {
            Debug.Log("Unrecognized command.");
        }
    }

    // Example method to turn on the lights
    private void TurnOnLights()
    {
        GameObject lightObject = GameObject.Find("Light");
        if (lightObject != null)
        {
            lightObject.SetActive(true);  // Turn the light on
        }
    }

    // Example method to turn off the lights
    private void TurnOffLights()
    {
        GameObject lightObject = GameObject.Find("Light");
        if (lightObject != null)
        {
            lightObject.SetActive(false);  // Turn the light off
        }
    }
}