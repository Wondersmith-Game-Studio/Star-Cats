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
    private PanelRenderer _panelRenderer;
    private VisualElement _resourceListContainer;
    private Button _spriteContainer;
    //private VisualElement _upgrades;  WIRE LATER 8/29/2026
    //private VisualElement _navBar; WIRE LATER 8/29/2026
    //private readonly IReadOnlyDictionary<string, Currency> _currencyDict = CurrencyManager.Instance.Items;
    private Action  _currentClick;
    private string _selectedId;

//////////////////////////////////////////////////////////////////////////////////////////////

    #pragma warning disable CS0649
    [SerializeField] GameObject _mainGameScreen;
    #pragma warning disable CS0649

//////////////////////////////////////////////////////////////////////////////////////////////
    void Awake()
    {
        _mainGameScreen.SetActive(false);
    }
    
    void OnEnable()
    {
        _panelRenderer = GetComponent<PanelRenderer>();
        if (_panelRenderer != null)
        {
            _panelRenderer.RegisterUIReloadCallback(OnUIReload);
        }
        else
        {
            Debug.Log("PanelRenderer not found on MainGameController Object(MainGameController.cs)");
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
    private void BuildResourceList(VisualElement _resourceListContainer)
    {
        _resourceListContainer?.Clear();

        foreach (Currency c in CurrencyManager.Instance.Items.Values)
        {
            var row = new VisualElement { name = c.Id };
            row.AddToClassList("Resource");

            //var icon = new Image { name = $"{c.Id}IMG" };
            //icon.AddToClassList("ResourceImage");
           // row.Add(icon);

            var label = new Label($"{c.Id} : {c.Amount}") { name = $"{c.Id}Label" };
            label.AddToClassList("ResourceElement");
            row.Add(label);

            Debug.Log($"{_resourceListContainer}");
            Debug.Log($"{row}");

            _resourceListContainer.Add(row);
        }
    }

    public void UpdateResourceList(string id)
    {
        
    }

    private void OnUIReload(PanelRenderer renderer, VisualElement root, int version)
    {
        _spriteContainer = root.Q<Button>("MainGameImage");
        _resourceListContainer = root.Q<VisualElement>("ResourceList");
        _currentClick = null; //CLEAR CLICKHANDLER BEFORE REBUILD
        _selectedId = DataManager.GetString("selectedCurrency", "SpaceRock");

        BuildSpriteContainer(_selectedId);
        BuildResourceList(_resourceListContainer);
    }

    //CREATES TEXT PORTION OF CURRENCY UI
    private void UpdateLabel(string id)
    {
        var label = _resourceListContainer.Q<Label>($"{id}Label");
        if (label != null) label.text = $"{CurrencyManager.Instance.AmountOf(id)}";
    }
    
    //FUNCTION FOR BUILDING THE CLICKABLE CONTAINER IN CENTER OF SCREEN BASED ON CURRENCY ID
    private void BuildSpriteContainer(string id)
    {
        //SET SELECTED ID IN PERSISTENT STORAGE (SEE DATAMANGER.SAVEDATA())
        _selectedId = id;

        //FIND CURRENCY BY ID

        Debug.Log($"sprite={_spriteContainer}, currency={CurrencyManager.Instance?.Get(id)}, id={id}");
        Currency c = CurrencyManager.Instance.Get(id);
        if (c == null || _spriteContainer == null) return;

        //FIND USE CURRENCY'S SPRITE PROPERTY FOR BACKGROUND IMAGE
        //Sprite sprite = Resources.Load<Sprite>(c.Sprite);  INSERT SPRITE 8/29/2026
        //_spriteContainer.style.backgroundImage = new StyleBackground(sprite); INSERT SPRITE 8/29/2026

        //SET CLICK HANDLER FOR THE SPRITE   
        _currentClick = () => OnSpriteClicked(c);
        _spriteContainer.clicked += _currentClick;

        Debug.Log($"_resourceSprite Built: (MainGameController.BuildSpriteContainer( { id } )" );
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    ///ONCLICKEVENT FUNCTIONS
    //////////////////////////////////////////////////////////////////////////////////////////
    
    //RESOURCESPRITE CLICK EVENT FUNCTION
    //ADD VALUE
    //TRIGGER ANIMATION (NEEDS IMPLEMENTED - 8/29/2026)
    private void OnSpriteClicked(Currency c)
    {
        //ADD THE VALUE ASSOCIATED WITH CURRENCY BY ID
        CurrencyManager.Instance.Add(c.Id);

        //TRIGGER ANIMATION(NEEDS IMPLEMENTED - 8/29/2026)
        //THINKING JUST MAKE A NUMBER FLOAT UP FROM CLICK LOCATION BEFORE FADING. MAYBE SOME SINE WAVE BACK AND FORTH ACTION AS IT RISES

        Debug.Log("Sprite Click Registered: (MainGameController.OnResourceSpriteClicked)");
    }
}