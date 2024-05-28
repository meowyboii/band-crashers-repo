using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance;
    private TurnManager turnManager;
    private MusicPlayer musicPlayer;

    // Game state
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }
    public GameState CurrentGameState { get; private set; }

    private void Awake()
    {
        // Ensure only one instance of the GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scene changes

        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize the game
        SetGameState(GameState.MainMenu);
    }

    public void StartGame()
    {
        SetGameState(GameState.Playing);
    }

    public void PauseGame()
    {
        SetGameState(GameState.Paused);
    }

    public void ResumeGame()
    {
        SetGameState(GameState.Playing);
    }

    public void EndGame()
    {
        SetGameState(GameState.GameOver);
    }

    private void SetGameState(GameState newState)
    {
        CurrentGameState = newState;

        // Perform actions based on the new game state
        switch (newState)
        {
            case GameState.MainMenu:
                // Show main menu UI
                // Hide other UI elements
                break;
            case GameState.Playing:
                // Start game logic
                // Show gameplay UI
                // Hide main menu UI
                break;
            case GameState.Paused:
                // Pause game logic
                // Show pause menu UI
                break;
            case GameState.GameOver:
                turnManager = GameObject.Find("TurnManager").GetComponent<TurnManager>();
                musicPlayer = GameObject.Find("Music Bot").GetComponent<MusicPlayer>();
                turnManager.currentPlayerTurn = TurnManager.PlayerTurn.Neutral;
                turnManager.ShowGameOver();
                musicPlayer.audioSource.Stop();
                break;
        }
        Debug.Log(CurrentGameState);
    }
}