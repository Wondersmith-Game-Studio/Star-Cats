using System;
using UnityEditor;
using UnityEngine;
using Assets.Scripts.Utilities;

public static class HeadlessBootstrap
{
    [MenuItem("Tools/IncrementalGame/Run Headless Debug")]
    public static void Run()
    {
        try
        {
            Debug.Log("[HeadlessBootstrap] Starting Unity batch debug pass...");

            var saveData = SaveManager.Load("SaveData.json");
            Debug.Log($"[HeadlessBootstrap] Loaded save root entries: {saveData.Count}");

            var bootstrapObject = new GameObject("HeadlessBootstrapObject");
            var currencyManager = bootstrapObject.AddComponent<CurrencyManager>();
            currencyManager.InitializeCurrencyManager(saveData);

            if (currencyManager.Items.Count == 0)
            {
                Debug.LogWarning("[HeadlessBootstrap] No currencies found in save data. This is expected on a fresh save.");
            }
            else
            {
                foreach (var item in currencyManager.Items)
                {
                    Debug.Log($"[HeadlessBootstrap] Currency: {item.Key} = {item.Value.Amount}");
                }
            }

            Debug.Log($"[HeadlessBootstrap] Headless run complete. Unity version: {Application.unityVersion}");

            UnityEngine.Object.DestroyImmediate(bootstrapObject);
            EditorApplication.Exit(0);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[HeadlessBootstrap] Failed: {ex}");
            EditorApplication.Exit(1);
        }
    }
}
