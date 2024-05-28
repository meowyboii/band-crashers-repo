using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PitchVisualizer : MonoBehaviour
{
    public AudioSource botAudioSource;
    public AudioSource playerAudioSource;
    public AudioPitchEstimator botEstimator;
    public AudioPitchEstimator playerEstimator;
    public LineRenderer lineSRH;
    public LineRenderer lineFrequency;
    public Transform marker;
    public TextMesh textFrequency;
    public TextMesh textMin;
    public TextMesh textMax;

    public AudioLoudnessEstimator audioLoudnessEstimator;

    public float estimateRate = 30;

    public TurnManager turnManager;

    private List<float> originalFrequencies = new List<float>();
    private List<float> playerFrequencies = new List<float>();

    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private Performance performance;


    void Start()
    {
        // Call at regular intervals
        InvokeRepeating(nameof(UpdateVisualizer), 0, 1.0f / estimateRate);
        InvokeRepeating(nameof(UpdatePrompt), 0, 1.0f / 3);
    }

    void UpdateVisualizer()
    {
        AudioSource audioSource = GetCurrentAudioSource();
        AudioPitchEstimator estimator = GetCurrentEstimator();

        var frequency = estimator.Estimate(audioSource);

        // SRH Score
        var srh = estimator.SRH;
        var numPoints = srh.Count;
        var positions = new Vector3[numPoints];
        for (int i = 0; i < numPoints; i++)
        {
            var position = (float)i / numPoints - 0.5f;
            var value = srh[i] * 0.005f;
            positions[i].Set(position, value, 0);
        }
        lineSRH.positionCount = numPoints;
        lineSRH.SetPositions(positions);

        // Silent
        if (float.IsNaN(frequency))
        {
            lineFrequency.positionCount = 0;
        }
        else
        {
            var min = estimator.frequencyMin;
            var max = estimator.frequencyMax;
            var position = (frequency - min) / (max - min) - 0.5f;

            lineFrequency.positionCount = 2;
            lineFrequency.SetPosition(0, new Vector3(position, +1, 0));
            lineFrequency.SetPosition(1, new Vector3(position, -1, 0));

            marker.position = new Vector3(position, 0, 0);
            textFrequency.text = string.Format("{0}\n{1:0.0} Hz", GetNameFromFrequency(frequency), frequency);
        }


        // Lower limit/upper limit number of cycles
        textMin.text = string.Format("{0} Hz", estimator.frequencyMin);
        textMax.text = string.Format("{0} Hz", estimator.frequencyMax);

        if (turnManager.GetCurrentTurn() == TurnManager.PlayerTurn.Player1)
        {
            originalFrequencies.Add(float.IsNaN(frequency) ? 0f : frequency);
        }
        else if (turnManager.GetCurrentTurn() == TurnManager.PlayerTurn.Player2)
        {
            playerFrequencies.Add(float.IsNaN(frequency) ? 0f : frequency);

        }

    }

    public void UpdatePrompt()
    {
        if (turnManager.GetCurrentTurn() == TurnManager.PlayerTurn.Player2)
        {
            int index = Mathf.Min(playerFrequencies.Count - 1, originalFrequencies.Count - 1);

            float pitchDiff = 0;

            if (index >= 0)
            {
                pitchDiff = Mathf.Abs(originalFrequencies[index] - playerFrequencies[index]);
            }

            if (pitchDiff > 50)
            {
               performance.ShowFloatingPerformance(0);
            }
            else if (pitchDiff > 30)
            {
                performance.ShowFloatingPerformance(1);
            }
            else if (pitchDiff > 10)
            {
                performance.ShowFloatingPerformance(2);
            }
            else
            {
                performance.ShowFloatingPerformance(3);
            }

        }
    }
    // 周波数 → 音名 Frequency → Note Name
    string GetNameFromFrequency(float frequency)
    {
        var noteNumber = Mathf.RoundToInt(12 * Mathf.Log(frequency / 440) / Mathf.Log(2) + 69);
        string[] names = {
            "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"
        };
        return names[noteNumber % 12];
    }
    AudioSource GetCurrentAudioSource()
    {
        if (turnManager.GetCurrentTurn() == TurnManager.PlayerTurn.Player1)
        {
            return botAudioSource;
        }
        else
        {
            return playerAudioSource;
        }
    }

    AudioPitchEstimator GetCurrentEstimator()
    {
        if (turnManager.GetCurrentTurn() == TurnManager.PlayerTurn.Player1)
        {
            return botEstimator;
        }
        else
        {
            return playerEstimator;
        }
    }

    public List<float> GetPlayerFrequencies()
    {
        return playerFrequencies;
    }

    public List<float> GetOriginalFrequencies()
    {
        return originalFrequencies;
    }

    public void DeleteFrequencies()
    {
        originalFrequencies.Clear();
        playerFrequencies.Clear();
    }

}
