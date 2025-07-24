using UnityEngine;

public class Begin : MonoBehaviour
{
    public float speed = 20; // Speed of the bullet
    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to this GameObject
        Destroy(gameObject, 0.15f); // Destroy the bullet after 2 seconds to prevent memory leaks
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
