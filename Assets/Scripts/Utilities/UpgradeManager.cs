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
    }