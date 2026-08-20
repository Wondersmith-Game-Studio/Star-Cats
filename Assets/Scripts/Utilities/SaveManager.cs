////////////////////////////SAVEMANAGER.CS///////////////////////////////


/////////////////////////////////////////////////////////////////////////
//Import Resources
/////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Text.Json;
using System.Text.Json.Nodes;
using UnityEngine;


#if UNITY_ANDROID && !UNITY_EDITOR
    using UnityEngine.Networking;
#endif

/////////////////////////////////////////////////////////////////////////
//Declare the namespace for easy inter-file calling
/////////////////////////////////////////////////////////////////////////

namespace Assets.Scripts.Utilities
{

    ////////////////////////////////////////////////////////////////////
    //CLASS DEFINITIONS
    ////////////////////////////////////////////////////////////////////

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
            
            /////////////////////////////////////////////////////////////////
            //DIR VARIABLE DEFINITION
            var dir = Path.GetDirectoryName(path);

            //IF DIR RETURNS EMPTY, CREATE DIRECTORY
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);
            /////////////////////////////////////////////////////////////////////////////////////////

            return path;  

            //END PATH VARIABLE DEFINITION//
            /////////////////////////////////////////////////////////////////
        }

        /////////////////////////////////////////////////////////////////////
        public static string GetTemplatePath(string fileName)
        {
            #if UNITY_5_3_OR_NEWER
                return Path.Combine(Application.streamingAssetsPath, fileName);
            #else
                return Path.Combine(Directory.GetCurrentDirectory(), fileName);
            #endif
        }

        /////////////////////////////////////////////////////////////////////
        public static void CreateNewSave(string selectedFile, Action<bool> onComplete = null
        #if UNITY_ANDROID && !UNITY_EDITOR
        , MonoBehaviour runner = null
        #endif
        )
        {
            string destinationPath = GetSavePath(selectedFile);

            //ANDROID NEEDS SPECIAL PATH HANDLING////////////////////////////////////////////////////
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
                    #if UNITY_5_3_OR_NEWER
                        Debug.LogError($"CreateNewSave failed: {ex.Message}");
                    #else
                        Console.WriteLine($"CreateNewSave failed: {ex.Message}");
                    #endif
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
        /////////////////////////////////////////////////////////////////////

        //SAVE METHOD FOR SERIALIZING AND SAVING DATA FROM SESSION STORAGE TO PERSISTENT STORAGE

        public static void Save(string fileName, JsonObject data)
        {
            //USE SAVE FILE HELPER FOR PATH
            string path = GetSavePath(fileName);

            //START JSON VARIABLE DEFINITION//
            var options = new JsonSerializerOptions {WriteIndented = true };
            string json = data.ToJsonString(options);
            //END JSON VARIABLE DEFINITION//

            //WRITE JSONOBJECT DATA TO SAVEFILE PATH
            File.WriteAllText(path, json);

            #if UNITY_5_3_OR_NEWER
                Debug.Log($"Saved to: {path}");
            #else
                Console.WriteLine($"Saved to: {path}");
            #endif
        }

        //LOAD METHOD FOR DESERIALIZING DATA FROM PERSISTENT STORAGE TO SESSION STORAGE

        public static JsonObject Load(string fileName)
        {
            string path = GetSavePath(fileName);

            //IF NO SAVE FILE EXISTS RETURN EMPTY OBJECT INSTEAD OF ERROR
            if (!File.Exists(path))
                return new JsonObject();

            //JSON VARIABLE DEFINITION
            //CONSISTS OF THE CONTENTS OF THE SAVEDATA FILE
            string json = File.ReadAllText(path);
            
            //PARSE CONTENTS INTO MUTABLE JSONOBJECT
            JsonObject data = JsonNode.Parse(json)?.AsObject() ?? new JsonObject();

            return data;
        }
    }
}