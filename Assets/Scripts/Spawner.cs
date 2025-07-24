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

    [SerializeField] private AudioClip enable;
    [SerializeField] private AudioClip disable;

    private Image buttonImage;

    private AudioSource audioSource; // Reference to the AudioSource component

    public GameObject prefab;
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
    }

    private void OnDisable()
    {
        spawnAction.performed -= OnSpawnActionPerformed;
        spawnAction.Disable();
    }

    private IEnumerator TemporarilyDisablenablePipes(float duration)
    {
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


    private void OnSpawnActionPerformed(InputAction.CallbackContext context)
    {
        // Toggle trạng thái bật/tắt spawn khi nhấn phím
        if (isSpawned)
        {
            StartCoroutine(TemporarilyDisablenablePipes(2f)); // bật trong 2 giây
        } else
        {
            StartCoroutine(TemporarilyEnablePipes(2f)); // bật trong 2 giây
        }

        isSpawned = !isSpawned;
    }

    private void Spawn()
    {
        GameObject pipes = Instantiate(prefab, transform.position, Quaternion.identity);
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
