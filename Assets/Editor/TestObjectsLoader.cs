using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class TestObjectsLoader : MonoBehaviour
{
    private static readonly string[] prefabPaths =
    {
        "Assets/Prefabs/Dev/GameTester.prefab"
    };

    private static readonly System.Collections.Generic.List<GameObject> instantiatedObjects = new();

    static TestObjectsLoader()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode) // && !IsSceneIsFirstScene()
        {
            RuntimeAddPrefabs();
        }
        else if (state == PlayModeStateChange.ExitingPlayMode)
        {
            RemovePrefabs();
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void RuntimeAddPrefabs()
    {
        if (!Application.isEditor) return;

        foreach (string path in prefabPaths)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab != null)
            {
                GameObject instance = Object.Instantiate(prefab);
                instantiatedObjects.Add(instance);
            }
        }
    }

    private static void RemovePrefabs()
    {
        foreach (GameObject obj in instantiatedObjects)
        {
            if (obj != null)
            {
                Object.DestroyImmediate(obj);
            }
        }
        instantiatedObjects.Clear();
    }

    private static bool IsSceneIsFirstScene()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0;
    }
}
