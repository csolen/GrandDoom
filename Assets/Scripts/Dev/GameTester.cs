using UnityEngine;
using TMPro;

public class GameTester : MonoBehaviour
{
    private bool seeCursor;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerController.instance.health = 100;
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
