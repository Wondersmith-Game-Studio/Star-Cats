using UnityEngine;
using Assets.Scripts.Utilities;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Linq;
using System.Collections.Generic;

/////////////////////////////////////////////////////////////////////

public interface IHasId
{
    string Id { get; }
}

/////////////////////////////////////////////////////////////////////

public class Currency: IHasId
{
    public string Id { get; set; }
    public double Amount { get; set; }
    public double Value { get; set; }
}

public abstract class DataManager<T> : MonoBehaviour where T : IHasId
{
    protected readonly Dictionary<string, T> _items = new();
    public IReadOnlyDictionary<string, T> Items => _items;
    public int Count => _items.Count;

    public event Action<string> OnChanged;
    protected void RaiseChanged(string id) =>OnChanged?.Invoke(id);

    protected void BuildFromSave(JsonNode itemsArray)
    {
        _items.Clear();
        if (itemsArray == null) return;

        var list = itemsArray.Deserialize<List<T>>(
            new JsonSerializerOptions { PropertyNameCaseIncensitive = true });
        
        if (list == null) return;
        foreach (T item in list)
            _items[item.Id] = item;
    }

    prot
}