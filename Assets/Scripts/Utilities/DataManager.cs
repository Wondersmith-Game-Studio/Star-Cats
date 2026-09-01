using UnityEngine;
using Assets.Scripts.Utilities;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Linq;
using System.Collections.Generic;

/////////////////////////////////////////////////////////////////////
//INTERFACE TO CALL TO ENSURE IT HAS AN ID OR COMPILER THROWS ERROR
public interface IHasId
{
    string Id { get; }
}

/////////////////////////////////////////////////////////////////////
//PUBLIC CURRENCY CLASS, ENSURES THERE'S AN ID
public class Currency: IHasId
{
    public string Id { get; set; }
    public double Amount { get; set; }
    public double Value { get; set; }

    public string Sprite { get; set; }
}

////////////////////////////////////////////////////////////////////
//NON-GENERIC BASE HOLDS THE SHARED, LOADED SAVE BLOB SO IT CAN BE REACHED AS
//DataManager.Data FROM ANY FILE (a static on the generic class would be per-T and
//could not be referenced without a type argument).
public abstract class DataManager : MonoBehaviour
{
    ///INSERTED FROM SAVEMANAGER
    public static JsonObject Data;


    public static string GetString(string key, string fallback = "")
    => Data?[key]?.GetValue<string>() ?? fallback;
    

    public static void SetString(string key, string value)
    {
        Data ??= new JsonObject();
        Data[key] = value;
    }
}

////////////////////////////////////////////////////////////////////
//<T> IS PLACEHOLDER FOR A TYPE, ENSURING THERE IS AN ID ASSOCIATED WITH INSERTED ITEM
public abstract class DataManager<T> : DataManager where T : IHasId
{
    ///////////////////////////////////////////////////////////////////////////////////

    ///READONLY DICTIONARY, GENERIC _ITEMS FOR VARIOUS MANAGER FILES
    protected readonly Dictionary<string, T> _items = new();
    
    ///READONLY PUBLIC DICT OTHER FILES CAN CALL
    public IReadOnlyDictionary<string, T> Items => _items;

    public int Count => _items.Count;

    public event Action<string> OnChanged; //PUBLIC EVENT MANAGERS CAN CALL TO TELL CONTROLLERS TO UPDATE, RATHER THAN POLLING. MORE RESOURCE EFFECIENT.
    
    protected void RaiseChanged(string id)
    {
        OnChanged?.Invoke(id); //TRIGGERS ONCHANGED EVENTS FOR EVERY LINE WITH ONE PER (id)   EG: ONCHANGED("SPACEROCK")
        Debug.Log($"{id} Invoked.");
    }
    //////////////////////////////////////////////////////////////////////////////////
    
    ///TAKES JSON OBJECT AND CREATES LIST "itemsArray" BASED ON INSERTED <T>
    ///EG: ONE FOR CURRENCY, ONE FOR GENERATORS, ONE FOR UPGRADES, ECT.
        protected void BuildFromSave(JsonNode itemsArray)
            {
        //CLEARS LIST, IF ONE EXISTS
        _items.Clear();
        if (itemsArray == null) return;

        //BUILD A LIST FROM ITEM ARRAY <T>, IF THERE IS ONE
        var list = itemsArray.Deserialize<List<T>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        if (list == null) return;
        
        //ITERATE THROUGH LIST, CREATE OBJECT FOR EVERY ENTRY
        foreach (T item in list)
            _items[item.Id] = item;
    }

    ///////////////////////////////////////////////////////////////////////////////
    ///METHOD TO PREPARE RUNTIME DATA FOR SAVEMANAGER. CONVERTS BACK TO JSON OBJECT
    
    protected JsonNode ToSave()
        => JsonSerializer.SerializeToNode(_items.Values.ToList());
    
    ///////////////////////////////////////////////////////////////////////////////
    
    public T Get(string id) => _items.TryGetValue(id, out var v) ? v : default;
    public bool Has(string id) => _items.ContainsKey(id);
}