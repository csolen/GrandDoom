using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    public Button reviveBtn;
    public Button reviveWithAdsBtn;
    public Button restartBtn;

    public GameObject RewardedAdsHolder;

    private bool hideRewardedAds;

    public void Start()
    {
        restartBtn.onClick.AddListener(RestartCurrentScene);
        reviveBtn.onClick.AddListener(RevivePlayerWithGold);
        reviveWithAdsBtn.onClick.AddListener(RevivePlayerWithAds);

        CheckRewardedAdsVisibility();
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
        hideRewardedAds = true;
        CheckRewardedAdsVisibility();
    }

    private void CheckRewardedAdsVisibility()
    {
        if (hideRewardedAds)
        {
            RewardedAdsHolder.SetActive(false);
        }
        else
        {
            RewardedAdsHolder.SetActive(true);
        }
    }
}
