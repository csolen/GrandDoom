using UnityEngine;

public class EnemyDestroyingAnimation : MonoBehaviour
{

    private void Start()
    {
        Debug.Log(gameObject.transform.position);
    }

    public void RemoveAnimationObject()
    {
        Destroy(gameObject);
    }
}
