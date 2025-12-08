using UnityEngine;
using UnityEngine.UI;

public class LuckyWheels : MonoBehaviour
{
    public float requiredTimeInside = 3f;
    public Slider progressSlider;

    private float timer = 0f;
    private bool playerInside = false;
    private bool triggeredThisStay = false;

    private void Start()
    {
        progressSlider.maxValue = 1f;
        progressSlider.value = 0f;
        progressSlider.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            triggeredThisStay = false;
            timer = 0f;

            progressSlider.value = 1f;
            progressSlider.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ResetProgress();
        }
    }

    private void Update()
    {
        if (playerInside)
        {
            timer += Time.deltaTime;
            float n = Mathf.Clamp01(timer / requiredTimeInside);
            progressSlider.value = 1f - n;

            if (!triggeredThisStay && n >= 1f)
            {
                triggeredThisStay = true;
                PlayerPrefs.SetInt("Open_SpinWheel", 1);
                ResetProgress();
            }
        }
    }

    private void ResetProgress()
    {
        playerInside = false;
        timer = 0f;
        triggeredThisStay = false;
        progressSlider.value = 0f;
        progressSlider.gameObject.SetActive(false);
    }
}
