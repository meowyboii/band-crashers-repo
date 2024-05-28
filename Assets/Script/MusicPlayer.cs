using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource instrumental;
    public AudioClip musicClip;
    public AudioClip instrumentalClip;

    public float BPM = 110;

    public int numberOfBeats = 2;

    public int timeSignature = 4;

    public int numberOfMeasureBeforeStart = 7;

    public List<int> songPattern = new List<int>(){
        1,1,1,1,1,1,2,2
    };


    public TurnManager turnManager;

    public Slider slider;
    public TextMesh timer;
    public Canvas canvas;
    private float currentProgress;

    public Canvas versusCanvas;



    private float duration;
    private float delayBeforeStart;
    private float newDuration;

    void Start()
    {
        duration = ((60f / BPM) * (float)timeSignature * (float)numberOfBeats)+0.02f;
        delayBeforeStart = (60f / BPM) * (float)timeSignature * (float)numberOfMeasureBeforeStart;
        Debug.Log("DELAY B4 START: " + delayBeforeStart);

        instrumental.clip = instrumentalClip;
        audioSource.clip = musicClip;

        if (canvas != null)
        {
            canvas.enabled = false;
        }
        StartCoroutine(WaitVersusScreen());
    }


    IEnumerator WaitVersusScreen()
    {
        float timer = 0f;
        float waitTime = 3.5f;

        // Wait for versus screen
        while (timer < waitTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (versusCanvas != null)
        {
            versusCanvas.enabled = false;
        }

        // Start game
        GameManager.Instance.StartGame();

        // Play both audio sources together
        instrumental.Play();
        audioSource.Play();

        float startDelay = delayBeforeStart;
        timer = 0f;
        while (timer < startDelay)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        StartAudioPlayback();
    }

    void StartAudioPlayback()
    {
        turnManager.SwitchTurn();
        // Initialize the AudioSource with the clip
        if (musicClip != null)
        {
            StartCoroutine(CyclePause());
        }
    }

    IEnumerator CyclePause()
    {
        for (int i = 0; i < songPattern.Count - 1; i++)
        {
            newDuration = duration * songPattern[i];
            Debug.Log("DURATION: " + newDuration);

            float beforeTimer = newDuration - 2.0f;
            float timer = 0f;
            while (timer < beforeTimer)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            // Start timer queue
            StartTimer();
            timer = 0f;
            while (timer < 2.0f)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            turnManager.SwitchTurn();

            // Resume after the given duration
            timer = 0f;
            while (timer < newDuration)
            {
                timer += Time.deltaTime;
                yield return null;
            }
        }

        GameManager.Instance.EndGame();
    }

    void StartTimer()
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.Playing)
        {
            StartCoroutine(UpdateTimer());
        }

    }

    IEnumerator UpdateTimer()
    {
        if (canvas != null)
        {
            canvas.enabled = true;
        }

        timer.text = "3";
        float timerDuration = 2f; // Total duration of the countdown timer
        float elapsedTime = 0f; // Time elapsed since the timer started

        while (elapsedTime < timerDuration)
        {
            // Update the timer text
            int secondsRemaining = Mathf.CeilToInt(timerDuration - elapsedTime);
            timer.text = secondsRemaining.ToString();

            // Increment the elapsed time using deltaTime
            elapsedTime += Time.deltaTime;

            // Update the slider value based on the elapsed time
            slider.value = elapsedTime / timerDuration * 120;

            yield return null; // Wait for the next frame
        }

        // Reset UI and canvas
        slider.value = 0;
        timer.text = "";
        if (canvas != null)
        {
            canvas.enabled = false;
        }

    }

    void Update()
    {
        // if (isPaused && turnManager.GetCurrentTurn() == TurnManager.PlayerTurn.Player1)
        // {
        //     turnManager.SwitchTurn();
        // }


    }

    public float GetDuration()
    {
        return newDuration;
    }
}

