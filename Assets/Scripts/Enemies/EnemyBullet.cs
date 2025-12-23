using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int bulletDamage;
    public float bulletSpeed = 2.2f;
    public float zChangePerSecond = 0.7f;

    Rigidbody2D rb;
    Vector2 direction;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (PlayerController.instance == null) return;

        Vector3 target = PlayerController.instance.transform.position;
        target.y -= 0.5f;

        direction = ((Vector2)target - (Vector2)transform.position).normalized;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direction * bulletSpeed;

        Vector3 p = transform.position;
        p.z += zChangePerSecond * Time.fixedDeltaTime;
        transform.position = p;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerController.instance != null)
                PlayerController.instance.TakeDamage(bulletDamage, transform.position);

            Destroy(gameObject);
            return;
        }
        else if (other.CompareTag("Obstacle_Wall") || other.CompareTag("Obstacle_Door") || other.CompareTag("Wall_Ceiling_Light"))
        {
            Destroy(gameObject);
            return;
        }
        else if (other.CompareTag("Design_Elements_Holder"))
        {
            Transform t = other.transform.Find("Face_To_Camera/Cube/Design_Element_Sprite_Back");
            if (t != null)
            {
                Design_Elements de = t.GetComponent<Design_Elements>();
                if (de != null)
                    de.DesignElementGotHit();
            }

            Destroy(gameObject);
            return;
        }
    }
}
