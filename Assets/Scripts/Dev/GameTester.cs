using UnityEngine;

public class GameTester : MonoBehaviour
{
    private static GameTester instance;

    private GameObject TileMap;

    private bool seeCursor;
    private bool firstTimeOpener;

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
        OpenTileMapsWhenGameStarts();

        PauseEditor();

        LockCursor();

        MakePlayerImmortal();
    }

    private void OpenTileMapsWhenGameStarts()
    {
        if (TileMap == null)
        {
            TileMap = GameObject.Find("Tilemap");
            for (int i = 0; i < TileMap.transform.childCount; i++)
            {
                TileMap.transform.GetChild(i).transform.gameObject.SetActive(true);
            }
        }
    }

    private void PauseEditor()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UnityEditor.EditorApplication.isPaused = true;
        }
    }

    private void LockCursor()
    {
        if (!firstTimeOpener)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            firstTimeOpener = true;
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

    private void MakePlayerImmortal()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerController.instance.health = PlayerController.instance.maxHealth;
            PlayerController.instance.ammoAmount = PlayerController.instance.maxAmmoAmount;
            PlayerController.instance.goldAmount = 100;
        }
    }

}
