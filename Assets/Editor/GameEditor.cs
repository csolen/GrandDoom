using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
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

    [MenuItem("DOOM/Player/Mode/Immortal Mode")]
    private static void MakePlayerImmortal()
    {
        PlayerController.instance.health = 10000;
        PlayerController.instance.ammoAmount = 10000;
        PlayerController.instance.goldAmount = 10000;
    }

    [MenuItem("DOOM/Player/Mode/Normal Mode")]
    private static void MakePlayerMortal()
    {
        PlayerController.instance.health = 100;
        PlayerController.instance.ammoAmount = 15;
        PlayerController.instance.goldAmount = 50;
    }

    [MenuItem("DOOM/Player/Add Ammo")]
    private static void GivePlayerAmmos()
    {
        PlayerController.instance.ammoAmount = 100;
    }

    [MenuItem("DOOM/Player/Add Health")]
    private static void GivePlayeHealth()
    {
        PlayerController.instance.health = 100;
    }

    [MenuItem("DOOM/Player/Add Coin")]
    private static void GivePlayerCoin()
    {
        PlayerController.instance.goldAmount = 100;
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
