using UnityEngine;

public class EnemyDestroyingAnimation : MonoBehaviour
{
    public GameObject deathParticle;
    public GameObject deathHolder;

    private CircleCollider2D deathHolderCircleCol;
    private BoxCollider2D deathHolderBoxCol;

    private void Awake()
    {
        deathHolderCircleCol = deathHolder.GetComponent<CircleCollider2D>();
        deathHolderBoxCol = deathHolder.GetComponent<BoxCollider2D>();
    }
    public void OpenDeathParticle()
    {
        deathParticle.SetActive(true);
    }

    public void RemoveAnimationObject()
    {
        Destroy(deathHolder);
    }
    public void DisableComponents()
    {
        if (deathHolderCircleCol != null) 
        {
            deathHolderCircleCol.enabled = false;
        }

        if (deathHolderBoxCol != null)
        {
            deathHolderBoxCol.enabled = false;
        }
        
    }
}
