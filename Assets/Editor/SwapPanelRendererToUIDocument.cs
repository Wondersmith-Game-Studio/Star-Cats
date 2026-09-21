//ONE-TIME EDITOR UTILITY - SWAPS PanelRenderer FOR UIDocument ON MainGameScreen/MainMenu
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public static class SwapPanelRendererToUIDocument
{
    [MenuItem("Tools/One-Time Fixes/Swap PanelRenderer to UIDocument")]
    static void Swap()
    {
        foreach (var name in new[] { "MainGameScreen", "MainMenu" })
        {
            var go = GameObject.Find(name);
            if (go == null)
            {
                Debug.LogWarning($"Could not find GameObject named '{name}' in the active scene.");
                continue;
            }

            var panelRenderer = go.GetComponent<PanelRenderer>();
            if (panelRenderer == null)
            {
                Debug.LogWarning($"'{name}' has no PanelRenderer component - skipping.");
                continue;
            }

            PanelSettings panelSettings = panelRenderer.panelSettings;
            VisualTreeAsset sourceAsset = panelRenderer.sourceAsset as VisualTreeAsset;

            Undo.DestroyObjectImmediate(panelRenderer);

            var doc = Undo.AddComponent<UIDocument>(go);
            doc.panelSettings = panelSettings;
            doc.visualTreeAsset = sourceAsset;

            Debug.Log($"Swapped PanelRenderer -> UIDocument on '{name}'.");
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
}
