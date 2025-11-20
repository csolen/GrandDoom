using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int EnemyHealth;

    public GameObject deathAnim;

    public float playerRange = 10f;
    public float moveSpeed = 1f;

    private Rigidbody2D rb;

    public float wanderRadius = 10f;
    private readonly float wanderChangeTime = 2f;

    private Vector2 wanderTarget;
    private float wanderTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        PickNewWanderTarget();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerController.instance.transform.position);

        if (distanceToPlayer < playerRange)
        {
            Vector3 dir = PlayerController.instance.transform.position - transform.position;
            rb.linearVelocity = dir.normalized * moveSpeed;
        }
        else
        {
            wanderTimer -= Time.deltaTime;

            if (wanderTimer <= 0f || Vector2.Distance(transform.position, wanderTarget) < 0.5f)
            {
                PickNewWanderTarget();
            }

            Vector3 dir = wanderTarget - (Vector2)transform.position;
            rb.linearVelocity = 1.2f * moveSpeed * dir.normalized;
        }
    }

    private void PickNewWanderTarget()
    {
        wanderTimer = wanderChangeTime;

        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        wanderTarget = (Vector2)transform.position + randomCircle;
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
