using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DeathScreenManager : MonoBehaviour
{
    public GameObject deathScreenPanel;

    public Button reviveBtn;
    public Button reviveWithAdsBtn;
    public Button restartBtn;

    public GameObject RewardedAdsHolder;

    private bool hideRewardedAds;

    public int reviveCost = 40;

    public TextMeshProUGUI reviveCostText;

    public void Start()
    {
        restartBtn.onClick.AddListener(RestartCurrentScene);
        reviveBtn.onClick.AddListener(RevivePlayerWithGold);
        reviveWithAdsBtn.onClick.AddListener(RevivePlayerWithAds);

        reviveCostText.text = reviveCost.ToString();

        CheckRewardedAdsVisibility();
    }

    private void Update()
    {
        if (!deathScreenPanel.activeInHierarchy)
        {
            return;
        }

        if (PlayerController.instance.goldAmount >= reviveCost)
        {
            reviveBtn.interactable = true;
        }
        else
        {
            reviveBtn.interactable = false;
        }
    }

    private void RevivePlayerWithGold()
    {
        PlayerController.instance.RevivePlayer(1);
        PlayerController.instance.AddGold(-reviveCost);
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
