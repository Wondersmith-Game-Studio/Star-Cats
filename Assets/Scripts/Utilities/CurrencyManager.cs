using UnityEngine;
using Assets.Scripts.Utilities;
using System.Text.Json.Nodes;

//////////////////////////////////////////////////////////////

public class CurrencyManager : DataManager<Currency>
{
    public static CurrencyManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        BuildFromSave(DataManager.Data?["currencies"]);
    }
    
    ////////////////////////////////////////////////////////////////////////////
    public double AmountOf(string id) => Has(id) ? Get(id).Amount : 0;
    

    //METHOD TO ADD CURRENCY
    /////////////////////////////////////////////////////////////////
    public void Add(string id, double amount)
    {
        if (!_items.TryGetValue(id, out var c)) return;
        c.Amount += amount;
        RaiseChanged(id);
    }

    //METHOD TO SPEND CURRENCY
    /////////////////////////////////////////////////////////////////
    public bool TrySpend(string id, double cost)
    {
        if (!_items.TryGetValue(id, out var c) || c.Amount < cost) return false;
        c.Amount -= cost;
        RaiseChanged(id);
        return true;
    }

    public JsonNode Save()
    {
        return ToSave();
    }
}