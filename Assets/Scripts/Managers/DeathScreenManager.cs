using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    public Button reviveBtn;
    public Button restartBtn;

    public void Start()
    {
        reviveBtn.onClick.AddListener(RevivePlayerWithGold);
        restartBtn.onClick.AddListener(RestartCurrentScene);
    }

    private void RevivePlayerWithGold()
    {
        PlayerController.instance.RevivePlayer();
    }

    private void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
