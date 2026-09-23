//THIS WILL BE THE SCRIPT TO MANAGE THE CATS
using UnityEngine;
using Assets.Scripts.Utilities;
using System.Text.Json.Nodes;
using System.Collections.Generic;
using UnityEngine.Rendering;
using TMPro;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class UpgradeManager : DataManager<Upgrade>
{
    public static UpgradeManager Instance { get; private set; }
        
    void Awake() => Instance = this;

    public void InitializeUpgradeManager(JsonObject Data)
    {
        BuildFromSave(Data?["Upgrades"]);
    }

    public void PurchaseUpgrade(Upgrade upgrade)
    {
        foreach (var cost in upgrade.Costs)
            if (CurrencyManager.Instance.AmountOf(cost.CurrencyId) < cost.Amount)
                return; // can't afford - bail before spending anything

        foreach (var cost in upgrade.Costs)
            CurrencyManager.Instance.TrySpend(cost.CurrencyId, cost.Amount);

        upgrade.Acquired = true;
    }

    public JsonNode Save() => ToSave();
}