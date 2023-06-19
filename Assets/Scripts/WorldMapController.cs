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
        int highestLevelReached = GameManager.instance.highestLevelReached + 1;

        for (int i = 0; i < levelButtons.Length; i++) 
        {
            if (i + 1 <= highestLevelReached)
            {
                levelButtons[i].interactable = true;

                var colors = levelButtons[i].colors;
                colors.normalColor = i + 1 < highestLevelReached ? Color.green : Color.blue;
                levelButtons[i].colors = colors;
            }
            else
            {
                levelButtons[i].interactable = false;

                var colors = levelButtons[i].colors;
                colors.normalColor = Color.red;
                levelButtons[i].colors = colors;
            }
        }
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