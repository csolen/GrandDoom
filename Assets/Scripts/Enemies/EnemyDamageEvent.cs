using UnityEngine;

public class EnemyDamageEvent : MonoBehaviour
{
    public int enemyDamage = 5;

    private void enemy;

    public void Awake()
    {
        
    }

    public void DealMeleeDamage()
    {
        PlayerController.instance.TakeDamage(enemyDamage);
    }
}
