using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorldMapController : MonoBehaviour
{
    public Button[] levelButtons; // The buttons for the levels

    void Start()
    {

    }

    // Load a level when its button is clicked
    public void LoadLevel(string levelName)
    {
        SceneManager.UnloadSceneAsync("WorldMap");
        SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
    }

    public void GoToMainMenu()
    {
        SceneManager.UnloadSceneAsync("WorldMap");
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
    }

    public void GoToShop()
    {
        SceneManager.UnloadSceneAsync("WorldMap");
        SceneManager.LoadScene("Shop", LoadSceneMode.Additive);
    }
}