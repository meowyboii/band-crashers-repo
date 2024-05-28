// using System.Collections;
// using UnityEngine;

// public class AudioRecorder : MonoBehaviour
// {
//     public AudioSource audioSource;
//     public int duration = 8;

//     void Start()
//     {
//         StartCoroutine(StartRecording());
//     }

//     IEnumerator StartRecording()
//     {
//         // Request microphone permissions for Android
//         yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);

//         // Check if microphone permissions are granted
//         if (Application.HasUserAuthorization(UserAuthorization.Microphone))
//         {
//             // Start recording
//             audioSource.clip = Microphone.Start(null, true, duration, AudioSettings.outputSampleRate);
//             audioSource.Play();
//             Debug.Log("Audio clip length: " + audioSource.clip.length);
//             Debug.Log("Microphone permissions granted.");
//         }
//         else
//         {
//             Debug.LogError("Microphone permissions not granted.");
//         }
//     }
// }

//using System.Collections;
//using UnityEngine;

//public class AudioRecorder : MonoBehaviour
//{
//    public AudioSource audioSource;

//    void Start()
//    {
//        // Start recording from the microphone
//        StartRecording();
//    }

//    void StartRecording()
//    {
//        // Request microphone permissions for Android
//        StartCoroutine(RequestMicrophonePermission());
//    }

//    IEnumerator RequestMicrophonePermission()
//    {
//        // Request microphone permissions for Android
//        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);

//        // Check if microphone permissions are granted
//        if (Application.HasUserAuthorization(UserAuthorization.Microphone))
//        {
//            // Start recording indefinitely
//            audioSource.clip = Microphone.Start(null, true, 3500, AudioSettings.outputSampleRate);
//            audioSource.loop = true;
//            while (!(Microphone.GetPosition(null) > 0)) { } // Wait until recording starts
//            audioSource.Play();
//            Debug.Log("Microphone permissions granted. Recording started.");
//        }
//        else
//        {
//            Debug.LogError("Microphone permissions not granted.");
//        }
//    }

//    // You may want to add a method to stop recording if needed
//    void StopRecording()
//    {
//        Microphone.End(null);
//        audioSource.Stop();
//        Debug.Log("Recording stopped.");
//    }
//}

using System.Collections;
using UnityEngine;

public class AudioRecorder : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        // Start recording from the microphone
        StartRecording();
    }

    void StartRecording()
    {
        // Request microphone permissions for Android
        StartCoroutine(RequestMicrophonePermission());
    }

    IEnumerator RequestMicrophonePermission()
    {
        // Request microphone permissions for Android
        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);

        // Check if microphone permissions are granted
        if (Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            // Start recording indefinitely
            AudioClip recording = Microphone.Start(null, true, 1, AudioSettings.outputSampleRate);
            audioSource.clip = recording;
            audioSource.loop = true;

            // Check if microphone recording has started
            while (Microphone.GetPosition(null) <= 0) { yield return null; }

            // Play the recording live
            audioSource.Play();

            Debug.Log("Microphone permissions granted. Live recording started.");
        }
        else
        {
            Debug.LogError("Microphone permissions not granted.");
        }
    }

    // You may want to add a method to stop recording if needed
    void StopRecording()
    {
        Microphone.End(null);
        audioSource.Stop();
        Debug.Log("Live recording stopped.");
    }
}