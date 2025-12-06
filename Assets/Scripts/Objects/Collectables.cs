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
                    return;
                PlayerController.instance.health =
                    Mathf.Min(PlayerController.instance.health + amount,
                              PlayerController.instance.maxHealth);
                break;

            case CollectableType.Gold:
                PlayerController.instance.goldAmount += amount;
                break;

            case CollectableType.Ammo:
                if (PlayerController.instance.ammoAmount >= PlayerController.instance.maxAmmoAmount)
                    return;
                PlayerController.instance.ammoAmount =
                    Mathf.Min(PlayerController.instance.ammoAmount + amount,
                              PlayerController.instance.maxAmmoAmount);
                break;

            case CollectableType.Xp:
                int xp = PlayerPrefs.GetInt("Roguelike_Xp");
                xp += amount;
                PlayerPrefs.SetInt("Roguelike_Xp", xp);
                break;
        }

        Destroy(gameObject);
    }
}
