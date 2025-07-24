using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public Player player; // Reference to the Player script

    public TextMeshProUGUI scoreText; // Reference to a UI Text component to display the score

    public GameObject spawnerObject;
    private Spawner spawner; // Reference to the Spawner script

    public GameObject playButton;

    public GameObject gameOver;

    

    public InputAction pauseAction; // Action to pause the game
    


    private int score;

    public int points
    {
        get { return score; }
    }

    bool isPaused = false; // Flag to check if the game is paused

    private void OnEnable()
    {
        pauseAction.Enable();
        spawner = spawnerObject.GetComponent<Spawner>(); // Get the Spawner component from the specified GameObject
    }

    private void OnDisable()
    {
        pauseAction.Disable();
    }


    private void Awake()
    {

        Pause();
    }

    public void Play()
    {
        spawner.isSpawned = true; // Enable spawning of pipes
        spawner.SetPublicImageColor(Color.white);
        isPaused = false; // Reset the pause flag
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
        player.transform.position = new Vector3(-7.0f, 0f, 0f); // Reset the player's position to the center of the screen
        player.transform.rotation = Quaternion.identity; // Reset the player's rotation to the default
        player.direction = Vector3.zero ;
    }



    public void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = false;// Pause the game by setting time scale to 0
        isPaused = true;
    }

    public void GameOver()
    {
        spawner.disableThePipes.SetActive(false);
        spawner.enableThePipes.SetActive(false);
        spawner.doNotHaveEnoughSign.SetActive(false);
        gameOver.SetActive(true); // Show the Game Over UI
        playButton.SetActive(true); // Show the Play button
        score = -1; // Reset the score to 0

        Pause(); // Pause the game
    }

    public void IncreaseScore()
    {
        score+= 5;
        scoreText.text = score.ToString(); // Update the score text in the UI
    }

    public void IncreaseScore(int scorePlus)
    {
        score += scorePlus;
        scoreText.text = score.ToString(); // Update the score text in the UI
    }

    private void Update()
    {

        if (isPaused && pauseAction.triggered)
        {
            Play(); // Pause the game if the Escape key is pressed
        }

        
    }

}
