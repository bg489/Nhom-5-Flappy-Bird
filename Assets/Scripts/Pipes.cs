using UnityEngine;

public class Pipes : MonoBehaviour
{

    public float speed = 5f; // Speed of the pipes

    private float leftEdge;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1; // Get the left edge of the screen in world coordinates
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime; // Move the pipes to the left

        if (transform.position.x < leftEdge) // If the pipes move off-screen
        {
            Destroy(gameObject); // Destroy the pipes
        }
    }
}
