using UnityEngine;

public class Collectables : MonoBehaviour
{
    public enum CollectableType
    {
        Health,
        Gold,
        Ammo
    }

    public CollectableType collectableType = CollectableType.Ammo;

    public int amount = 25;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            switch (collectableType)
            {
                case CollectableType.Health:

                    if (PlayerController.instance.health >= PlayerController.instance.maxHealth)
                    {
                        return;
                    }

                    PlayerController.instance.health =
                        Mathf.Min(PlayerController.instance.health + amount,
                                  PlayerController.instance.maxHealth);
                    break;

                case CollectableType.Gold:
                    PlayerController.instance.goldAmount += amount;
                    break;

                case CollectableType.Ammo:

                    if (PlayerController.instance.ammoAmount >= PlayerController.instance.maxAmmoAmount)
                    {
                        return;
                    }

                    PlayerController.instance.ammoAmount =
                        Mathf.Min(PlayerController.instance.ammoAmount + amount,
                                  PlayerController.instance.maxAmmoAmount);

                    break;
            }

            Destroy(gameObject);
        }
    }
}
