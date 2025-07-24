using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // This script is responsible for spawning enemies in the game
    public GameObject enemyPrefab; // Prefab of the enemy to spawn

    public float spawnInterval = 2.0f; // Time interval between spawns
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval); // Start spawning enemies at regular intervals
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Method to spawn an enemy at a random position within a specified range
    public void SpawnEnemy()
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0.7f, 1)); // Bottom-left corner of the screen
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(0.9f, 1)); // Top-right corner of the screen
        GameObject enemy = (GameObject)Instantiate(enemyPrefab, new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y)), Quaternion.identity);
        enemy.transform.rotation = Quaternion.Euler(0, 180, 0); // Rotate the enemy to face downwards
    }
}
