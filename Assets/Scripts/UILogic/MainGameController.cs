using UnityEngine;
using UnityEngine.UIElements;
using Assets.Scripts.Utilities;

////////////////////////////////////////////////////////////////////////////////////////////////
public class MainGameController : MonoBehaviour
{
    private PanelRenderer _panelRenderer;
    private VisualElement _resourceList;
    private VisualElement _image;
    private VisualElement _upgrades;
    private VisualElement _navBar;

    [SerializeField] private GameObject _mainMenuScreen;
    [SerializeField] private GameObject _mainGameScreen;

    private void SwapToGameScreen()
    {
        _mainMenuScreen.SetActive(false);
        _mainGameScreen.SetActive(true);
    }

    private static ResourceList(currencyDict)
}