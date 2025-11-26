using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum EnemyType { Melee, Ranged }
    private enum EnemyState { Wandering, Chasing }

    [Header("General")]
    public EnemyType enemyType = EnemyType.Melee;
    public int EnemyHealth = 3;
    public GameObject deathAnim;

    [Header("References")]
    private Transform player;
    private Rigidbody2D rb;
    public Animator anim;

    [Header("Movement")]
    public float wanderSpeed = 1.5f;
    public float chaseSpeed = 2.3f;
    public float sightRange = 7f;
    public float loseSightRange = 10f;
    public float meleeStopDistance = 0.6f;
    public float rangedStopDistance = 4f;
    public float turnSpeed = 8f;

    [Header("Wander")]
    public float wanderChangeTime = 2f;
    private float wanderTimer;
    private Vector2 wanderDirection;

    [Header("Shooting (Ranged)")]
    public bool shouldShoot = false;
    public GameObject bullet;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public float shootRange = 8f;
    private float shotCounter;

    [Header("Stuck Fix")]
    public float stuckPositionThreshold = 0.02f;
    public float stuckCheckDelay = 0.4f;
    public float unstuckDuration = 0.5f;

    private EnemyState currentState = EnemyState.Wandering;
    private Vector2 moveDirection;
    private Vector2 lastPosition;
    private float stuckTimer;
    private float unstuckTimer;
    private Vector2 unstuckDirection;
    private bool isChasing;
    private bool isAttacking;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.rotation = Quaternion.Euler(0f, 90f, -90f);
    }

    private void Start()
    {
        if (player == null)
        {
            if (PlayerController.instance != null)
            {
                player = PlayerController.instance.transform;
            }
            else
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null)
                    player = playerObj.transform;
            }
        }

        PickNewWanderDirection();
        shotCounter = fireRate;
        lastPosition = rb.position;
    }

    private void Update()
    {
        if (player == null)
        {
            if (PlayerController.instance != null)
                player = PlayerController.instance.transform;
            else
                return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Wandering:
                HandleWandering(distanceToPlayer);
                break;

            case EnemyState.Chasing:
                HandleChasing(distanceToPlayer);
                break;
        }

        UpdateStuckLogic(distanceToPlayer);

        if (moveDirection.sqrMagnitude > 0.001f)
        {
            moveDirection = Vector2.Lerp(
                moveDirection,
                moveDirection.normalized,
                turnSpeed * Time.deltaTime
            );
        }
    }

    private void FixedUpdate()
    {
        float speed = (currentState == EnemyState.Chasing) ? chaseSpeed : wanderSpeed;

        if (moveDirection.sqrMagnitude < 0.01f)
        {
            rb.linearVelocity = Vector2.zero;

            if (!isAttacking)
            {
                anim.SetTrigger("shouldIdle");
            }
        }
        else
        {
            rb.linearVelocity = moveDirection.normalized * speed;

            if (!isChasing)
            {
                anim.SetTrigger("shouldWalk");
            }
            else
            {
                anim.SetTrigger("shouldChase");
            }
        }
    }

    private void HandleWandering(float distanceToPlayer)
    {
        if (distanceToPlayer <= sightRange)
        {
            currentState = EnemyState.Chasing;
            return;
        }

        wanderTimer -= Time.deltaTime;
        if (wanderTimer <= 0f)
        {
            PickNewWanderDirection();
        }

        moveDirection = wanderDirection;
    }

    private void HandleChasing(float distanceToPlayer)
    {
        if (distanceToPlayer > loseSightRange)
        {
            currentState = EnemyState.Wandering;
            PickNewWanderDirection();
            isChasing = false;
            isAttacking = false;
            return;
        }

        Vector2 dirToPlayer = (player.position - transform.position).normalized;

        if (enemyType == EnemyType.Melee)
        {
            if (distanceToPlayer > meleeStopDistance)
            {
                isChasing = true;
                isAttacking = false;
                moveDirection = dirToPlayer;
            }
            else
            {
                moveDirection = Vector2.zero;

                if (!isAttacking)
                {
                    isAttacking = true;
                    anim.ResetTrigger("shouldIdle");
                    anim.ResetTrigger("shouldWalk");
                    anim.ResetTrigger("shouldChase");
                    anim.SetTrigger("shouldAttack");
                }
            }
        }
        else
        {
            float targetDist = rangedStopDistance;
            float distDiff = distanceToPlayer - targetDist;

            if (Mathf.Abs(distDiff) > 0.3f)
            {
                moveDirection = distDiff > 0 ? dirToPlayer : -dirToPlayer;
            }
            else
            {
                moveDirection = Vector2.zero;
            }

            if (shouldShoot)
            {
                HandleShooting(distanceToPlayer, dirToPlayer);
            }
        }
    }

    private void HandleShooting(float distanceToPlayer, Vector2 dirToPlayer)
    {
        if (bullet == null || firePoint == null)
            return;

        if (distanceToPlayer > shootRange)
            return;

        shotCounter -= Time.deltaTime;

        if (shotCounter <= 0f)
        {
            float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            anim.SetTrigger("shouldAttack");
            Instantiate(bullet, firePoint.position, firePoint.rotation);
            shotCounter = fireRate;
        }
    }

    private void PickNewWanderDirection()
    {
        wanderTimer = wanderChangeTime;
        wanderDirection = Random.insideUnitCircle.normalized;
        isChasing = false;
        isAttacking = false;
    }

    private void UpdateStuckLogic(float distanceToPlayer)
    {
        Vector2 currentPos = rb.position;
        float moved = Vector2.Distance(currentPos, lastPosition);
        lastPosition = currentPos;

        float stopDistance = enemyType == EnemyType.Melee ? meleeStopDistance : rangedStopDistance;
        bool shouldBeMovingToPlayer =
            currentState == EnemyState.Chasing &&
            distanceToPlayer > stopDistance + 0.1f;

        if (unstuckTimer > 0f)
        {
            unstuckTimer -= Time.deltaTime;
            moveDirection = unstuckDirection;

            if (unstuckTimer <= 0f)
            {
                unstuckDirection = Vector2.zero;
            }

            return;
        }

        if (shouldBeMovingToPlayer && moved < stuckPositionThreshold)
        {
            stuckTimer += Time.deltaTime;

            if (stuckTimer >= stuckCheckDelay)
            {
                stuckTimer = 0f;
                unstuckDirection = Random.insideUnitCircle.normalized;
                if (unstuckDirection.sqrMagnitude < 0.01f)
                    unstuckDirection = Vector2.right;
                unstuckTimer = unstuckDuration;
            }
        }
        else
        {
            stuckTimer = 0f;
        }
    }

    public void TakeDamage()
    {
        EnemyHealth--;

        if (EnemyHealth <= 0)
        {
            if (deathAnim != null)
            {
                Instantiate(deathAnim, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }
    }

}