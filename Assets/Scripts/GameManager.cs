using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Player player; // Reference to the Player script

    public TextMeshProUGUI scoreText; // Reference to a UI Text component to display the score

    public GameObject playButton;

    public GameObject gameOver;


    private int score;

    private void Awake()
    {

        Pause();
    }

    public void Play()
    {
        score = 0;
        scoreText.text = score.ToString();

        playButton.SetActive(false); // Hide the Play button
        gameOver.SetActive(false); // Hide the Game Over UI

        Time.timeScale = 1f; // Resume the game by setting time scale to 1
        player.enabled = true; // Enable the player script to allow player movement

        Pipes[] pipes = FindObjectsOfType<Pipes>(); // Find all Pipes in the scene

        foreach (Pipes pipe in pipes)
        {
            Destroy(pipe.gameObject); // Destroy all existing Pipes
        }
        player.transform.position = Vector3.zero; // Reset the player's position to the center of the screen
        player.transform.rotation = Quaternion.identity; // Reset the player's rotation to the default
        player.direction = Vector3.zero ;
    }



    public void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = false;// Pause the game by setting time scale to 0
    }

    public void GameOver()
    {
        gameOver.SetActive(true); // Show the Game Over UI
        playButton.SetActive(true); // Show the Play button

        Pause(); // Pause the game
    }

    public void IncreaseScore()
    {
        score+= 1;
        scoreText.text = score.ToString(); // Update the score text in the UI
    }

}
