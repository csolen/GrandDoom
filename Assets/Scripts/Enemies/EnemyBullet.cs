using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int bulletDamage;

    public float bulletSpeed = 2.2f;

    private Rigidbody2D rb;

    private Vector3 direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        direction = PlayerController.instance.transform.position - transform.position;
        direction.Normalize();
        direction *= bulletSpeed;
    }

    private void Update()
    {
        rb.linearVelocity = direction * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.instance.TakeDamage(bulletDamage, transform.position);

            Destroy(gameObject);
        }

        if (other.CompareTag("Obstacle_Wall") || other.CompareTag("Obstacle_Door")
            || other.CompareTag("Wall_Ceiling_Light"))
        {
            Destroy(gameObject);
        }
    }

}
