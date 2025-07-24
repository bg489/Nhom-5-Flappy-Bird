using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private AudioClip jumpSound; // Audio clip for the jump sound effect
    [SerializeField] private AudioClip pipePassSound;
    [SerializeField] private AudioClip redPipePassSound;
    [SerializeField] private AudioClip ouch;
    [SerializeField] private AudioClip lose;
    private AudioSource audioSource; // Reference to the AudioSource component

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    public InputAction jumpAction; // Action to handle jumping

    public Sprite[] sprites;

    private int spriteIndex = 0; // Index to track the current sprite

    public Vector3 direction;

    public float gravity = -9.81f; // Gravity value for the player

    public float strength = 10f;

    private void OnEnable()
    {
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        jumpAction.Disable();
    }



    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component attached to this GameObject
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to this GameObject
        transform.position = new Vector3(-7.0f, 0f, 0f); // Set the initial position of the player
    }

    // Update is called once per frame
    void Update()
    {
        if(jumpAction.triggered)
        {
            direction = Vector3.up * strength;
            audioSource.PlayOneShot(jumpSound); // Play the jump sound effect
        }

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == UnityEngine.TouchPhase.Began)
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
            audioSource.PlayOneShot(ouch); // Play the ouch sound effect
            audioSource.PlayOneShot(lose); // Play the ouch sound effect
        } else if (collision.CompareTag("Scoring"))
        {
            FindAnyObjectByType<GameManager>().IncreaseScore();
            audioSource.PlayOneShot(pipePassSound);
        }
        else if (collision.CompareTag("ScoringRedPipes"))
        {
            FindAnyObjectByType<GameManager>().IncreaseScore(40);
            audioSource.PlayOneShot(redPipePassSound);
        }
    }
}
