using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGameManager : MonoBehaviour
{
    public GameObject pauseTheGamePanel;

    public Button pauseGameBtn;
    public Button returnMainPageBtn;
    public Button[] resumeGameBtn;

    public Button soundButton;
    public Button musicButton;

    private RoguelikeManager m_rogueLikeManager;
    public GameObject cardsParentObj;
    public Image selectedSkillIconPrefab;

    private void Start()
    {
        m_rogueLikeManager = GetComponent<RoguelikeManager>();

        pauseGameBtn.onClick.AddListener(OpenPauseGamePanel);
        returnMainPageBtn.onClick.AddListener(ReturnToMainGame);

        soundButton.onClick.AddListener(() => ChangeMusicAndSoundButtons("isSoundOn"));
        musicButton.onClick.AddListener(() => ChangeMusicAndSoundButtons("isMusicOn"));

        for (int i = 0; i < resumeGameBtn.Length; i++)
        {
            resumeGameBtn[i].onClick.AddListener(ClosePauseGamePanel);
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseTheGamePanel.activeInHierarchy)
            {
                OpenPauseGamePanel();
            }
        }
#endif
    }

    private void OpenPauseGamePanel()
    {
        pauseTheGamePanel.SetActive(true);
        RefreshSelectedSkillIcons();
        GameTester.Instance.ShouldStopTheGame(true);
        Time.timeScale = 0f;
    }

    public void ClosePauseGamePanel()
    {
        pauseTheGamePanel.SetActive(false);
        GameTester.Instance.ShouldStopTheGame(false);
        Time.timeScale = 1f;
    }

    public void ReturnToMainGame()
    {
        Debug.Log("Go back to main menu");
    }

    private void ChangeMusicAndSoundButtons(string keyName)
    {
        if (PlayerPrefs.GetInt(keyName) == 0)
        {
            PlayerPrefs.SetInt(keyName, 1);
        }
        else
        {
            PlayerPrefs.SetInt(keyName, 0);
        }
    }

    private void RefreshSelectedSkillIcons()
    {
        for (int i = cardsParentObj.transform.childCount - 1; i >= 0; i--)
            Destroy(cardsParentObj.transform.GetChild(i).gameObject);

        var seenGroups = new HashSet<string>();

        foreach (var s in m_rogueLikeManager.allSkills)
        {
            if (s == null) continue;

            string gid = s.levelGroupId;
            if (string.IsNullOrEmpty(gid)) continue;

            if (!seenGroups.Add(gid)) continue;

            int lvl = PlayerPrefs.GetInt("SkillLevel_" + gid, 0);
            if (lvl <= 0) continue;

            Image img = Instantiate(selectedSkillIconPrefab, cardsParentObj.transform);
            img.sprite = s.icon;
            img.enabled = (img.sprite != null);
        }
    }

}
