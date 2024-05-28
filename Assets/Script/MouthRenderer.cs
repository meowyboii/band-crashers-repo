using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthRenderer : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioLoudnessEstimator audioLoudnessEstimator;
    public TurnManager turnManager;
    public SpriteRenderer spriteRenderer;
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
    public Sprite sprite4;
    public Sprite sprite5;
    public bool player = true;
    private TurnManager.PlayerTurn currentPlayer;

    void Start()
    {
        InvokeRepeating(nameof(UpdateMouthSprite), 0, 1.0f / 5);
        if (player){
            currentPlayer = TurnManager.PlayerTurn.Player2;
        }
        else{
            currentPlayer = TurnManager.PlayerTurn.Player1;
        }
    }

    void UpdateMouthSprite()
    {
        if (turnManager.GetCurrentTurn() == currentPlayer)
        {
            float loudness = audioLoudnessEstimator.Estimate(audioSource);
            // Debug.Log("LOUDNESS: " + loudness);
            if (loudness <= 5)
            {
                spriteRenderer.sprite = sprite1;
            }
            else if (loudness <= 20)
            {
                spriteRenderer.sprite = sprite2;
            }
            else if (loudness <= 30)
            {
                spriteRenderer.sprite = sprite3;
            }
            else if (loudness <= 55)
            {
                spriteRenderer.sprite = sprite4;
            }
            else
            {
                spriteRenderer.sprite = sprite5;
            }
        }
        else
        {
            spriteRenderer.sprite = sprite1;
        }

    }
}
