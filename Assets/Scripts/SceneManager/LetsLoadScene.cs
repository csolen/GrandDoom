using UnityEngine;
using UnityEngine.SceneManagement;

public class LetsLoadScene : MonoBehaviour
{
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
