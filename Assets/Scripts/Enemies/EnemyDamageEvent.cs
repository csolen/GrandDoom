using UnityEngine;

public class EnemyDamageEvent : MonoBehaviour
{
    private int enemyDamage;
    EnemyController enemyController;

    private void Awake()
    {
        enemyController = GetComponentInParent<EnemyController>();
        enemyDamage = enemyController.enemyDamage;
    }

    public void DealMeleeDamage()
    {
        PlayerController.instance.TakeDamage(enemyDamage);
    }
}
