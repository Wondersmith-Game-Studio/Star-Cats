using UnityEngine;
using UnityEngine.UIElements;
using Assets.Scripts.Utilities;
using System.Text.Json;
using System.Text.Json.Nodes;
using JetBrains.Annotations;
using System;

//////////////////////////////////////////////////////////////////////////////////////////////

public class MainGameController : MonoBehaviour
{
    private PanelRenderer _panelRenderer;
    private VisualElement _resourceListContainer;
    private Button _spriteContainer;
    private VisualElement _upgrades;
    private VisualElement _navBar;
    private static currencyDict = CurrencyManager.Instance.Items;

//////////////////////////////////////////////////////////////////////////////////////////////

    [SerializeField] GameObject _mainGameScreen;

//////////////////////////////////////////////////////////////////////////////////////////////
    void OnEnable()
    {
        _panelRenderer = GetComponent<PanelRenderer>();
        if (_panelRenderer != null)
        {
            _panelRenderer.RegisterUIReloadCallback(OnUIReload);
        }
        else
        {
            Debug.Log("PanelRenderer not found on MainGameController Object(MainGameController.cs)")
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////

    void OnDisable()
    {
        _panelRenderer?.UnregisterUIReloadCallback(OnUIReload);
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    ///DYNAMICALLY POPULATE UI BY ITERATING THROUGH EXISTING CURRENCYLIST
    ///BUILT BY DATAMANAGER -> CURRENCYMANAGER, POPULATING VLAUES FROM CURRENCYMANAGER
    //////////////////////////////////////////////////////////////////////////////////////////
    ///NOTE TO SELF 8/28/2026: INSERT ICON.SPRITE PATHWAY ONCE VISUALS EXIST,
    ///ADD BOOL VALUES TO DETERMINE IF VISUAL ELEMENT SHOULD BE CREATED OR NOT
    ///DEPENDENT ON IF PLAYER HAS UNLOCKED CURRENCY - PROBABLY AS PROPERTY ATTACHED TO OBJECTS
    //////////////////////////////////////////////////////////////////////////////////////////
    private void BuildResourceList(VisualElement root)
    {
        _resourceListContainer = root.Q<VisualElement>("ResourceList");
        _resourceListContainer.Clear();

        foreach (Currency c in currencyDict)
        {
            var row = new VisualElement { name = c.Id };
            row.AddToClassList("Resource");

            var icon = new Image { name = $"{c.Id}IMG" };
            icon.AddToClassList("ResourceImage");
            row.Add(icon);

            var label = new Label($"{c.Id} : {c.Amount}") { name = $"{c.Id}Label" };
            label.AddToClassList("ResourceElement");
            row.Add(label);

            _resourceListContainer.Add(row);
        }
    }

    private void OnUIReload(PanelRenderer renderer, VisualElement root, int version)
    {
        _spriteContainer = root.Q<Button>("MainGameImage");
        var _resourceSprite = new Button { name = $"{c.Id}Sprite" } { img src = (c.resourceSprite) };

        BuildResourceList(root);
        BuildSpriteContainer();

        CurrencyManager.Instance.OnChanged += UpdateLabel;
        if (_resourceSprite != null) _resourceSprite.clicked += OnResourceSpriteClicked(c)
    }

    private void UpdateLabel(string id)
    {
        var label = _resourceListContainer.Q<Label>($"{id}Label");
        if (label != null) label.text = $"{CurrencyManager.Instance.AmountOf(id)}";
    }
    //////////////////////////////////////////////////////////////////////////////////////////
    ///END OF DYNAMIC CURRENCY UI POPULATION
    //////////////////////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////////////////////////////
    ///SCRIPT FOR CLICKING SPRITE IMAGE TO ADD CURRENCY
    private void BuildSpriteContainer(string id)
    {
        Currency c = CurrencyManager.Instance.Get(id);
        if (c == null || _spriteContainer == null) return;

        _spriteContainer.Clear();

        _spriteContainer.AddToClassList("Image");

        Debug.Log($"_resourceSprite Built: (MainGameController.BuildSpriteContainer({id.Id})")
    }

    private void OnResourceSpriteClicked(string id)
    {
        Debug.Log("Sprite Click Registered: (MainGameController.OnResourceSpriteClicked)");
    }
}