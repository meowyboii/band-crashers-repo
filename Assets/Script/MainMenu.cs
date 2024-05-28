using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        StartCoroutine(LoadAsync(1));
        GameManager.Instance.StartGame();

        FindObjectOfType<AudioManager>().Play("VersusTheme");
        FindObjectOfType<AudioManager>().Play("ClickSound");
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            Debug.Log(progress);
            yield return null;
        }
    }

    public void QuitGame()
    {
        Debug.Log("QUITTED!!!!!");
        Application.Quit();
    }

    public void RestartGame(){
        // Get the index of the current scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        // Reload the current scene
        SceneManager.LoadScene(currentSceneIndex);
    }
}
