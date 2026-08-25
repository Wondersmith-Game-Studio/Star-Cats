using UnityEngine;
using UnityEngine.UIElements;
using Assets.Scripts.Utilities;
using System.Text.Json;
using System.Text.Json.Nodes;
using JetBrains.Annotations;
using System;

////////////////////////////////////////////////////////////////////////////////////////////////
public class MainGameController : MonoBehaviour
{
    private PanelRenderer _panelRenderer;
    private VisualElement _resourceList;
    private VisualElement _image;
    private VisualElement _upgrades;
    private VisualElement _navBar;

    public static JsonObject Data;

    [SerializeField] private UIDocument MainGameScreen

    private void OnEnable()
    {
        var _resourceList = MainGameScreen.ResourceList;
        
        if (_resourceList == null) return;

        foreach (var Currency in Data)
        {
            VisualElement newItem = 
        }
    }

    [SerializeField] private GameObject _mainGameScreen;
}