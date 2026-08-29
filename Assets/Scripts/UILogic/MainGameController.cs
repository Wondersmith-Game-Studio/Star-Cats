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
        _currentClick = null; //CLEAR CLICKHANDLER BEFORE REBUILD
        _selectedId = GetString("selectedCurrency", "SpaceRock");

        BuildSpriteContainer(_selectedId);
        BuildResourceList(root);
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
        Currency c = CurrencyManager.Instance.Get(id);
        if (c == null || _spriteContainer == null) return;

        //FIND USE CURRENCY'S SPRITE PROPERTY FOR BACKGROUND IMAGE
        _spriteContainer.style.backgroundImage = new StyleBackground(c.Sprite);

        //REMOVE OLD HANDLER SO IT DOESN'T STACK
        if (_currentClick != null) _currentClick -= OnSpriteClicked;

        //CREATE NEW HANDLER FOR THIS CURRENCY, REMEMBER IT        
        _currentClick = () => OnSpriteClicked(c);
        _spriteContainer.clicked += _currentClick;

        Debug.Log($"_resourceSprite Built: (MainGameController.BuildSpriteContainer({id.Id})");
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
        CurrencyManager.Instance.Add(c.Id, c.Value);

        //TRIGGER ANIMATION(NEEDS IMPLEMENTED - 8/29/2026)
        //THINKING JUST MAKE A NUMBER FLOAT UP FROM CLICK LOCATION BEFORE FADING. MAYBE SOME SINE WAVE BACK AND FORTH ACTION AS IT RISES

        Debug.Log("Sprite Click Registered: (MainGameController.OnResourceSpriteClicked)");
    }
}