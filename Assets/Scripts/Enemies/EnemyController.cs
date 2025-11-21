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

    public SpriteRenderer enemySr;
    private Material currentMaterial;
    public Material takeDamageMaterial;

    public bool shouldShoot;
    public float fireRate = .5f;
    private float shotCounter;
    public GameObject bullet;
    public Transform firePoint;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentMaterial = enemySr.material;
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

            if (shouldShoot)
            {
                shotCounter -= Time.deltaTime;

                if (shotCounter <= 0)
                {
                    Instantiate(bullet, firePoint.position, firePoint.rotation);
                    shotCounter = fireRate;
                }
            }
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

        enemySr.material = takeDamageMaterial;
        Invoke(nameof(ChangeMaterialBack), .05f);

        if (EnemyHealth <= 0)
        {
            Destroy(gameObject);
            Instantiate(deathAnim, transform.position, transform.rotation);
        }
    }

    private void ChangeMaterialBack()
    {
        enemySr.material = currentMaterial;
    }
}
