using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
using IncrementalGame.Utilities;

public class MainMenuController : MonoBehaviour
{
    private UIDocument _document;
    private  VisualElement _saveSelectorContainer;

    void Start()
    {
        //Variable Declarations for Document and Root
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        _document = GetComponent<UIDocument>();
        var root = _document.rootVisualElement;
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Query Parent Containers
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        var buttonContainer = root.Q<VisualElement>("ButtonContainer");
        _saveSelectorContainer = root.Q<VisualElement>("SaveSelectorContainer");
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (buttonContainer != null)
        {
            //Query the Buttons Within the Containers
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Button loadButton = buttonContainer.Q<Button>("LoadGame");
            Button newButton = buttonContainer.Q<Button>("NewGame");
            Button settingsButton = buttonContainer.Q<Button>("Settings");
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Attach Listeners
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (loadButton != null) loadButton.clicked += OnLoadGameClicked;
            if (newButton != null) newButton.clicked += OnNewGameClicked;
            if (settingsButton != null) settingsButton.clicked += OnSettingsClicked;
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }
        else
        {
            Debug.LogError("ButtonContainer not found, check name in UIBuilder (MainMenuController Line 14)");
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if (_saveSelectorContainer != null)
        {
            //Ensure SaveSelectorContainer Starts Hidden
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            _saveSelectorContainer.style.display = DisplayStyle.None;
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Query Buttons within container
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Button save1button = _saveSelectorContainer.Q<Button>("Save1");
            Button save2button = _saveSelectorContainer.Q<Button>("Save2");
            Button save3button = _saveSelectorContainer.Q<Button>("Save3");
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            
            //Attach Listeners
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (save1button != null)
            {
                //STUB--WIRE LATER
                save1button.clicked += OnSave1ButtonClicked;
            }
            else
            {
                Debug.LogError("Save1 Button not found, check MainMenu in UIBuilder and Button Logic in MainMenuController.cs");
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (save2button != null)
            {
                //STUB--WIRE LATER
                save2button.clicked += OnSave2ButtonClicked;
            }
            else
            {
                Debug.LogError("Save2 Button not found, check MainMenu in UIBuilder and Button Logic in MainMenuController.cs");
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (save3button != null)
            {
                //STUB--WIRELATER
                save3button.clicked += OnSave3ButtonClicked;
            }
            else
            {
                Debug.LogError("Save3 Button not found, check MainMenu in UIBuilder and Button Logic in MainMenuController.cs");
            }
        }
        else
        {
            Debug.LogError("SaveSelectorContainer not found, check MainMenu in UIBuilder and Button Logic in MainMenuController.cs");
        }
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void OnLoadGameClicked()
    {
        Debug.Log("Load Game Button Clicked");
        if (_saveSelectorContainer != null)
        {
            if (_saveSelectorContainer.style.display == DisplayStyle.None)
            {
                _saveSelectorContainer.style.display = DisplayStyle.Flex;
            }
            else
            {
                _saveSelectorContainer.style.display = DisplayStyle.None;
            }
        }
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void OnNewGameClicked()
    {
        //INSERT "CREATE NEW GAME" LOGIC HERE LATER
        Debug.Log("New Game Button Clicked");
        if (_saveSelectorContainer != null)
        {
            if (_saveSelectorContainer.style.display == DisplayStyle.None)
            {
                _saveSelectorContainer.style.display = DisplayStyle.Flex;
            }
            else
            {
                _saveSelectorContainer.style.display = DisplayStyle.None;
            }
        }
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void OnSettingsClicked()
    {
        //INSERT "SETTINGS BUTTON CLICKED" LOGIC HERE LATER
        Debug.Log("Settings Button Clicked");
    }
}