using UnityEngine;

public class EnemyDamageEvent : MonoBehaviour
{
    public int enemyDamage = 5;

    public void DealMeleeDamage()
    {
        PlayerController.instance.TakeDamage(enemyDamage);
    }
}
