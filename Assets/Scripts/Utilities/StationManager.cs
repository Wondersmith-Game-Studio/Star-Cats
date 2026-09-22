using UnityEngine;
using Assets.Scripts.Utilities;
using System.Text.Json.Nodes;
using System.Collections.Generic;
using UnityEngine.Rendering;
using TMPro;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class StationManager : DataManager<Station>
{
    public static StationManager Instance { get; private set; }
    [SerializeField] GameObject StationPrefab;

    void Awake() => Instance = this;

    public void InitializeStationManager(JsonObject Data)
    {
        BuildFromSave(Data?["Stations"]);
        // spawn a GameObject per _items entry, positioned at (station.X, station.Y)
    }

    public void BuildStation(string taskId, Vector2 position)
    {
        var station = new Station
        {
            Id = $"{taskId}_Station_{_items.Count}",
            TaskId = taskId,
            X = position.x,
            Y = position.y,
        };

        _items[station.Id] = station;
        RaiseChanged(station.Id);

        // Instantiate(StationPrefab, position, Quaternion.identity), same as spawning a cat
    }

    public void UpdatePosition(string stationId, Vector2 position)
    {
        if (!_items.TryGetValue(stationId, out var station)) return;
        station.X = position.x;
        station.Y = position.y;
        RaiseChanged(stationId);
    }
}