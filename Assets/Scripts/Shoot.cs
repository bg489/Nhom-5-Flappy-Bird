using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public Transform shootingPoint;
    public GameObject beginPrefab;
    public GameObject bulletPrefab;
    public InputAction shootAction;
    [SerializeField] private AudioClip shootSound;
    private AudioSource audioSource;
    public GameObject gun;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shootAction.Enable();
        audioSource = GetComponent<AudioSource>(); // Gán component AudioSource
    }

    // Update is called once per frame
    void Update()
    {
        if(shootAction.triggered && (Time.timeScale != 0))
        {
            gun.SetActive(true); // Activate the gun GameObject
            audioSource.PlayOneShot(shootSound); // Play the shooting sound effect
            Instantiate(beginPrefab, shootingPoint.position, transform.rotation);
            Instantiate(bulletPrefab, shootingPoint.position, transform.rotation);

            StartCoroutine(DisableGunAfterDelay(0.3f));
        }
    }

    private IEnumerator DisableGunAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gun.SetActive(false);
    }

}
