/// <author> Thomas Krahl </author>

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace UnitEditor.Data
{
    public class DataHandler
    {
        #region PrivateFields

        private static string editorDataPath;
        private UnitEditorData data;
        private Dictionary<string, List<GameObject>> unitTypeMasterList;

        #endregion

        #region PublicFields

        //public UnitEditorData UnitEditorData => data;

        #endregion

        #region Setup

        public bool Setup()
        {
            bool setupResult = CheckEditorPath();

            if (!setupResult)
            {
                return false;
            }

            setupResult = LoadEditorData();

            if (!setupResult)
            {
                return false;
            }

            CheckResourceFolder();
            CheckUnitsFolder();
            CreateUnitLists();
            return true;
        }

        private bool CheckEditorPath()
        {
            string[] path = AssetDatabase.FindAssets("UnitEditor");
            editorDataPath = AssetDatabase.GUIDToAssetPath(path[0]);

            if (string.IsNullOrEmpty(editorDataPath))
            {
                Debug.LogError("Could not find DataPath");
                return false;
            }
            return true;
        }

        public bool LoadEditorData()
        {
            data = (UnitEditorData)LoadAsset(typeof(UnitEditorData), editorDataPath + "/Data/UnitEditorData.asset");

            if (data == null)
            {
                Debug.LogError("Could not load EditorData");
                return false;
            }
            return true;
        }

        #endregion

        #region Folder

        private void CreateFolder(string parentFolder, string folderName)
        {
            AssetDatabase.CreateFolder(parentFolder, folderName);
            Debug.Log($"<color=cyan>Created new Folder at : {parentFolder} with name : {folderName}</color>");
        }

        public bool CheckIfFolderExists(string path)
        {
            return AssetDatabase.IsValidFolder(path);
        }

        public void CheckResourceFolder()
        {
            bool folderExists = CheckIfFolderExists(data.resourcesPath);
            if (folderExists) return;
            CreateFolder("Assets", "Resources");
        }

        public void CheckUnitsFolder()
        {
            bool folderExists = CheckIfFolderExists(data.resourcesPath + "/Resources/" + data.unitsRootFolderName);
            if (folderExists) return;
            CreateFolder(data.resourcesPath, data.unitsRootFolderName);
        }

        public bool CheckUnitTypeFolder(string type)
        {
            string path = data.resourcesPath + "/Resources/" + data.unitsRootFolderName + "/";
            bool folderExists = CheckIfFolderExists(path + type);
            if (!folderExists)
            {
                CreateFolder(path, type);
                return false;
            }
            return true;
        }

        #endregion

        #region Lists

        private void CreateUnitLists()
        {
            unitTypeMasterList = new Dictionary<string, List<GameObject>>();

            foreach (var unitType in Enum.GetValues(typeof(UnitType)))
            {
                if (unitType.ToString() == "Undefined") continue;               
                if (!CheckUnitTypeFolder(unitType.ToString())) continue;

                string listName = unitType.ToString();
                List<GameObject> list = new List<GameObject>();

                string path = data.unitsRootFolderName + "/" + unitType.ToString();            
                UnityEngine.Object[] objects = LoadAllFromResources(path);

                foreach (var obj in objects)
                {
                    if (obj is GameObject)
                    {
                        list.Add((GameObject)obj);
                    }
                }
                unitTypeMasterList.Add(listName, list);
            }
        }

        public List<GameObject> GetList(UnitType type)
        {
            List<GameObject> list;
            unitTypeMasterList.TryGetValue(type.ToString(), out list);
            return list;
        }

        public GameObject GetObjectFromList(UnitType type, int listIndex)
        {
            List<GameObject> list = GetList(type);
            if (listIndex < 0 || listIndex > list.Count || list == null) return null;
            return list[listIndex];
        }

        #endregion

        public UnityEngine.Object[] LoadAllFromResources(string path)
        {          
            var loadedObjects = Resources.LoadAll(path);
            return loadedObjects;
        }

        public object LoadAsset(Type type, string path)
        {
            var loadedObj = AssetDatabase.LoadAssetAtPath(path, type);
            //Debug.Log(loadedObj);
            return loadedObj;
        }

        public static string GetEditorDataPath()
        {
            return editorDataPath;
        }
    }
}

