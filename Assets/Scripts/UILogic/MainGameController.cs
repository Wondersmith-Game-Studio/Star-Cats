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

    void Start()
    {
        Console.WriteLine(Data);
    }

    [SerializeField] private GameObject _mainGameScreen;
}