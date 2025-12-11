using UnityEngine;
using UnityEngine.UI;

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
    public GameObject[] takeHitPos;
    public GameObject playerCollectItemsScreen;

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
    public float criticalDamageChance = 0.1f;

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

    [Header("Weapon Switch")]
    public float weaponSwitchCooldown = 0.2f;
    public float weaponSwitchLockDurationGun = 0.35f;
    public float weaponSwitchLockDurationSword = 0.35f;
    private float weaponSwitchTimer = 0f;
    private bool weaponSwitchLocked = false;

    private enum HitSide
    {
        Front,
        Back,
        Left,
        Right
    }

    [Header("Hit Feedback (Debug)")]
    [SerializeField] private Vector2 lastHitDirection;
    [SerializeField] private HitSide lastHitSide;

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
        gunDefaultLocalPos = gunObject.transform.localPosition;
        gunDefaultLocalRot = gunObject.transform.localRotation;

        katanaDefaultLocalPos = swordObject.transform.localPosition;
        katanaDefaultLocalRot = swordObject.transform.localRotation;

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

        if (weaponSwitchTimer > 0f)
        {
            weaponSwitchTimer -= Time.deltaTime;
            if (weaponSwitchTimer < 0f)
                weaponSwitchTimer = 0f;
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
        if (weapon == currentWeapon)
            return;

        if (weaponSwitchTimer > 0f || weaponSwitchLocked)
            return;

        weaponSwitchTimer = weaponSwitchCooldown;
        currentWeapon = weapon;

        weaponSwitchLocked = true;
        CancelInvoke(nameof(UnlockWeaponSwitch));

        if (weapon == WeaponType.Gun)
        {
            swordObject.SetActive(false);

            gunObject.SetActive(true);
            gunObject.transform.localPosition = gunDefaultLocalPos;
            gunObject.transform.localRotation = gunDefaultLocalRot;

            Player_Weapon_Gun.Rebind();
            Player_Weapon_Gun.Update(0f);
            Player_Weapon_Gun.Play("Player_Weapon_Gun_Enter", 0, 0f);

            Invoke(nameof(UnlockWeaponSwitch), weaponSwitchLockDurationGun);
        }
        else
        {
            gunObject.SetActive(false);

            swordObject.SetActive(true);
            swordObject.transform.localPosition = katanaDefaultLocalPos;
            swordObject.transform.localRotation = katanaDefaultLocalRot;

            Player_Weapon_Sword.Rebind();
            Player_Weapon_Sword.Update(0f);
            Player_Weapon_Sword.Play("Player_Weapon_Sword_Enter", 0, 0f);

            Invoke(nameof(UnlockWeaponSwitch), weaponSwitchLockDurationSword);
        }
    }

    private void UnlockWeaponSwitch()
    {
        weaponSwitchLocked = false;
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
        if (weaponSwitchLocked)
            return;

        if (ammoAmount <= 0)
            return;

        Player_Weapon_Gun.SetTrigger("isShooting");
        CrosshairRecoil.instance.OnShoot();

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
        if (weaponSwitchLocked)
            return;

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

    public void TakeDamage(int damageAmount, Vector3 damageSourcePosition)
    {
        Vector2 hitDir = GetHitDirection2D(damageSourcePosition);
        lastHitDirection = hitDir;

        HitSide side = GetHitSideFromDirection(hitDir);
        lastHitSide = side;

        if (health - damageAmount > 0)
        {
            health -= damageAmount;
            ShowHitScreenForSide(side);
            Invoke(nameof(CloseHitAnimation), .4f);
        }
        else
        {
            deadScreen.SetActive(true);
            hasDied = true;
            GameTester.Instance.ShouldStopTheGame(true);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        TakeDamage(damageAmount, transform.position);
    }

    private Vector2 GetHitDirection2D(Vector3 damageSourcePosition)
    {
        Vector2 attackerPos2D = new (damageSourcePosition.x, damageSourcePosition.y);
        Vector2 playerPos2D = new (transform.position.x, transform.position.y);
        Vector2 toAttacker = (attackerPos2D - playerPos2D).normalized;
        return toAttacker;
    }

    private HitSide GetHitSideFromDirection(Vector2 toAttacker)
    {
        Vector2 forward = transform.right;
        Vector2 right = new Vector2(forward.y, -forward.x);

        float forwardDot = Vector2.Dot(forward, toAttacker);
        float rightDot = Vector2.Dot(right, toAttacker);

        if (Mathf.Abs(forwardDot) > Mathf.Abs(rightDot))
        {
            if (forwardDot > 0f)
                return HitSide.Front;
            else
                return HitSide.Back;
        }
        else
        {
            if (rightDot > 0f)
                return HitSide.Right;
            else
                return HitSide.Left;
        }
    }

    private void ShowHitScreenForSide(HitSide side)
    {
        if (takeHitPos == null || takeHitPos.Length == 0)
            return;

        for (int i = 0; i < takeHitPos.Length; i++)
        {
            if (takeHitPos[i] != null)
                takeHitPos[i].SetActive(false);
        }

        int index = (int)side;

        if (index >= 0 && index < takeHitPos.Length && takeHitPos[index] != null)
        {
            takeHitPos[index].SetActive(true);
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
        if (takeHitPos == null || takeHitPos.Length == 0)
            return;

        for (int i = 0; i < takeHitPos.Length; i++)
        {
            if (takeHitPos[i] != null)
                takeHitPos[i].SetActive(false);
        }
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

    public void ShowPlayerCollectItemsScreen()
    {
        /*
        playerCollectItemsScreen.GetComponent<Image>().color = new Color(0f,0f,0f);
        playerCollectItemsScreen.SetActive(true);
        */
    }
}
