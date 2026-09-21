//THIS WILL BE THE SCRIPT TO MANAGE THE TASKS
using UnityEngine;
using Assets.Scripts.Utilities;
using System.Text.Json.Nodes;
using System.Collections.Generic;

public class TaskManager : DataManager<CatTask>
    {
        public static TaskManager Instance { get; private set; }
        
        [SerializeField] Transform SpaceRockMineSpot;
        
        void Awake() => Instance = this;

        public void InitializeTaskManager(JsonObject Data)
        {
            BuildFromSave(Data?["Tasks"]);
        }

        public void CreateMineTask()
        {

        }

        public JsonNode Save() => ToSave();
    }