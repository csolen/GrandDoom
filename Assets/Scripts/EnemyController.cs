using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int EnemyHealth;

    public GameObject deathAnim;

    public float playerRange = 10f;

    private Rigidbody2D rb;

    public float moveSpeed = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < playerRange)
        {
            Vector3 playerDirection = PlayerController.instance.transform.position - transform.position;

            rb.linearVelocity = playerDirection.normalized * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void TakeDamage()
    {
        EnemyHealth--;

        if (EnemyHealth <= 0)
        {
            Destroy(gameObject);
            Instantiate(deathAnim, transform.position, transform.rotation);
        }
    }
}
