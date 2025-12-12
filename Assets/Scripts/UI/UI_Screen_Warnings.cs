using UnityEngine;

public class UI_Screen_Warnings : MonoBehaviour
{
    [Header("Low Health Panel")]
    public GameObject lowHealthPanel;

    [Range(0f, 1f)] public float lowHealthThreshold = 0.5f;
    [Range(0f, 1f)] public float criticalHealthThreshold = 0.3f;

    private bool isLowHealthShown = false;
    private Animator healthPanelAnim;

    private void Awake()
    {
        lowHealthPanel.SetActive(false);
        healthPanelAnim = lowHealthPanel.GetComponent<Animator>();
    }

    private void Update()
    {
        CheckLowHealthPanel();
    }

    private void CheckLowHealthPanel()
    {
        float healthPercent =
            (float)PlayerController.instance.health /
            PlayerController.instance.maxHealth;

        if (healthPercent <= lowHealthThreshold && !isLowHealthShown)
        {
            lowHealthPanel.SetActive(true);
            isLowHealthShown = true;
        }
        else if (healthPercent > lowHealthThreshold && isLowHealthShown)
        {
            lowHealthPanel.SetActive(false);
            isLowHealthShown = false;
        }

        if (isLowHealthShown)
        {
            bool isCritical = healthPercent <= criticalHealthThreshold;
            healthPanelAnim.SetBool("isSlow", !isCritical);
        }
    }
}
