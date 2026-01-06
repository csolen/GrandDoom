using UnityEngine;
using UnityEngine.UI;

public class PauseGameManager : MonoBehaviour
{
    public Button pauseGameBtn;
    public Button closeGameBtn;
    public Button returnMainPageBtn;

    public GameObject pauseTheGamePanel;

    private void Start()
    {
        pauseGameBtn.onClick.AddListener(OpenPauseGamePanel);
        closeGameBtn.onClick.AddListener(ClosePauseGamePanel);
        returnMainPageBtn.onClick.AddListener(ReturnToMainGame);
    }

    private void OpenPauseGamePanel()
    {
        pauseTheGamePanel.SetActive(true);
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
}
