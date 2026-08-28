using UnityEngine;
using UnityEngine.UIElements;
using Assets.Scripts.Utilities;
using System;
using Mono.Cecil.Cil;
using System.Data;

////////////////////////////////////////////////////////////////////////////////////////////////
public class MainMenuController : MonoBehaviour
{
    //////////////////////////////////////////////////////////////////////////////////////////
    
    private PanelRenderer _panelRenderer;
    private VisualElement _saveSelectorContainer;
    private VisualElement _newSaveContainer;
    private VisualElement _settingsContainer;
    
    //////////////////////////////////////////////////////////////////////////////////////////
    
    [SerializeField] private GameObject _mainMenuScreen;
    [SerializeField] private GameObject _mainGameScreen;

    //////////////////////////////////////////////////////////////////////////////////////////

    private void SwapToGameScreen()
    {
        _mainMenuScreen.SetActive(false);
        _mainGameScreen.SetActive(true);
    }
    //////////////////////////////////////////////////////////////////////////////////////////
    void OnEnable()
    {
        _panelRenderer = GetComponent<PanelRenderer>();
        if (_panelRenderer != null)
        {
            _panelRenderer.RegisterUIReloadCallback(OnUIReload);
        }
        else
        {
            Debug.LogError("PanelRenderer not found on this GameObject.");
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    void OnDisable()
    {
        _panelRenderer?.UnregisterUIReloadCallback(OnUIReload);
    }
    
    //////////////////////////////////////////////////////////////////////////////////////////
    
    private void OnUIReload(PanelRenderer renderer, VisualElement root, int version)
    {
        _saveSelectorContainer = root.Q<VisualElement>("SaveSelectorContainer");
        _newSaveContainer = root.Q<VisualElement>("NewSaveContainer");
        _settingsContainer = root.Q<VisualElement>("SettingsContainer");

        // Ensure SaveSelectorContainer and newSaveContainer starts hidden
        //////////////////////////////////////////////////////////////////////////////////////
        if (_saveSelectorContainer != null)
        {
            _saveSelectorContainer.style.display = DisplayStyle.None;
        }

        if(_newSaveContainer != null)
        {
            _newSaveContainer.style.display = DisplayStyle.None;
        }

        if(_settingsContainer != null)
        {
            _settingsContainer.style.display = DisplayStyle.None;
        }
        /////////////////////////////////////////////////////////////////////////////////////////

        //CLICK HANDLER LOGIC
        /////////////////////////////////////////////////////////////////////////////////////////
        var buttonContainer = root.Q<VisualElement>("ButtonContainer");
        
        if (buttonContainer != null)
        {
            Button loadButton = buttonContainer.Q<Button>("LoadGame");
            Button newButton = buttonContainer.Q<Button>("NewGame");
            Button settingsButton = buttonContainer.Q<Button>("Settings");

            if (loadButton != null) loadButton.clicked += OnLoadGameClicked;
            if (newButton != null) newButton.clicked += OnNewGameClicked;
            if (settingsButton != null) settingsButton.clicked += OnSettingsClicked;
        }
        else
        {
            Debug.LogError("ButtonContainer not found, check name in UIBuilder (MainMenuController) & MainMenuController.cs");
        }

        if (_newSaveContainer != null)
        {
            Button newSave1 = _newSaveContainer.Q<Button>("NewSave1");
            Button newSave2 = _newSaveContainer.Q<Button>("NewSave2");
            Button newSave3 = _newSaveContainer.Q<Button>("NewSave3");

            if (newSave1 != null) newSave1.clicked += OnNewSave1Clicked;
            if (newSave2 != null) newSave2.clicked += OnNewSave2Clicked;
            if (newSave3 != null) newSave3.clicked += OnNewSave3Clicked;
        }

        if (_saveSelectorContainer != null)
        {
            Button loadSave1 = _saveSelectorContainer.Q<Button>("Save1");
            Button loadSave2 = _saveSelectorContainer.Q<Button>("Save2");
            Button loadSave3 = _saveSelectorContainer.Q<Button>("Save3");

            if (loadSave1 != null) loadSave1.clicked += OnLoadSave1Clicked;
            if (loadSave2 != null) loadSave2.clicked += OnLoadSave2Clicked;
            if (loadSave3 != null) loadSave3.clicked += OnLoadSave3Clicked;
        }
    }
    //////////////////////////////////////////////////////////////////////////////////////////
    
    //ONCLICK EVENTS
    //////////////////////////////////////////////////////////////////////////////////////////
    private void OnLoadGameClicked()
    {
        Debug.Log("Load Game Button Clicked");
        if (_saveSelectorContainer != null)
        {
            _saveSelectorContainer.style.display =
                _saveSelectorContainer.style.display == DisplayStyle.None
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
        }
    }
    //////////////////////////////////////////////////////////////////////////////////////////
    ///NEW SAVE LOGIC
    private void OnNewGameClicked()
    {
        Debug.Log("New Game Button Clicked");
        if(_newSaveContainer != null)
        {
            _newSaveContainer.style.display =
                _newSaveContainer.style.display == DisplayStyle.None
                ? DisplayStyle.Flex
                : DisplayStyle.None;
        }
    }
    
    private void OnNewSave1Clicked()
    {
        
        SaveManager.CreateNewSave("Save1.json", success =>
        {
            if (success)
            {
                Debug.Log("Save1 created successfully.");
                //refresh the save selector list, close the New Save panel, etc.
                DataManager.Data = SaveManager.Load("Save1.json");
                SwapToGameScreen();
            }
            else
            {
                Debug.LogError("Failed to create Save1.");
                //show an error message in the UI
            }
        }
        #if UNITY_ANDROID && !UNITY_EDITOR
        , runner: this
        #endif
        );
    }
    
    private void OnNewSave2Clicked()
    {
        SaveManager.CreateNewSave("Save2.json", success =>
        {
            if (success)
            {
                Debug.Log("Save2 created successfully.");
                //refresh the save selector list, close the New Save panel, etc.
                DataManager.Data = SaveManager.Load("Save2.json");
                SwapToGameScreen();
            }
            else
            {
                Debug.LogError("Failed to create Save2.");
                //show an error message in the UI
            }
        }
        #if UNITY_ANDROID && !UNITY_EDITOR
        , runner: this
        #endif
        );
    }

    private void OnNewSave3Clicked()
    {
        SaveManager.CreateNewSave("Save3.json", success =>
        {
            if (success)
            {
                Debug.Log("Save3 created successfully.");
                //refresh the save selector list, close the New Save panel, etc.
                DataManager.Data = SaveManager.Load("Save3.json");
                SwapToGameScreen();
            }
            else
            {
                Debug.LogError("Failed to create Save3.");
                //show an error message in the UI
            }
        }
        #if UNITY_ANDROID && !UNITY_EDITOR
        , runner: this
        #endif
        );
    }

    private void OnLoadSave1Clicked()
    {
        DataManager.Data = SaveManager.Load("Save1.json");
        SwapToGameScreen();
        Debug.Log("Save1 Loaded Successfully!");
    }

    private void OnLoadSave2Clicked()
    {
        DataManager.Data = SaveManager.Load("Save2.json");
        SwapToGameScreen();
        Debug.Log("Save2 Loaded Successfully!");
    }

    private void OnLoadSave3Clicked()
    {
        DataManager.Data = SaveManager.Load("Save3.json");
        SwapToGameScreen();
        Debug.Log("Save3 Loaded Successfully!");
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////
    private void OnSettingsClicked()
    {
        Debug.Log("Settings Button Clicked");
        if(_settingsContainer != null)
        {
            _settingsContainer.style.display =
                _settingsContainer.style.display == DisplayStyle.None
                ? DisplayStyle.Flex
                : DisplayStyle.None;
        }
    }
}