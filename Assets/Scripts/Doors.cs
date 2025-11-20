using UnityEngine;

public class Doors : MonoBehaviour
{
    public Transform doorPos;
    public GameObject colliderObj;

    public float openSpeed = 1f;

    private bool shouldOpen;

    public void Update()
    {
        if (shouldOpen && doorPos.position.z != 1f)
        {
            doorPos.position = Vector3.MoveTowards(doorPos.position, new Vector3(doorPos.position.x, doorPos.position.y, 1f), openSpeed * Time.deltaTime);

            if (doorPos.position.z >= .2f)
            {
                colliderObj.SetActive(false);
            }
        }
        else if (!shouldOpen && doorPos.position.z != 0f)
        {
            doorPos.position = Vector3.MoveTowards(doorPos.position, new Vector3(doorPos.position.x, doorPos.position.y, 0f), openSpeed * Time.deltaTime);

            if (doorPos.position.z <= .8f)
            {
                colliderObj.SetActive(true);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            shouldOpen = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            shouldOpen = false;
        }
    }
}
