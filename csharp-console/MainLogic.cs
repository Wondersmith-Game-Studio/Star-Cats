using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using IncrementalGame.Utilities;

#if UNITY_5_3_OR_NEWER
using UnityEngine;
#endif

//Declare the namespace for easy inter-file calling
namespace IncrementalGame.Mechanical
{
    class MainLogic
    {
        static void Main()
        {
            var saveData = "SaveData.json";
            var myCurrencies = SaveManager.load(saveData);

            foreach (var item in myCurrencies)
            {
            Console.WriteLine($"{item.Key}: {item.Value}");
            }
        }
    }
}