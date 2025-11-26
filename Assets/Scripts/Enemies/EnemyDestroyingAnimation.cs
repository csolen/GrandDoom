using UnityEngine;

public class EnemyDestroyingAnimation : MonoBehaviour
{
    public GameObject deathParticle;

    public void OpenDeathParticle()
    {
        deathParticle.SetActive(true);
    }

    public void RemoveAnimationObject()
    {
        Destroy(gameObject);
    }
}
