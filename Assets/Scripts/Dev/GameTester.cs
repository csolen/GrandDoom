using UnityEngine;
using TMPro;

public class GameTester : MonoBehaviour
{
    private static GameTester instance;
    private bool seeCursor;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerController.instance.health = PlayerController.instance.maxHealth;
            PlayerController.instance.ammoAmount = 100;
            PlayerController.instance.goldAmount = 100;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!seeCursor)
            {
                seeCursor = true;
            }
            else
            {
                seeCursor = false;
            }
        }

        if (!seeCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }


    }
}
