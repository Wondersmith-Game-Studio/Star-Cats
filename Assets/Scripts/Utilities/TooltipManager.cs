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
using UnityEditor;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance { get; private set; }

    [SerializeField] UIDocument uiDocument;
    [SerializeField] VisualTreeAsset tooltipTemplate;
    [SerializeField] VisualTreeAsset upgradeButtonTemplate;

    VisualElement _tooltip;
    VisualElement _upgrades;
    Label _label;
    public bool IsPointerOverTooltip { get; set; }

    void Awake()
    {
        Instance = this;
        _tooltip = tooltipTemplate.Instantiate();
        _label = _tooltip.Q<Label>("TooltipLabel");
        _upgrades= _tooltip.Q<VisualElement>("UpgradeContainer");
        _tooltip.style.position = Position.Absolute;
        _tooltip.style.display = DisplayStyle.None;

        _tooltip.RegisterCallback<PointerEnterEvent>(_ => IsPointerOverTooltip = true);
        _tooltip.RegisterCallback<PointerLeaveEvent>(_ => IsPointerOverTooltip = false);
    }

    public void ShowUpgrades(string text, string[] upgrades, Vector2 screenPosition)
    {
        BuildTooltip(text, screenPosition);
        BuildUpgradeContainer(upgrades);
    }

    public void BuildTooltip(string text, Vector2 screenPosition)
    {
        _label.text = text;

        Vector2 panelPosition = RuntimePanelUtils.ScreenToPanel(
            uiDocument.rootVisualElement.panel,
            new Vector2(screenPosition.x, Screen.height - screenPosition.y));

        _tooltip.style.left = panelPosition.x;
        _tooltip.style.top = panelPosition.y;
        _tooltip.style.display = DisplayStyle.Flex;

        uiDocument.rootVisualElement.Add(_tooltip);
    }

    public void BuildUpgradeContainer(string[] upgrades)
    {
        _upgrades?.Clear();

        foreach (string id in upgrades)
        {
            Upgrade upgrade = UpgradeManager.Instance.Get(id);
            if (upgrade == null || upgrade.Acquired) continue;

            VisualElement row = upgradeButtonTemplate.Instantiate();
            Button button = row.Q<Button>("UpgradeButtonButton");
            button.text = $"{upgrade.Id}\n{BuildCostText(upgrade.Costs)}";
            button.clicked += () =>
            {
                UpgradeManager.Instance.PurchaseUpgrade(upgrade);
                BuildUpgradeContainer(upgrades);
            };
            _upgrades.Add(row);
        }
    }

    private string BuildCostText(Cost[] costs)
    {
        string text = "";

        for (int i = 0; i < costs.Length; i++)
        {
            text += $"{costs[i].Amount} {costs[i].CurrencyId}";
                    
            if (i < costs.Length - 1)
                text += ", ";
        }
        return text;
    }
    public void Hide() => _tooltip.style.display = DisplayStyle.None;
}