using UnityEngine;
using UnityEngine.InputSystem;

public class Spawner : MonoBehaviour
{
    public InputAction spawnAction; // Action để bật/tắt spawning

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
    }

    private void OnDisable()
    {
        spawnAction.performed -= OnSpawnActionPerformed;
        spawnAction.Disable();
    }

    private void OnSpawnActionPerformed(InputAction.CallbackContext context)
    {
        // Toggle trạng thái bật/tắt spawn khi nhấn phím
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
