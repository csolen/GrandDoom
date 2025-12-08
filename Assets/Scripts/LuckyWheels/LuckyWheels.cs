using UnityEngine;

public class LuckyWheels : MonoBehaviour
{
    public float requiredTimeInside = 3f;
    private float timer = 0f;
    private bool playerInside = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            timer = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            timer = 0f;
        }
    }

    private void Update()
    {
        if (playerInside)
        {
            timer += Time.deltaTime;

            if (timer >= requiredTimeInside)
            {
                PlayerPrefs.SetInt("Open_SpinWheel", 1);
                playerInside = false;
            }
        }
    }
}
