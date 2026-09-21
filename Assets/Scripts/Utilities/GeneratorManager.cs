using UnityEngine;
using Assets.Scripts.Utilities;
using System.Text.Json.Nodes;

//////////////////////////////////////////////////////////////

public class GeneratorManager : DataManager<Generator>
{
    public static GeneratorManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void InitializeCurrencyManager(JsonObject Data)
    {
        BuildFromSave(Data?["Generators"]);

        Debug.Log("CurrencyManager Initialized (CurrencyManager.InitializeCurrencyManager");
    }
}