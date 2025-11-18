using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEditor
{
    [MenuItem("DOOM/Restart Scene")]
    private static void RestartScene()
    {
        if (EditorApplication.isPlaying)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName);
        }
    }

    [MenuItem("DOOM/Data/Delete Save Data")]
    private static void DeleteSaveData()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("DOOM/Player/Immortal Mode")]
    private static void MakePlayerImmortal()
    {
        
    }

    [MenuItem("DOOM/Player/Unlimited Ammo")]
    private static void GivePlayerUnlimitedAmmos()
    {

    }

    [MenuItem("DOOM/Player/Unlimited Health")]
    private static void GivePlayerUnlimitedHealth()
    {

    }

    [MenuItem("DOOM/Player/Unlimited Coin")]
    private static void GivePlayerUnlimitedCoin()
    {

    }

}
