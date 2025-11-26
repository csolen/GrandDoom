using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTester : MonoBehaviour
{
    private static GameTester instance;

    private GameObject TileMap;

    private bool seeCursor;
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
        OpenTileMapsWhenGameStarts();

        PauseEditor();

        LockCursor();

        MakePlayerImmortal();

        RestartScene();

        StopAllEnemies();
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

    private void StopAllEnemies()
    {
        if (Input.GetKeyDown(KeyCode.F))
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
