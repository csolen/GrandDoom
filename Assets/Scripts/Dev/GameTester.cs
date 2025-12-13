using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTester : MonoBehaviour
{
    private static GameTester instance;
    public static GameTester Instance => instance;

    private GameObject TileMap;
    private bool firstTimeOpener;
    private bool enemiesStopped = false;

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
        if (SceneManager.GetActiveScene().name == "Level01")
        {
            FirstTimeOpener();
            OpenTileMapsWhenGameStarts();
        }

        RestartScene();

        StopAllEnemies();

        PauseTheGame();

        ReloadAmmo();

        GivePlayerHealth();

        OpenPanels();

    }

    private void OpenTileMapsWhenGameStarts()
    {
        if (TileMap == null)
        {
            TileMap = GameObject.Find("Tilemap");

            if (TileMap != null)
            {
                for (int i = 0; i < TileMap.transform.childCount; i++)
                {
                    TileMap.transform.GetChild(i).transform.gameObject.SetActive(true);
                }
            }
        }
    }

    private void FirstTimeOpener()
    {
        if (!firstTimeOpener)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            firstTimeOpener = true;
        }
    }


    private void PauseTheGame()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            UnityEditor.EditorApplication.isPaused = true;
        }
    }

    private void StopAllEnemies()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            enemiesStopped = !enemiesStopped;

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy_Holder");

            for (int i = 0; i < enemies.Length; i++)
            {
                GameObject enemy = enemies[i];

                Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
                EnemyController enemyController = enemy.GetComponent<EnemyController>();

                if (rb != null)
                {
                    if (enemiesStopped)
                    {
                        rb.freezeRotation = true;
                        rb.constraints = RigidbodyConstraints2D.FreezePositionX |
                                         RigidbodyConstraints2D.FreezePositionY;
                    }
                    else
                    {
                        rb.freezeRotation = false;
                        rb.constraints = RigidbodyConstraints2D.None;
                    }
                }

                if (enemyController != null)
                {
                    enemyController.shouldShoot = !enemiesStopped;
                }
            }
        }
    }

    private void RestartScene()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void ReloadAmmo()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            PlayerController.instance.ammoAmount = PlayerController.instance.maxAmmoAmount;
        }
    }

    private void GivePlayerHealth()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            PlayerController.instance.health = PlayerController.instance.maxHealth;
            PlayerController.instance.goldAmount = 100;
        }
    }

    private void OpenPanels()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            PlayerPrefs.SetInt("Open_Roguelike", 1);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayerPrefs.SetInt("Open_SpinWheel", 1);
        }

    }

    public void ShouldStopTheGame(bool state)
    {
        if (state)
        {
            PlayerPrefs.SetInt("ShouldStopTheGame", 1);
            PlayerController.instance.FreezePlayer();

#if UNITY_EDITOR

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
#endif
        }
        else
        {
            PlayerPrefs.SetInt("ShouldStopTheGame", 0);
            PlayerController.instance.UnFreezePlayer();

#if UNITY_EDITOR

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
#endif
        }
    }
}
