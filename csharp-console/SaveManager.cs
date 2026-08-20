////////////////////////////SAVEMANAGER.CS///////////////////////////////


/////////////////////////////////////////////////////////////////////////
//Import Resources
/////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text.Json; // Standard .NET

//ONLY PERFROM IF COMPILING INSIDE UNITY//
//FOR EVERY SECTION THIS SNIPPET EXISTS

#if UNITY_5_3_OR_NEWER
    using UnityEngine;
#endif

/////////////////////////////////////////////////////////////////////////
//Declare the namespace for easy inter-file calling
/////////////////////////////////////////////////////////////////////////

namespace IncrementalGame.Utilities
{

    ////////////////////////////////////////////////////////////////////
    //CLASS DEFINITIONS
    ////////////////////////////////////////////////////////////////////

    //CURRENCY CLASS FOR CURRENCIES

    [System.Serializable]
    public class Currency
    {
        public string id = string.Empty;
        public double amount;
    }

    //GAMESAVEDATA CLASS FOR DATA TRANSFER OBJECT (DTO)

    [System.Serializable]
    public class GameSaveData
    {
        public List<Currency> currencies;

        public GameSaveData()
        {
            currencies = new List<Currency>();
        }
    }

    //SAVEMANAGER STATIC CLASS FOR SAVE LOGIC
    public static class SaveManager
    {

        //Get Save Path Helper
        public static string GetSavePath(string fileName)
        {
            //PATH VARIABLE DEFINITION//
            /////////////////////////////////////////////////////////////////////////////////////////
            //PATH = WHEREVER THIS PROCESS IS RUNNING FROM + FILENAME
            string path;

            #if UNITY_5_3_OR_NEWER
                //UNITY USES APPLICATION.PERSISTENTDATAPATH FOR CURRENT DIRECTORY
                path = Path.Combine(Application.persistentDataPath, fileName);
            #else
                //IDE USES DIRECTORY.GETCURRENT DIRECTORY FOR CURRENT DIRECTORY
                path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            #endif
            
            /////////////////////////////////////////////////////////////////////////////////////////
            //DIR VARIABLE DEFINITION
            var dir = Path.GetDirectoryName(path);

            //IF DIR RETURNS EMPTY, CREATE DIRECTORY
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);
            /////////////////////////////////////////////////////////////////////////////////////////

            return path;  

            //END PATH VARIABLE DEFINITION//
            /////////////////////////////////////////////////////////////////////////////////////////
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        public static string GetTemplatePath(string fileName)
        {
            #if UNITY_5_3_OR_NEWER
                return Path.Combine(Application.streamingAssetsPath, fileName);
            #else
                return Path.Combine(Directory.GetCurrentDirectory(), fileName);
            #endif
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        public static void CreateNewSave(string selectedFile, Action<bool> onComplete = null, MonoBehaviour runner = null)
        {
            string destinationPath = GetSavePath(selectedFile);

            #if UNITY_ANDROID && !UNITY_EDITOR
                if (runner == null)
                {
                    Debug.LogError("CreateNewSave on Android requires a MonoBehaviour to run the coroutine.");
                    onComplete?.Invoke(false);
                    return;
                }
                runner.StartCoroutine(CreateNewSaveAndroid(destinationPath, onComplete));
            #else
                try
                {
                    string sourcePath = GetTemplatePath("SaveData.json");
                    File.Copy(sourcePath, destinationPath, overwrite: true);
                    onComplete?.Invoke(true);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"CreateNewSave failed: {ex.Message}");
                    onComplete?.Invoke(false);
                }
            #endif
        }

        #if UNITY_ANDROID && !UNITY_EDITOR
            private static IEnumerator CreateNewSaveAndroid(string destinationPath, Action<bool> onComplete)
            {
                string templatePath = GetTemplatePath("SaveData.json");

                using (UnityWebRequest request = UnityWebRequest.Get(templatePath))
                {
                    yield return request.SendWebRequest();

                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        File.WriteAllText(destinationPath, request.downloadHandler.text);
                        onComplete?.Invoke(true);
                    }
                    else
                    {
                        Debug.LogError($"Failed to read save template: {request.error}");
                        onComplete?.Invoke(false);
                    }
                }
            }
        #endif
    

        //SAVE METHOD FOR SERIALIZING AND SAVING DATA FROM SESSION STORAGE TO PERSISTENT STORAGE

        public static void Save(string fileName, Dictionary<string, double> currencyDict)
        {
            //CREATE DATA TRANSFER OBJECT "DATA"

            GameSaveData data = new GameSaveData();

            //FOR EACH KEY-VALUE PAIR (KVP) IN THE CURRENCY DICTIONARY

            foreach (var kvp in currencyDict)
            {
                //DATA OBJECT, CURRENCIES PROPERTY
                //ADD NEW CURRENCY OBJECT, SET THE ID = KVP KEY, AND THE AMOUNT = KVP VALUE
                data.currencies.Add(new Currency { id = kvp.Key, amount = kvp.Value });
            }

            //START JSON VARIABLE DEFINITION//
            string json;

            #if UNITY_5_3_OR_NEWER
                json = JsonUtility.ToJson(data, true);
            #else
                var options = new JsonSerializerOptions { WriteIndented = true, IncludeFields = true };
                json = JsonSerializer.Serialize(data, options);
            #endif
            //END JSON VARIABLE DEFINITION//

            //USE SAVE FILE HELPER TO GET PATH
            string path = GetSavePath(fileName);

            //WRITE JSON STRING TO DETERMINED PATH, WRITE USER FEEDBACK
            File.WriteAllText(path, json);
            Console.WriteLine($"Saved to: {path}");
        }

        //LOAD METHOD FOR DESERIALIZING DATA FROM PERSISTENT STORAGE TO SESSION STORAGE

        public static Dictionary<string, double> load(string fileName)
        {
            string path = GetSavePath(fileName);

            //JSON VARIABLE DEFINITION
            //CONSISTS OF THE CONTENTS OF THE SAVEDATA FILE
            string json = File.ReadAllText(path);
            //CREATES DATA OBJECT USING GAMESAVEDATA CLASS AND FOLLOWING LOC
            GameSaveData? data;

            //BEGIN DATA DEFINITION
            #if UNITY_5_3_OR_NEWER
                //DATA DTO = GAMESAVEDATA CLASS
                //FROMJSON DESERIALIZES INTO DTO OBJECT "DATA" USING JSON VAR
                data = JsonUtility.FromJson<GameSaveData>(json);
            #else
                //OPTIONS NECESSARY FOR JSONSERIALIZEROPTIONS, SINCE WE USE FIELDS
                var options = new JsonSerializerOptions { IncludeFields = true };
                //DTO CREATED BY DESERIALIZING USING GAMESAVEDATA
                data = JsonSerializer.Deserialize<GameSaveData>(json, options);
            #endif

            //IF RETURNS NULL, CREATE NEW DICT
            if (data == null || data.currencies == null)
                return new Dictionary<string, double>();

            //CURRENCYDICT DICTIONARY CREATED BY DESERIALIZING FROM DTO
            var currencyDict = new Dictionary<string, double>();

////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////
            //ITERATES THROUGH EACH ENTRY
            foreach (var currency in data.currencies)
            {
                //CURRENCY.ID IS KEY, CURRENCY.AMOUNT IS VALUE
                currencyDict[currency.id] = currency.amount;
            }

            //RETURNS THE NEW CURRENCY DICTIONARY
            return currencyDict;
        }
    }
}