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

    void Start()
    {
        //BUILDFROMSAVE CONSTRUCTS CURRENECY OBJECTS
        //CURRENCIES IS CURRENCIES DICT USED THROUGHOUT GAME
        _currencies= BuildFromSave(Data);
            if (Data = null)
                return;
    }

    //BUILDFROMSAVE METHOD CONSTRUCTS DICT FROM DATA JSONOBJECT
    //--SEE SAVE MANAGER FOR DATA DEFINITION
    /////////////////////////////////////////////////////////////////////////////
    private Dictionary<string, Currency> BuildFromSave(JsonObject data);
    {
        var byId = new Dictionary<string, Currency>();
        if (data == null) return byId;
        foreach (Currency c in data.Deserialize<SaveData>().currencies)
            byId[c.Id] = c;
        return byId
    }
    /////////////////////////////////////////////////////////////////////////////
    
    //SAVE METHOD FOR TRANSFORMING _CURRENCIES DICT TO JSON OBJECT
    //TO THEN HAND CURRENCIES JSON OBJECT TO SAVE MANAGER
    /////////////////////////////////////////////////////////////////////////////
    public JsonObject ToSave()
    {
        var save = new SaveData { currencies = _currencies.Values.ToList() };
        currencySaveData =JsonSerializer.SerializeToNode(save).AsObject();
        return currencySaveData
    }
    ////////////////////////////////////////////////////////////////////////////

/   ///////////////////////////////////////////////////////////////////////////
    public static CurrencyManager Instance { get; private set; } 

    void Awake()
    {
        //INSTANTIATE CURRENCYMANAGER OBJECT
        Instance = this;
    }
    ////////////////////////////////////////////////////////////////////////////
    
    public Currency Get(string id) => _currencies[id];
    public double AmountOf(string id) => _currencies[id].Amount;

    //METHOD TO ADD CURRENCY
    /////////////////////////////////////////////////////////////////
    public void Add(string id, double amount)
    {
        _currencies[id].Amount += amount;
        OnCurrencyChanged?.Invoke(id);
    }

    //METHOD TO SPEND CURRENCY
    /////////////////////////////////////////////////////////////////
    public bool TrySpend(string id, double cost)
    {
        if (_currencies[id].amount < cost) return false;
        _currencies[id].Amount -= cost;
        OnCurrencyChanged?.Invoke(id);
        return true;
    }
    ////////////////////////////////////////////////////////////////////////////
    
    //TRIGGER PUBLIC EVENT ONCURRENCYCHANGED FOR DATA POPULATION VS POLLING
    //---LESS HARDWARE RESOURCE USAGE
    //////////////////////////////////////////////////////////////////
    public event System.Action<string> OnCurrencyChanged;
    }
}