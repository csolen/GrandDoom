using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    public Button reviveBtn;
    public Button reviveWithAdsBtn;
    public Button restartBtn;

    public void Start()
    {
        restartBtn.onClick.AddListener(RestartCurrentScene);
        reviveBtn.onClick.AddListener(RevivePlayerWithGold);
        reviveBtn.onClick.AddListener(RevivePlayerWithAds);
    }

    private void RevivePlayerWithGold()
    {
        PlayerController.instance.RevivePlayer(1);
    }

    private void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void RevivePlayerWithAds()
    {
        PlayerController.instance.RevivePlayer(2);
    }
}
