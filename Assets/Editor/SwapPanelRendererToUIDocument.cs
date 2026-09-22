//ONE-TIME EDITOR UTILITY - SWAPS PanelRenderer FOR UIDocument ON MainGameScreen/MainMenu
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public static class SwapPanelRendererToUIDocument
{
    [MenuItem("Tools/One-Time Fixes/Swap PanelRenderer to UIDocument")]
    static void Swap()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogError("Stop Play Mode before running this - scene changes made during Play Mode are discarded when you stop.");
            return;
        }

        foreach (var name in new[] { "MainGameScreen", "MainMenu" })
        {
            var go = FindInActiveScene(name);
            if (go == null)
            {
                Debug.LogWarning($"Could not find GameObject named '{name}' in the active scene.");
                continue;
            }

            if (go.GetComponent<UIDocument>() != null)
            {
                Debug.Log($"'{name}' already has a UIDocument component - skipping.");
                continue;
            }

            var panelRendererType = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name == "PanelRenderer" && typeof(Component).IsAssignableFrom(t));

            if (panelRendererType == null)
            {
                Debug.LogWarning($"'{name}' does not contain a PanelRenderer in this Unity version; no conversion was needed.");
                continue;
            }

            var panelRenderer = go.GetComponent(panelRendererType) as Component;
            if (panelRenderer == null)
            {
                Debug.LogWarning($"'{name}' has no PanelRenderer component - skipping.");
                continue;
            }

            PanelSettings panelSettings = null;
            VisualTreeAsset sourceAsset = null;

            try
            {
                var panelSettingsProp = panelRendererType.GetProperty("panelSettings");
                var sourceAssetProp = panelRendererType.GetProperty("sourceAsset");

                panelSettings = panelSettingsProp?.GetValue(panelRenderer) as PanelSettings;
                sourceAsset = sourceAssetProp?.GetValue(panelRenderer) as VisualTreeAsset;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Could not read UI Toolkit data from '{name}': {ex.Message}");
            }

            Undo.DestroyObjectImmediate(panelRenderer);

            var doc = Undo.AddComponent<UIDocument>(go);
            doc.panelSettings = panelSettings;
            doc.visualTreeAsset = sourceAsset;

            Debug.Log($"Swapped PanelRenderer -> UIDocument on '{name}'.");
        }

        var scene = EditorSceneManager.GetActiveScene();
        if (scene.IsValid())
            EditorSceneManager.MarkSceneDirty(scene);
    }

    //FINDS AN OBJECT BY NAME EVEN IF IT'S CURRENTLY INACTIVE - GameObject.Find CANNOT SEE THOSE
    static GameObject FindInActiveScene(string name)
    {
        var scene = EditorSceneManager.GetActiveScene();
        foreach (var root in scene.GetRootGameObjects())
        {
            var found = FindRecursive(root.transform, name);
            if (found != null) return found.gameObject;
        }
        return null;
    }

    static Transform FindRecursive(Transform parent, string name)
    {
        if (parent.name == name) return parent;

        foreach (Transform child in parent)
        {
            var result = FindRecursive(child, name);
            if (result != null) return result;
        }

        return null;
    }
}
