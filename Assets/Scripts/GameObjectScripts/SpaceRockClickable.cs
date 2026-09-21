//THIS WILL BE THE SCRIPT HELD BY THE SPRITE-ROCK GAMEOBJECT
using UnityEngine;
using UnityEngine.InputSystem;
using Assets.Scripts.Utilities;

public class SpaceRockClickable : MonoBehaviour
{
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            if (GetComponent<Collider2D>().OverlapPoint(worldPoint))
                OnClicked();
        }
    }

    private void OnClicked()
    {
        CurrencyManager.Instance.Add("SpaceRock");
    }
}
