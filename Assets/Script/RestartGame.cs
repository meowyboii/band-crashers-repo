using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void Restart(){

        FindObjectOfType<AudioManager>().Play("VersusTheme");
        // Get the index of the current scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        // Reload the current scene
        SceneManager.LoadScene(currentSceneIndex);
    }
    public void GoHome()
    {
        SceneManager.LoadScene("Main Menu"); 
    }
}
