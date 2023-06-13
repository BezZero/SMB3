using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    void Start()
    {
        Scene uiScene = SceneManager.GetSceneByName("UIScene");
        if (!uiScene.isLoaded) // Check if the UIScene is already loaded
        {
            // Load the UIScene additively
            SceneManager.LoadScene("UIScene", LoadSceneMode.Additive);
        }
    }

    public void NewGame()
    {
        SceneManager.UnloadSceneAsync("MainMenu");
        SceneManager.LoadScene("WorldMap", LoadSceneMode.Additive);
    }

    public void LoadGame()
    {
        // You will need to implement your own save/load system here
        Debug.Log("Load Game button clicked!");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit button clicked!"); // Since Application.Quit() doesn't work in the editor, this log message can help confirm the button is working
    }
}
