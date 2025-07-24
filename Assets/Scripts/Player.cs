using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private AudioClip jumpSound; // Audio clip for the jump sound effect
    [SerializeField] private AudioClip pipePassSound;
    [SerializeField] private AudioClip redPipePassSound;
    [SerializeField] private AudioClip ouch;
    [SerializeField] private AudioClip lose;
    [SerializeField] private AudioClip shieldBlockPipes;
    [SerializeField] private AudioClip openShield;
    [SerializeField] private AudioClip doNotHaveEnoughPoint;
    private AudioSource audioSource; // Reference to the AudioSource component

    public GameObject doNotHaveEnoughSign;

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    public InputAction jumpAction; // Action to handle jumping

    public Sprite[] sprites;

    private int spriteIndex = 0; // Index to track the current sprite

    public Vector3 direction;

    public float gravity = -9.81f; // Gravity value for the player

    public float strength = 10f;

    public bool isShielded = false; // Flag to check if the player is shielded

    public GameObject shieldGraphic;
    public GameObject shieldButton;
    public InputAction shieldAction; // Action to pause the game

    private Image buttonImage;

    public GameObject gameManagerObject;
    private GameManager gameManager;

    private void OnEnable()
    {
        jumpAction.Enable();
        shieldAction.Enable(); // Enable the shield action
    }

    private void OnDisable()
    {
        jumpAction.Disable();
        shieldAction.Disable(); // Disable the shield action
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
        buttonImage = shieldButton.GetComponent<Image>();
        gameManager = gameManagerObject.GetComponent<GameManager>(); // Get the GameManager component from the specified GameObject
    }

    private IEnumerator TemporarilyShieldAction(float duration)
    {
        gameManager.IncreaseScore(-50); // Decrease score by 50 points
        audioSource.PlayOneShot(openShield);
        shieldGraphic.SetActive(true);
        isShielded = true;
        buttonImage.color = Color.red;

        float blinkStartTime = duration - 1.5f;
        yield return new WaitForSeconds(blinkStartTime);

        
        float blinkDuration = duration - blinkStartTime;
        float blinkInterval = 0.2f;
        float elapsed = 0f;

        while (elapsed < blinkDuration)
        {
            shieldGraphic.SetActive(!shieldGraphic.activeSelf);
            buttonImage.color = shieldGraphic.activeSelf ? Color.red : Color.white;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        
        shieldGraphic.SetActive(false);
        isShielded = false;
        buttonImage.color = Color.white;
    }

    private IEnumerator TemporarilyDoNotHaveEnoughPipes(float duration)
    {
        audioSource.PlayOneShot(doNotHaveEnoughPoint);
        doNotHaveEnoughSign.SetActive(true);
        yield return new WaitForSeconds(duration);
        doNotHaveEnoughSign.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (shieldAction.triggered && !isShielded)
        {
            if (gameManager.points < 0)
            {
                return;
            }
            else if (gameManager.points >= 50)
            {
                StartCoroutine(TemporarilyShieldAction(5f));
            }
            else
            {
                StartCoroutine(TemporarilyDoNotHaveEnoughPipes(2f));
                return;
            }
        }

        if (jumpAction.triggered)
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
        if (collision.CompareTag("Ground"))
        {
            FindAnyObjectByType<GameManager>().GameOver(); // Call the GameOver method from GameManager
            audioSource.PlayOneShot(ouch); // Play the ouch sound effect
            audioSource.PlayOneShot(lose); // Play the ouch sound effect
        } else if (collision.CompareTag("Obstacles"))
        {
            if(isShielded) // Check if the player is shielded
            {
                audioSource.PlayOneShot(shieldBlockPipes); // Play the ouch sound effect
                return; // Exit the method to prevent further actions
            }
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
