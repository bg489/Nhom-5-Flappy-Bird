using UnityEngine;

public class EnemyController : MonoBehaviour
{
    float speed;
    public GameObject explosionPrefab; // Reference to the explosion prefab
    [SerializeField] private AudioClip explosionSound;
    AudioSource audioSource;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = 2f;
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to this GameObject
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;
        position = new Vector2(position.x, position.y - speed * Time.deltaTime);
        transform.position = position;
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        if (transform.position.y < min.y)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MyBullet"))
        {
            audioSource.PlayOneShot(explosionSound); // Play the explosion sound effect
            FindAnyObjectByType<GameManager>().IncreaseScore(15); // Increase the score in the GameManager
            GameObject afterExplore = (GameObject)Instantiate(explosionPrefab, collision.transform.position, collision.transform.rotation);
            Destroy(afterExplore, 0.5f); // Destroy the explosion effect after 0.5 seconds
            Destroy(collision.gameObject); // Destroy the bullet
            Destroy(gameObject); // Destroy the enemy
        }
    }
}
