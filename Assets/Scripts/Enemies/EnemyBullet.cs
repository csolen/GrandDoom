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

        if (other.CompareTag("Design_Elements_Holder"))
        {
            Transform t = other.transform.Find("Face_To_Camera/Cube/Design_Element_Sprite_Back");
            if (t != null)
            {
                Design_Elements de = t.GetComponent<Design_Elements>();
                if (de != null)
                {
                    de.DesignElementGotHit();
                }
            }

            Destroy(gameObject);
        }
    }

}
