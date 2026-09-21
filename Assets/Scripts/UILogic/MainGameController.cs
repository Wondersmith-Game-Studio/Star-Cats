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

//////////////////////////////////////////////////////////////////////////////////////////////

public class MainGameController : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _resourceListContainer;
    //private VisualElement _upgrades;  WIRE LATER 8/29/2026
    //private VisualElement _navBar; WIRE LATER 8/29/2026

//////////////////////////////////////////////////////////////////////////////////////////////

    #pragma warning disable CS0649
    [SerializeField] GameObject _mainGameScreen;
    [SerializeField] VisualTreeAsset resourceRowTemplate; //ResourceRowTemplate.uxml
    #pragma warning disable CS0649

//////////////////////////////////////////////////////////////////////////////////////////////
    void Awake()
    {
        _mainGameScreen.SetActive(false);
    }

    void OnEnable()
    {
        _uiDocument = GetComponent<UIDocument>();
        if (_uiDocument != null)
        {
            BuildUI(_uiDocument.rootVisualElement);
        }
        else
        {
            Debug.Log("UIDocument not found on MainGameController Object(MainGameController.cs)");
        }

        CurrencyManager.Instance.OnChanged += OnCurrencyChanged;
    }

    //////////////////////////////////////////////////////////////////////////////////////////

    void OnDisable()
    {
        CurrencyManager.Instance.OnChanged -= OnCurrencyChanged;
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    ///DYNAMICALLY POPULATE UI BY ITERATING THROUGH EXISTING CURRENCYLIST
    ///BUILT BY DATAMANAGER -> CURRENCYMANAGER, POPULATING VLAUES FROM CURRENCYMANAGER
    //////////////////////////////////////////////////////////////////////////////////////////
    ///NOTE TO SELF 8/28/2026: INSERT ICON.SPRITE PATHWAY ONCE VISUALS EXIST,
    ///ADD BOOL VALUES TO DETERMINE IF VISUAL ELEMENT SHOULD BE CREATED OR NOT
    ///DEPENDENT ON IF PLAYER HAS UNLOCKED CURRENCY - PROBABLY AS PROPERTY ATTACHED TO OBJECTS
    //////////////////////////////////////////////////////////////////////////////////////////
    private void BuildResourceList(VisualElement _resourceListContainer)
    {
        _resourceListContainer?.Clear();

        foreach (Currency c in CurrencyManager.Instance.Items.Values)
        {
            VisualElement row = resourceRowTemplate.Instantiate();
            row.name = c.Id;
            row.AddToClassList("Resource");

            var img = row.Q<Image>("ResourceIMG");
            img.name = $"{c.Id}IMG";
            if (!string.IsNullOrEmpty(c.Sprite))
                img.sprite = Resources.Load<Sprite>(c.Sprite);

            var label = row.Q<Label>("ResourceLabel");
            label.name = $"{c.Id}Label";
            label.text = $"{c.Id} : {c.Amount}";

            _resourceListContainer.Add(row);
        }
    }

    private void BuildUI(VisualElement root)
    {
        _resourceListContainer = root.Q<VisualElement>("ResourceList");

        BuildResourceList(_resourceListContainer);
    }

    //CREATES TEXT PORTION OF CURRENCY UI
    private void UpdateLabel(Currency c)
    {
        var label = _resourceListContainer.Q<Label>($"{c.Id}Label");
        if (label != null) label.text = $"{c.Id} : {c.Amount}";
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    ///ONCHANGED HANDLER - FIRES WHENEVER A CURRENCY CHANGES, REGARDLESS OF WHAT TRIGGERED IT
    ///(EG: SPRITE-ROCK CLICKS IN THE SCENE, NOT JUST UI)
    //////////////////////////////////////////////////////////////////////////////////////////
    private void OnCurrencyChanged(string id)
    {
        Currency c = CurrencyManager.Instance.Get(id);
        if (c != null) UpdateLabel(c);
    }
}
