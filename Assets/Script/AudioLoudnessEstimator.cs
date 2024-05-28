using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoudnessEstimator : MonoBehaviour
{
    const int spectrumSize = 1024;
    float[] spectrum = new float[spectrumSize];

    public float Estimate(AudioSource audioSource)
    {
        // Check if the audio source is playing
        if (!audioSource.isPlaying) return 0f;

        // Get the spectrum data of the audio signal
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Hanning);

        // Calculate the loudness (amplitude) of the audio signal
        float loudness = 0f;
        for (int i = 0; i < spectrumSize; i++)
        {
            // Add the squared magnitude of each spectrum bin to the loudness
            loudness += spectrum[i] * spectrum[i];
        }

        // Take the square root of the sum of squared magnitudes
        loudness = Mathf.Sqrt(loudness);

        // Scale it up and round off
        loudness = Mathf.RoundToInt(loudness * 1000);

        return loudness;
    }
}