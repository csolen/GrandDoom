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

    public int health = 100;
    public int goldAmount = 50;
    public int ammoAmount = 15;

    private void Awake()
    {
        instance = this;

        viewCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
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
                Ray ray = viewCam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log(hit.transform.name);

                    Vector3 bulletImpactOffset = new (-0.1f, 0f, 0f);
                    Instantiate(bulletImpact, hit.point + bulletImpactOffset, transform.rotation);

                }

                ammoAmount--;

            }
        }
    }
}
