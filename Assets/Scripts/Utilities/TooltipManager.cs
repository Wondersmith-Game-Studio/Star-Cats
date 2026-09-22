using UnityEngine;
using UnityEngine.UIElements;
using Assets.Scripts.Utilities;
using System.Text.Json;
using System.Text.Json.Nodes;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem.iOS;
using Mono.Cecil.Cil;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance { get; private set; }

    [SerializeField] UIDocument uiDocument;
    [SerializeField] VisualTreeAsset tooltipTemplate;

    VisualElement _tooltip;
    Label _label;

    void Awake()
    {
        Instance = this;
        _tooltip = tooltipTemplate.Instantiate();
        _label = _tooltip.Q<Label>("TooltipLabel");
        _tooltip.style.display = DisplayStyle.None;
        uiDocument.rootVisualElement.Add(_tooltip);
    }

    public void Show(string text, Vector2 screenPosition)
    {
        _label.text = text;
        _tooltip.style.left = screenPosition.x;
        _tooltip.style.top = screenPosition.y;
        _tooltip.style.display = DisplayStyle.Flex;
    }

    public void Hide() => _tooltip.style.display = DisplayStyle.None;
}