using UnityEngine;

public class ObjectsFaceToCamera : MonoBehaviour
{
    private SpriteRenderer sr;

    private void Awake()
    {
        if (sr != null)
        {
            sr = GetComponent<SpriteRenderer>();
            sr.flipX = true;
        }

    }

    private void Update()
    {
        transform.LookAt(PlayerController.instance.transform.position, -Vector3.forward);
    }
}
