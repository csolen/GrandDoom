using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    public float lifeTime;

    private void Update()
    {
        Destroy(gameObject, lifeTime);
    }
}
