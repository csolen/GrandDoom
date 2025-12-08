using UnityEngine;

public enum WeaponType
{
    Gun,
    Katana
}

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private Rigidbody2D rb;
    private Camera viewCam;
    private Animator anim;

    [Header("Movement")]
    public float moveSpeed = 5f;

    private Vector2 moveInput;
    private Vector2 mouseInput;

    public float mouseSensitivity = 1f;

    [Header("Health & Stats")]
    public int maxHealth = 100;
    public int health = 100;
    public int goldAmount = 50;
    public int ammoAmount = 15;
    public int maxAmmoAmount = 60;
    public int playerDamage = 40;

    [Header("Weapon Animators")]
    public Animator Player_Weapon_Sword;
    public Animator Player_Weapon_Gun;

    [Header("UI Screens")]
    public GameObject deadScreen;
    public GameObject winScreen;
    public GameObject playerTakeHitScreen;

    private bool hasDied;

    [Header("Ladder")]
    public float climbSpeed = 2f;
    private bool isOnLadder = false;
    private Ladder currentLadder;
    private float ladderTargetZ;
    private float ladderStartZ;

    [Header("Gun FX")]
    public GameObject muzzleFlash;
    public Transform muzzleFlashPoint;
    public GameObject bulletImpact;

    [Header("Camera")]
    private float camY;
    public float camLimiterYMin = 40f;
    public float camLimiterYMax = 120f;

    [Header("Lifesteal")]
    public int lifeStealAmount = 10;
    public float lifeStealChance = 0.1f;

    [Header("Weapons")]
    public WeaponType currentWeapon = WeaponType.Katana;

    [Header("Weapon Objects")]
    public GameObject gunObject;
    public GameObject swordObject;

    private Vector3 gunDefaultLocalPos;
    private Quaternion gunDefaultLocalRot;
    private Vector3 katanaDefaultLocalPos;
    private Quaternion katanaDefaultLocalRot;

    [Header("Katana Settings")]
    public int katanaDamage = 60;
    public float katanaRange = 2f;
    public float katanaAttackCooldown = 0.4f;
    private float katanaAttackTimer = 0f;

    private void Awake()
    {
        instance = this;

        viewCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        camY = viewCam.transform.localEulerAngles.y;
    }

    private void Start()
    {
        if (gunObject != null)
        {
            gunDefaultLocalPos = gunObject.transform.localPosition;
            gunDefaultLocalRot = gunObject.transform.localRotation;
        }

        if (swordObject != null)
        {
            katanaDefaultLocalPos = swordObject.transform.localPosition;
            katanaDefaultLocalRot = swordObject.transform.localRotation;
        }

        SetWeapon(WeaponType.Katana);
        GameTester.Instance.ShouldStopTheGame(false);
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

        katanaAttackTimer -= Time.deltaTime;

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

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetWeapon(WeaponType.Katana);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetWeapon(WeaponType.Gun);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (currentWeapon == WeaponType.Gun)
            {
                HandleGunShoot();
            }
            else if (currentWeapon == WeaponType.Katana)
            {
                HandleKatanaAttack();
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

    public void SetWeapon(WeaponType weapon)
    {
        currentWeapon = weapon;

        if (weapon == WeaponType.Gun)
        {
            if (swordObject != null)
                swordObject.SetActive(false);

            if (gunObject != null)
            {
                gunObject.SetActive(true);
                gunObject.transform.localPosition = gunDefaultLocalPos;
                gunObject.transform.localRotation = gunDefaultLocalRot;

                if (Player_Weapon_Gun != null)
                {
                    Player_Weapon_Gun.Play("Player_Weapon_Gun_Idle", 0, 0f);
                }
            }
        }
        else
        {
            if (gunObject != null)
                gunObject.SetActive(false);

            if (swordObject != null)
            {
                swordObject.SetActive(true);
                swordObject.transform.localPosition = katanaDefaultLocalPos;
                swordObject.transform.localRotation = katanaDefaultLocalRot;

                if (Player_Weapon_Sword != null)
                {
                    Player_Weapon_Sword.Play("Player_Weapon_Sword_Idle", 0, 0f);
                }
            }
        }
    }

    public void SelectGun()
    {
        SetWeapon(WeaponType.Gun);
    }

    public void SelectKatana()
    {
        SetWeapon(WeaponType.Katana);
    }

    private void HandleGunShoot()
    {
        if (ammoAmount <= 0)
        {
            return;
        }

        if (CrosshairRecoil.instance != null)
        {
            Player_Weapon_Gun.SetTrigger("isShooting");
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

        if (ammoAmount <= 0)
        {
            SetWeapon(WeaponType.Katana);
        }
    }

    private void HandleKatanaAttack()
    {
        if (katanaAttackTimer > 0f)
            return;

        katanaAttackTimer = katanaAttackCooldown;

        Player_Weapon_Sword.SetTrigger("isShooting");

        Ray ray = viewCam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, katanaRange))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.parent.GetComponent<EnemyController>().TakeDamage();
            }
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
            GameTester.Instance.ShouldStopTheGame(true);
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

    public void AddAmmo(int aAmount)
    {
        ammoAmount += aAmount;

        if (ammoAmount > maxAmmoAmount)
        {
            ammoAmount = maxAmmoAmount;
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
        GameTester.Instance.ShouldStopTheGame(true);
    }

    public int IncreaseByPercent(int value, int percent)
    {
        float result = value * (1f + percent / 100f);
        return Mathf.RoundToInt(result);
    }

    public float IncreaseByPercent(float value, float percent)
    {
        return value * (1f + percent / 100f);
    }

    public void FreezePlayer()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX |
                         RigidbodyConstraints2D.FreezePositionY;
    }

    public void UnFreezePlayer()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.freezeRotation = true;
    }
}
