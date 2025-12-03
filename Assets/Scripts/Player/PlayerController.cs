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
    public GameObject winScreen;
    public GameObject playerTakeHitScreen;

    private bool hasDied;

    public float climbSpeed = 2f;
    private bool isOnLadder = false;
    private Ladder currentLadder;
    private float ladderTargetZ;
    private float ladderStartZ;

    public GameObject muzzleFlash;
    public Transform muzzleFlashPoint;

    private float camY;
    public float camLimiterYMin = 40f;
    public float camLimiterYMax = 120f;

    private void Awake()
    {
        ShowCursorInEditor(false);
        instance = this;

        viewCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        camY = viewCam.transform.localEulerAngles.y;
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

        if (PlayerPrefs.GetInt("ShouldStopTheGame") == 1)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 moveHorizontal = transform.up * -moveInput.x;
        Vector3 moveVertical = transform.right * moveInput.y;

        rb.linearVelocity = (moveHorizontal + moveVertical) * moveSpeed;

        if (isOnLadder && currentLadder != null && moveInput.y > 0.1f)
        {
            Vector3 pos = transform.position;
            pos.z = Mathf.MoveTowards(pos.z, ladderTargetZ, climbSpeed * Time.deltaTime);
            transform.position = pos;

            if (Mathf.Approximately(pos.z, ladderTargetZ))
            {
                isOnLadder = false;
                currentLadder = null;
            }
        }

        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y,
            transform.rotation.eulerAngles.z - mouseInput.x
        );

        camY += mouseInput.y;
        camY = Mathf.Clamp(camY, camLimiterYMin, camLimiterYMax);

        Vector3 camRot = viewCam.transform.localEulerAngles;
        camRot.y = camY;
        viewCam.transform.localEulerAngles = camRot;

        if (Input.GetMouseButtonDown(0))
        {
            if (ammoAmount > 0)
            {
                if (CrosshairRecoil.instance != null)
                {
                    gunAnim.SetTrigger("isShooting");
                    CrosshairRecoil.instance.OnShoot();
                }

                Instantiate(muzzleFlash, muzzleFlashPoint.position, muzzleFlashPoint.rotation);

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Level_Pass_Trigger"))
        {
            LevelPassed();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (health - damageAmount > 0)
        {
            health -= damageAmount;
            playerTakeHitScreen.SetActive(true);
            Invoke(nameof(CloseHitAnimation), .4f);
        }
        else
        {
            deadScreen.SetActive(true);
            hasDied = true;

            ShowCursorInEditor(true);
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

    public void EnterLadder(Ladder ladder, float targetZ)
    {
        currentLadder = ladder;
        ladderTargetZ = targetZ;

        ladderStartZ = transform.position.z;

        isOnLadder = true;
    }

    public void ExitLadder(Ladder ladder)
    {
        if (currentLadder == ladder)
        {
            float currentZ = transform.position.z;

            float distToStart = Mathf.Abs(currentZ - ladderStartZ);
            float distToTarget = Mathf.Abs(currentZ - ladderTargetZ);

            float finalZ = distToStart < distToTarget ? ladderStartZ : ladderTargetZ;

            Vector3 pos = transform.position;
            pos.z = finalZ;
            transform.position = pos;

            isOnLadder = false;
            currentLadder = null;
        }
    }

    private void LevelPassed()
    {
        winScreen.SetActive(true);
        hasDied = true;

        ShowCursorInEditor(true);
    }

    private void ShowCursorInEditor(bool state)
    {
#if UNITY_EDITOR
        if (state)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
#endif
    }
}
