using UnityEngine;

public class EnemyBullet1 : MonoBehaviour
{
    public float speed = 20f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Bay qua tr�i, ch�o l�n ho?c xu?ng ng?u nhi�n
        Vector2 randomDirection = new Vector2(-1f, Random.Range(-0.5f, 0.5f)).normalized;

        rb.linearVelocity = randomDirection * speed;

        Destroy(gameObject, 2f);
    }
}
