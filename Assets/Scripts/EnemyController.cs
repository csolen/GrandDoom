using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int EnemyHealth;

    public GameObject deathAnim;

    public float playerRange = 5f;
    public float moveSpeed = 1.7f;

    private Rigidbody2D rb;

    public float wanderRadius = 10f;
    public float wanderChangeTime = 1.5f;

    private Vector2 wanderTarget;
    private float wanderTimer;

    public float turnSpeed = 5f;
    private Vector2 moveDirection;
    private Vector2 targetDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        PickNewWanderTarget();
        moveDirection = Random.insideUnitCircle.normalized;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerController.instance.transform.position);

        if (distanceToPlayer < playerRange)
        {
            Vector3 dir = PlayerController.instance.transform.position - transform.position;
            targetDirection = dir.normalized;
        }
        else
        {
            wanderTimer -= Time.deltaTime;

            if (wanderTimer <= 0f || Vector2.Distance(transform.position, wanderTarget) < 0.5f)
            {
                PickNewWanderTarget();
            }

            Vector3 dir = wanderTarget - (Vector2)transform.position;
            targetDirection = dir.normalized;
        }

        moveDirection = Vector2.Lerp(
            moveDirection,
            targetDirection,
            turnSpeed * Time.deltaTime
        );
    }

    private void FixedUpdate()
    {
        if (targetDirection.sqrMagnitude < 0.001f)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.linearVelocity = moveDirection * moveSpeed;
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
