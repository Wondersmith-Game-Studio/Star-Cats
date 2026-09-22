using UnityEngine;
using Assets.Scripts.Utilities;
using System.Text.Json.Nodes;
using System.Collections.Generic;
using UnityEngine.Rendering;
using TMPro;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class StationDraggable : MonoBehaviour
{
    private string _stationId;
    private bool _dragging;

    public void Bind(string stationId) => _stationId = stationId;

    void Update()
    {
        // on mouse down over this collider: _dragging = true
        // while _dragging: transform.position = mouse world position
        // on mouse up: _dragging = false, then commit the position back:
        //   StationManager.Instance.UpdatePosition(_stationId, transform.position);
    }
}

