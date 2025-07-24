using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public InputAction spawnAction; // Action để bật/tắt spawning

    public GameObject disableThePipes;
    public GameObject enableThePipes;
    public GameObject disableThePipesButton;
    public GameObject doNotHaveEnoughSign;
    public GameObject gameManagerObject;
    private GameManager gameManager;

    [SerializeField] private AudioClip enable;
    [SerializeField] private AudioClip disable;
    [SerializeField] private AudioClip doNotHaveEnoughPoint;

    private Image buttonImage;

    private AudioSource audioSource; // Reference to the AudioSource component

    public GameObject prefab;
    public GameObject prefab2;
    public float spawnRate = 1f;
    public float minHeight = -1f;
    public float maxHeight = 1f;

    private bool wasSpawned;
    public bool isSpawned = true;

    private void OnEnable()
    {
        spawnAction.Enable();
        spawnAction.performed += OnSpawnActionPerformed;
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to this GameObject
        buttonImage = disableThePipesButton.GetComponent<Image>();
        gameManager = gameManagerObject.GetComponent<GameManager>(); // Get the GameManager component from the specified GameObject
    }

    private void OnDisable()
    {
        spawnAction.performed -= OnSpawnActionPerformed;
        spawnAction.Disable();
    }

    public void SetPublicImageColor(Color color)
    {
        buttonImage.color = color; // Set the button image color to the specified color
    }

    private IEnumerator TemporarilyDisablenablePipes(float duration)
    {
        gameManager.IncreaseScore(-100); // Decrease score by 100 points
        Pipes[] pipes = FindObjectsOfType<Pipes>(); // Find all Pipes in the scene

        foreach (Pipes pipe in pipes)
        {
            Destroy(pipe.gameObject); // Destroy all existing Pipes
        }
        buttonImage.color = Color.red; // Change button color to red when pipes are disabled
        audioSource.PlayOneShot(disable);
        disableThePipes.SetActive(true); // bật
        yield return new WaitForSeconds(duration); // đợi x giây
        disableThePipes.SetActive(false); // tắt
    }

    private IEnumerator TemporarilyEnablePipes(float duration)
    {
        buttonImage.color = Color.white; // Change button color to green when pipes are enabled
        audioSource.PlayOneShot(enable);
        enableThePipes.SetActive(true); // bật
        yield return new WaitForSeconds(duration); // đợi x giây
        enableThePipes.SetActive(false); // tắt
    }

    private IEnumerator TemporarilyDoNotHaveEnoughPipes(float duration)
    {
        audioSource.PlayOneShot(doNotHaveEnoughPoint); // Phát âm thanh khi không đủ điểm
        doNotHaveEnoughSign.SetActive(true); // bật
        yield return new WaitForSeconds(duration); // đợi x giây
        doNotHaveEnoughSign.SetActive(false); // tắt
    }


    private void OnSpawnActionPerformed(InputAction.CallbackContext context)
    {
        // Toggle trạng thái bật/tắt spawn khi nhấn phím
        if (isSpawned)
        {
            if (gameManager.points < 0)
            {
                return;
            }
            if (gameManager.points >= 100)
            {
                StartCoroutine(TemporarilyDisablenablePipes(2f)); // bật trong 2 giây
            }
            else
            {
                StartCoroutine(TemporarilyDoNotHaveEnoughPipes(2f)); // bật trong 2 giây
                return; // Không thực hiện việc tắt spawn nếu không đủ điểm
            }
            
        } else
        {
            if (gameManager.points < 0)
            {
                return;
            }
            StartCoroutine(TemporarilyEnablePipes(2f)); // bật trong 2 giây
        }

        isSpawned = !isSpawned;
    }

    private void Spawn()
    {
        GameObject selectedPrefab = Random.value < 0.8f ? prefab : prefab2;

        GameObject pipes = Instantiate(selectedPrefab, transform.position, Quaternion.identity);
        pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
    }

    private void Update()
    {
        if (wasSpawned != isSpawned)
        {
            if (isSpawned)
            {
                InvokeRepeating(nameof(Spawn), 0f, spawnRate);
            }
            else
            {
                CancelInvoke(nameof(Spawn));
            }

            wasSpawned = isSpawned;
        }
    }
}
