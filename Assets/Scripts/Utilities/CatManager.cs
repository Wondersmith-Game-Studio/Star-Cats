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
            for (int i = this.Quantity; i < quantity; this.Quantity++)
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

        public void AssignCatTask(string catId)
        {
            
        }

        public void AssignToHaul(string catId)
        {

        }

        public void AssignToSmelt(string catId)
        {

        }



        public JsonNode Save() => ToSave();
    }