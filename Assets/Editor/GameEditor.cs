using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEditor : EditorWindow
{
    private int sliderValue = 1;
    private string levelName = "Level_1";

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

    [MenuItem("DOOM/Load Level")]
    public static void OpenWindow()
    {
        EditorWindow.GetWindow<GameEditor>("Level Loader");
    }

    private void OnGUI()
    {
        GUILayout.Label("Load Level", EditorStyles.boldLabel);
        GUILayout.Space(5);

        int newSliderValue = EditorGUILayout.IntSlider("Level Number:", sliderValue, 1, 100);

        if (newSliderValue != sliderValue)
        {
            sliderValue = newSliderValue;
            levelName = $"Level_{sliderValue}";
        }

        GUILayout.Space(10);

        EditorGUILayout.LabelField("Level Name:", levelName);

        GUILayout.Space(15);

        if (GUILayout.Button("Load Scene"))
        {
            if (!EditorApplication.isPlaying)
            {
                SceneLoader(false);
            }
            else
            {
                SceneLoader(true);
            }
        }

    }

    private void SceneLoader(bool isEditorWorking)
    {
        if (isEditorWorking)
        {
            SceneManager.LoadScene(levelName);
        }
        else
        {
            EditorSceneManager.OpenScene("Assets/Scenes/" + levelName + ".unity");
        }
    }
}
