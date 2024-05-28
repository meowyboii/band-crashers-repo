using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnManager : MonoBehaviour
{
    public MusicPlayer musicPlayer;
    public PitchVisualizer pitchVisualizer;
    public ScoreManager scoreManager;
    public TextMesh scoreText;
    public Slider slider;
    public TextMeshProUGUI health;
    public float botScore = 70f;

    public TextMeshProUGUI accuracy;
    public Canvas gameOverCanvas;

    private AudioManager audioManager;


    // Enum for player turn states
    public enum PlayerTurn
    {
        Player1,
        Player2,
        Neutral
    }

    // Current player turn
    public PlayerTurn currentPlayerTurn;


    // Start is called before the first frame update
    void Start()
    {
        // Set initial player turn
        currentPlayerTurn = PlayerTurn.Neutral;

        // Set slider value to 50%
        float maxValue = slider.maxValue;
        slider.value = maxValue / 2;
        health.text = "Health: " + slider.value.ToString();


        //Hide game over canvas
        if (gameOverCanvas != null)
        {
            gameOverCanvas.enabled = false;
        }

        audioManager = AudioManager.instance;

    }

    void Update(){
        if (slider.value<=0){
            GameManager.Instance.EndGame();
        }
    }

    // Method to switch to the next player's turn
    public void SwitchTurn()
    {
        // Toggle between Player1 and Player2 turns
        currentPlayerTurn = (currentPlayerTurn == PlayerTurn.Player1) ? PlayerTurn.Player2 : PlayerTurn.Player1;

        Debug.Log("Current turn: " + currentPlayerTurn);

        // If the current player is Player 2, start the coroutine to switch back to Player 1
        if (currentPlayerTurn == PlayerTurn.Player2)
        {
            StartCoroutine(SwitchBackToPlayer1());
        }
    }


    // Method to get the current player turn
    public PlayerTurn GetCurrentTurn()
    {
        return currentPlayerTurn;
    }

    // Coroutine to switch back to Player 1 after a delay
    IEnumerator SwitchBackToPlayer1()
    {
        yield return new WaitForSeconds(musicPlayer.GetDuration());


        string listAsString = string.Join(", ", pitchVisualizer.GetPlayerFrequencies());
        string listAsString2 = string.Join(", ", pitchVisualizer.GetOriginalFrequencies());
        Debug.Log("ORIGINAL: " + listAsString2);
        Debug.Log("PLAYER: " + listAsString);

        float score = scoreManager.CalculateScore();
        string scoreText = score.ToString();
        Debug.Log("Score: " + scoreText);


        //Calculate player and bot score difference and reveal score gained
        float scoreDiff = score - botScore;
        if (scoreDiff < 0)
        {
            scoreManager.ShowFloatingText("Accuracy: " + scoreText + "%\n" + scoreDiff + "!");
        }
        else
        {
            scoreManager.ShowFloatingText("Accuracy: " + scoreText + "%\n" + "+ " + scoreDiff + "!");
        }

        //Update the health slider
        float sliderScore = slider.value + scoreDiff;

        if (sliderScore < 0)
        {
            slider.value = 0;
        }
        else
        {
            slider.value = sliderScore;
        }

        //Update health value
        health.text = "Health: " + slider.value.ToString();

        // scoreText.text = "Original Freqs: " + listAsString2 + "\nPlayer Freqs: " + listAsString + "\nScore: " + score;

        // Switch back to Player 1
        currentPlayerTurn = PlayerTurn.Player1;
        pitchVisualizer.DeleteFrequencies();
    }

    public void ShowGameOver()
    {
       StartCoroutine(DelayGameOver());
    }
    IEnumerator DelayGameOver(){
        yield return new WaitForSeconds(2);
        string result;
        if (gameOverCanvas != null)
        {
            if (slider.value<=0){
                result = "YOU LOSE!\n";
            }
            else{
                result = "YOU WIN!\n";
                //win audio
            }
            string message = result + "Score: " + scoreManager.GetAverageAccuracy() + "%";
            accuracy.text = message;
            gameOverCanvas.enabled = true;
        }
    }

}