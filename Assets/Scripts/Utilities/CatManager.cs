//THIS WILL BE THE SCRIPT TO MANAGE THE CATS
using UnityEngine;
using Assets.Scripts.Utilities;
using System.Text.Json.Nodes;
using System.Collections.Generic;
using UnityEngine.Rendering;
using TMPro;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class CatManager : DataManager<Cat>
    {
        public static CatManager Instance { get; private set; }
        public int Quantity { get; set; } = 0;

        [SerializeField] GameObject CatPrefab;
        [SerializeField] Transform SpawnPoint;
        [SerializeField] Transform SpaceRockMineSpot;
        
        void Awake() => Instance = this;

        public void InitializeCatManager(JsonObject Data)
        {
            int quantity = (int)(Data?["Cats"]?["Quantity"] ?? 0);

            var cats = new List<Cat>();
            for (int i = 0; i < quantity; i++, this.Quantity = i)
                cats.Add(new Cat { Id = $"Cat_{i}"});

            BuildFromList(cats);
        }

        public void SpawnCat()
        {
            var cat = new Cat { Id = $"Cat_{this.Quantity}"};
            _items[cat.Id] = cat;
            this.Quantity++;
            RaiseChanged(cat.Id);
        }

        public string GetIdleCatId()
        {
            foreach (var cat in _items.Values)
            {
                if (cat.Task == "Idle")
                    return cat.Id;
            }

            return null;
        }

        public void AssignToMine(string? catId)
        {
            if (catId != null)
            {
                _items[catId].Task = "Mining";
            }
            else
            {
                var idleCatId = GetIdleCatId();
                if (idleCatId != null)
                    _items[idleCatId].Task = "Mining";
            }
        }

        public void AssignToHaul(string? catId)
        {
            if (catId != null)
            {
                _items[catId].Task = "Hauling";
            }
            else
            {
                var idleCatId = GetIdleCatId();
                if (idleCatId != null)
                    _items[idleCatId].Task = "Hauling";
            }
        }

        public void AssignToSmelt(string catId)
        {
            if (catId != null)
            {
                _items[catId].Task = "Smelting";
            }
            else
            {
                var idleCatId = GetIdleCatId();
                if (idleCatId != null)
                    _items[idleCatId].Task = "Smelting";
            }
        }

        public JsonNode Save() => ToSave();
    }