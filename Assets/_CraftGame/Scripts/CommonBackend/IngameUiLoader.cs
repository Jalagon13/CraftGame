using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
public class IngameUiLoader : MonoBehaviour
{
    void Start()
    {
        LoadUI();
    }

    [Button]
    public void LoadUI()
    {
        if (SceneManager.GetSceneByName("Ingame UI").isLoaded)
            return;
        if (Application.isPlaying)
            SceneManager.LoadSceneAsync("Scenes/Ingame UI", LoadSceneMode.Additive);
        #if UNITY_EDITOR
        else
            EditorSceneManager.OpenScene("Assets/_CraftGame/Scenes/DragToHierarchy/Ingame UI.unity", OpenSceneMode.Additive);
        #endif
    }

    [Button]
    public void UnloadUI()
    {
        if (!SceneManager.GetSceneByName("Ingame UI").isLoaded)
            return;
        if (Application.isPlaying)
            SceneManager.UnloadSceneAsync("Ingame UI");
#if UNITY_EDITOR
        else
            EditorSceneManager.CloseScene(EditorSceneManager.GetSceneByName("Ingame UI"), true);
#endif
    }
}