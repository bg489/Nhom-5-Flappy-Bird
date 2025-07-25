using System.Collections;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public Transform shootingPoint; // Vị trí để bắn
    public GameObject bulletPrefab; // Prefab viên đạn
    public GameObject muzzleFlashPrefab; // Hiệu ứng khi bắn (có thể để null nếu không dùng)
    public AudioClip shootSound; // Âm thanh bắn
    public float shootInterval = 2f; // Thời gian chờ giữa mỗi lần bắn
    private Animator animator;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(ShootRepeatedly());
        animator = GetComponent<Animator>();
    }

    private IEnumerator ShootRepeatedly()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(shootInterval);
        }
    }

    private void Shoot()
    {
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        if (muzzleFlashPrefab != null)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, shootingPoint.position, shootingPoint.rotation);
            Destroy(flash, 0.2f); // Xoá flash sau 0.2s
        }

        if (animator != null)
        {
            animator.Play("Enemy Attack 1"); // Tên phải đúng tên state
        }
        Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
    }
}
