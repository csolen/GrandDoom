using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject[] panels;

    private void Awake()
    {

    }

    public void OpenPanel(int index)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == index);
        }
    }

    public void GoAnotherLevel()
    {
        SceneManager.LoadScene("Level01");
    }
}
