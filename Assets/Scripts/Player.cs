using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    public Sprite[] sprites;

    private int spriteIndex = 0; // Index to track the current sprite

    public Vector3 direction;

    public float gravity = -9.81f; // Gravity value for the player

    public float strength = 10f;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component attached to this GameObject
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetMouseButtonDown(0)){
            direction = Vector3.up * strength;
        }

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                direction = Vector3.up * strength;
            }
        }

        direction.y += gravity * Time.deltaTime; // Apply gravity to the direction
        transform.position += direction * Time.deltaTime; // Move the player based on the direction
        transform.rotation = Quaternion.Euler(0, 0, direction.y * 2); // Rotate the player based on the vertical speed
    }

    private void AnimateSprite()
    {
        spriteIndex++;

        if (spriteIndex >= sprites.Length)
        {
            spriteIndex = 0; // Reset the index if it exceeds the number of sprites
        }

        spriteRenderer.sprite = sprites[spriteIndex]; // Update the sprite of the SpriteRenderer
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacles"))
        {
            FindAnyObjectByType<GameManager>().GameOver(); // Call the GameOver method from GameManager
        } else if (collision.CompareTag("Scoring"))
        {
            FindAnyObjectByType<GameManager>().IncreaseScore();
        }
    }
}
