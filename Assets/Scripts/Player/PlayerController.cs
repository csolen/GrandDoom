using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private Rigidbody2D rb;

    public float moveSpeed = 5f;

    private Vector2 moveInput;
    private Vector2 mouseInput;

    public float mouseSensitivity = 1f;

    private Camera viewCam;

    public GameObject bulletImpact;

    public int maxHealth = 100;
    public int health = 100;
    public int goldAmount = 50;
    public int ammoAmount = 15;
    public int maxAmmoAmount = 60;

    public Animator gunAnim;
    private Animator anim;

    public GameObject deadScreen;
    public GameObject playerTakeHitScreen;

    private bool hasDied;

    private void Awake()
    {
        instance = this;

        viewCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        if (hasDied)
        {
            return;
        }

        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 moveHorizontal = transform.up * -moveInput.x;

        Vector3 moveVertical = transform.right * moveInput.y;

        rb.linearVelocity = (moveHorizontal + moveVertical) * moveSpeed;

        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - mouseInput.x);

        viewCam.transform.localRotation = Quaternion.Euler(viewCam.transform.localRotation.eulerAngles + new Vector3(0f, mouseInput.y, 0f));

        if (Input.GetMouseButtonDown(0))
        {
            if (ammoAmount > 0)
            {
                if (CrosshairRecoil.instance != null)
                {
                    gunAnim.SetTrigger("isShooting");
                    CrosshairRecoil.instance.OnShoot();
                }

                Ray ray = viewCam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 bulletImpactOffset = new(-0.1f, 0f, 0f);
                    Instantiate(bulletImpact, hit.point + bulletImpactOffset, transform.rotation);

                    if (hit.transform.CompareTag("Enemy"))
                    {
                        hit.transform.parent.GetComponent<EnemyController>().TakeDamage();
                    }

                    if (hit.transform.CompareTag("Wall_Ceiling_Light"))
                    {
                        hit.transform.GetComponent<Design_TileChanger>().ChangeSprite();
                    }

                }

                ammoAmount--;
            }
        }

        if (moveInput != Vector2.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (health - damageAmount > 0)
        {
            health -= damageAmount;
            playerTakeHitScreen.SetActive(true);
            Invoke(nameof(CloseHitAnimation) , .4f);
        }
        else
        {
            deadScreen.SetActive(true);
            hasDied = true;
        }
    }

    public void AddHealth(int healAmount)
    {
        health += healAmount;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    private void CloseHitAnimation()
    {
        playerTakeHitScreen.SetActive(false);
    }
}
