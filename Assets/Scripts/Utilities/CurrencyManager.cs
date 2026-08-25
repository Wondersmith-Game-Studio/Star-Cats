using UnityEngine;
using UnityEngine.UIElements;
using Assets.Scripts.Utilities;
using Assets.Scripts.UILogic;
using System.Text.Json;
using System.Text.Json.Nodes;
using JetBrains.Annotations;
using System;

//////////////////////////////////////////////////////////////

pubic class CurrencyManager : MonoBehaviour
{
    public static Json Object Data;

    public class Currency
    {
        public string Id { get; set; }
        public double Amount { get; set; }
        public double Value { get; set; }
    }

    public class SaveData
    {
        public List<Currency> currencies { get; set; } = new();
    }

    private Dictionary<string, Currency>_currencies;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _currencies= BuildFromSave(Data);
            if (Data = null)
                return;
    }

    public static CurrencyManager Instance { get; private set; } 

    void Awake()
    {
        Instance = this;

    }

    public Currency Get(string id) => _currencies[id];
    public double AmountOf(string id) => _currencies[id].Amount;

    public void Add(string id, double amount)
    {
        _currencies[id].Amount += amount;
        OnCurrencyChanged?.Invoke(id);
    }

    public bool TrySpend(string id, double cost)
    {
        if (_currencies[id].amount < cost) return false;
        _currencies[id].Amount -= cost;
        OnCurrencyChanged?.Invoke(id);
        return true;
    }

    public event System.Action<string> OnCurrencyChanged;
    }
}