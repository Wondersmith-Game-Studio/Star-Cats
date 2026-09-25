using UnityEngine;
using UnityEngine.InputSystem;
using Assets.Scripts.Utilities;
using UnityEngine.UIElements;

public class HoverTooltip : MonoBehaviour
{
    [SerializeField] string TooltipText;
    [SerializeField] string[] AvailableUpgradeIds;
    bool _hovering;

    void Update()
    {
        if (Camera.main == null) return;

        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        bool isOver = GetComponent<Collider2D>().OverlapPoint(worldPoint);
        

        if (isOver && !_hovering)
            TooltipManager.Instance.ShowUpgrades(TooltipText, AvailableUpgradeIds, Mouse.current.position.ReadValue());

        else if (!isOver && _hovering && !TooltipManager.Instance.IsPointerOverTooltip)
            TooltipManager.Instance.Hide();

        _hovering = isOver;
    }
}