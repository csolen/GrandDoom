using UnityEngine;

public class Collectables : MonoBehaviour
{
    public enum CollectableType
    {
        Health,
        Gold,
        Ammo,
        Xp
    }

    public CollectableType collectableType = CollectableType.Ammo;

    public int amount = 25;

    public float attractionRange = 2.5f;
    public float moveSpeed = 6f;

    public GameObject[] healVfx;
    public GameObject[] ammoVfx;
    public GameObject[] goldVfx;
    public GameObject[] xpVfx;

    private Transform target;

    private void Start()
    {
        if (PlayerController.instance != null)
        {
            target = PlayerController.instance.transform;
        }
        else
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                target = p.transform;
        }
    }

    private void Update()
    {
        if (collectableType != CollectableType.Xp)
            return;

        if (target == null)
            return;

        float dist = Vector2.Distance(transform.position, target.position);

        if (dist <= attractionRange)
        {
            Vector2 dir = (target.position - transform.position).normalized;
            transform.position += (Vector3)(dir * moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player"))
            return;
        
        switch (collectableType)
        {
            case CollectableType.Health:

                if (PlayerController.instance.health >= PlayerController.instance.maxHealth)
                {
                    return;
                }
                else
                {
                    Instantiate(healVfx[0], new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
                    Instantiate(healVfx[1], new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z - .5f), transform.rotation);
                    PlayerController.instance.health = Mathf.Min(PlayerController.instance.health + amount, PlayerController.instance.maxHealth);
                }
                break;

            case CollectableType.Gold:
                PlayerController.instance.goldAmount += amount;
                break;

            case CollectableType.Ammo:

                if (PlayerController.instance.ammoAmount >= PlayerController.instance.maxAmmoAmount)
                {
                    return;
                }
                else
                {
                    Instantiate(ammoVfx[0], new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
                    Instantiate(ammoVfx[1], new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z), Quaternion.Euler(0, 180f, 0f));
                    PlayerController.instance.ammoAmount = Mathf.Min(PlayerController.instance.ammoAmount + amount, PlayerController.instance.maxAmmoAmount);
                }
                break;

            case CollectableType.Xp:
                PlayerController.instance.ShowPlayerCollectItemsScreen();
                int xp = PlayerPrefs.GetInt("Roguelike_Xp");
                xp += amount;
                PlayerPrefs.SetInt("Roguelike_Xp", xp);
                break;
        }

        Destroy(gameObject);
    }
}
