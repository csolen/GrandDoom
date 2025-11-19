using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    public float lifeTime;

    void Update()
    {
        Destroy(gameObject, lifeTime);
    }
}
