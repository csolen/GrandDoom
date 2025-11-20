using UnityEngine;
using TMPro;

public class GameTester : MonoBehaviour
{
    public GameObject UI_Holder;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI goldText;

    public TextMeshProUGUI explainerText;

    public void Start()
    {
        explainerText.text = "Press TAB for UI";

        UI_Holder.SetActive(true);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (UI_Holder.activeInHierarchy)
            {
                UI_Holder.SetActive(false);
            }
            else
            {
                UI_Holder.SetActive(true);
            }
        }

        if (UI_Holder.activeInHierarchy)
        {
            explainerText.gameObject.SetActive(false);

            healthText.text = "Health: " + PlayerController.instance.health.ToString();
            ammoText.text = "Ammo: " + PlayerController.instance.ammoAmount.ToString();
            goldText.text = "Gold: " + PlayerController.instance.goldAmount.ToString();
        }
        else
        {
            explainerText.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerController.instance.health = 100;
            PlayerController.instance.ammoAmount = 100;
            PlayerController.instance.goldAmount = 100;
        }

    }
}
