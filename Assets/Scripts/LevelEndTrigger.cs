using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Assuming the GameManager script is attached to a GameObject called "GameManager"
            GameManager.instance.LevelCompleted(SceneManager.GetActiveScene().buildIndex);
        }
    }
}