using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public int lives = 3; // Number of lives
    public TextMeshProUGUI livesText; // Reference to the UI Text component
    public GameObject DeathScreen;
    public GameObject GameOverScreen;
    public GameObject livesTextObject;
    public int highestLevelReached;

    // Singleton instance
    public static GameManager instance = null;

    void Awake()
    {
        // Make this object persistent across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public void RestartGame()
    {
        lives = 3; // reset lives
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(scene.name);
        SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
        GameOverScreen.SetActive(false);
    }

    public void GoToMainMenu()
    {
        lives = 3; // reset lives
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(scene.name);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
        GameOverScreen.SetActive(false);
        livesTextObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        highestLevelReached = PlayerPrefs.GetInt("HighestLevelReached", 1);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.StartsWith("Level"))
        {
            livesTextObject.SetActive(true);
            UpdateLivesText();
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void PlayerDied()
    {
        lives--;
        UpdateLivesText();

        if (lives <= 0)
        {
            GameOverScreen.SetActive(true);
            livesText.text = "Game Over!";
        }
        else
        {
            Time.timeScale = 0; // Pause Game
            DeathScreen.SetActive(true); // Show Death Screen
            StartCoroutine(RespawnPlayer()); // After some delay respawn
        }
    }

    void UpdateLivesText()
    {
        livesText.text = "Lives: " + lives;
    }

    IEnumerator RespawnPlayer()
    {
        yield return new WaitForSecondsRealtime(2f);

        Time.timeScale = 1; // unpause game 
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(scene.name);
        SceneManager.LoadScene(scene.name, LoadSceneMode.Additive);
        DeathScreen.SetActive(false);
    }

    public void LevelCompleted(int levelNumber)
    {
        if (levelNumber > highestLevelReached)
        {
            highestLevelReached = levelNumber;
            PlayerPrefs.SetInt("HighestLevelReached", highestLevelReached);
        }

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(scene.name);
        SceneManager.LoadScene("WorldMap", LoadSceneMode.Additive);
    }
}
