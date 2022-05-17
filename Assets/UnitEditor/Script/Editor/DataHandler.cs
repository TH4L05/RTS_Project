/// <author> Thomas Krahl </author>

using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Object;
using UnityEngine.AI;
using UnityEditor;
using System.IO;

namespace UnitEditor.Data
{
    public class DataHandler
    {
        #region PrivateFields

        private static string editorDataPath;
        private UnitEditorData editorData;
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
            editorData = (UnitEditorData)LoadAsset(typeof(UnitEditorData), editorDataPath + "/Data/UnitEditorData.asset");

            if (editorData == null)
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
            bool folderExists = CheckIfFolderExists(editorData.resourcesPath);
            if (folderExists) return;
            CreateFolder("Assets", "Resources");
        }

        public void CheckUnitsFolder()
        {
            bool folderExists = CheckIfFolderExists(editorData.resourcesPath + "/Resources/" + editorData.unitsRootFolderName);
            if (folderExists) return;
            CreateFolder(editorData.resourcesPath, editorData.unitsRootFolderName);
        }

        public bool CheckUnitTypeFolder(string type)
        {
            string path = editorData.resourcesPath + "/Resources/" + editorData.unitsRootFolderName + "/";
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

                string path = editorData.unitsRootFolderName + "/" + unitType.ToString();            
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

        private void DeleteFromList(UnitType type, GameObject obj)
        {
            List<GameObject> list = GetList(type);
            list.Remove(obj);          
        }

        #endregion

        #region Other

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

        #endregion

        #region CreateUnit

        public void CreateNewUnit(UnitType type, string name)
        {
            string path = editorData.resourcesPath + "Resources/" + editorData.unitsRootFolderName + "/" + type.ToString() +"/";
            var unitData = CreateUnitData(type, name, path);
            CreateUnitObject(type, name, path, unitData);
        }

        private UnitData CreateUnitData(UnitType type, string name, string path)
        {
            UnitData unitData = null;
            //Debug.Log(path);

            switch (type)
            {
                case UnitType.Undefined:
                default:
                    return unitData;


                case UnitType.Building:
                    unitData = ScriptableObject.CreateInstance<BuildingData>();
                    break;


                case UnitType.Character:
                    unitData = ScriptableObject.CreateInstance<CharacterData>();
                    break;
            }

            unitData.name = name;
            unitData.SetTypeAndName(type, name);
            AssetDatabase.CreateAsset(unitData, path + "Data/" + name + ".asset");
            return unitData;
        }

        private void CreateUnitObject(UnitType type, string name, string path, UnitData data)
        {
            GameObject newUnitObject = null;
            Unit unit = null;

            switch (type)
            {
                case UnitType.Undefined:
                default:
                    return;


                case UnitType.Building:
                    if (editorData.buildingTemplate == null)
                    {
                        Debug.LogError(type.ToString() + " Template is missing !! - check editorData");
                        return;
                    }
                    newUnitObject = Instantiate(editorData.buildingTemplate);
                    unit = newUnitObject.GetComponent<Building>();
                    unit.SetUnitData(data);

                    // old creation without Prefab
                    //unit = go.AddComponent<Character>();
                    //go.AddComponent<NavMeshAgent>();
                    //go.AddComponent<CapsuleCollider>();               
                    break;


                case UnitType.Character:
                    if (editorData.characterTemplate == null)
                    {
                        Debug.LogError(type.ToString() + " Template is missing !! - check editorData");
                        return;
                    }
                    newUnitObject = Instantiate(editorData.characterTemplate);
                    unit = newUnitObject.GetComponent<Character>();
                    unit.SetUnitData(data);              
                    break;
            }

            newUnitObject.name = name;
            newUnitObject.layer = LayerMask.NameToLayer("Unit");
            newUnitObject.tag = type.ToString();

            SaveAsPrefabAsset(newUnitObject, path);

            List<GameObject> list = GetList(type);
            list.Add(newUnitObject);
            list.Sort((x, y) => string.Compare(x.name, y.name));          
        }

        private void SaveAsPrefabAsset(GameObject obj, string path)
        {
            PrefabUtility.SaveAsPrefabAsset(obj, path + obj.name + ".prefab");
        }

        #endregion

        #region DeleteUnit

        public void DeleteUnit(UnitType type, int index)
        {
            var obj = GetObjectFromList(type, index);
            string name = obj.name;
            string path = editorData.resourcesPath + "Resources/" + editorData.unitsRootFolderName + "/" + type.ToString() + "/";

            DeleteUnitData(path, name);
            DeleteUnitObject(path, name);

            DeleteFromList(type, obj);
        }

        private void DeleteUnitData(string path, string name)
        {
            bool deleteSuccess = false;
            string unitDataPath = path + "Data/" + name + ".asset";
            deleteSuccess = AssetDatabase.DeleteAsset(unitDataPath);

            if (deleteSuccess)
            {
                Debug.Log($"<color=#43F92A>{unitDataPath} deleted</color>");
            }
            else
            {
                Debug.LogError($"<color=red>ERROR !! - Could not delete {unitDataPath}</color>");
            }
        }

        private void DeleteUnitObject(string path, string name)
        {
            bool deleteSuccess = false;
            string unitObjectPath = path + name + ".prefab";
            deleteSuccess = AssetDatabase.DeleteAsset(unitObjectPath);

            if (deleteSuccess)
            {
                Debug.Log($"<color=#43F92A>{unitObjectPath} deleted</color>");
            }
            else
            {
                Debug.LogError($"<color=red>ERROR !! - Could not delete {unitObjectPath}</color>");
            }
        }

        public string[] LoadLinesFromCSV(string path)
        {
            string filePath = path;

            if (File.Exists(filePath))
            {
                return File.ReadAllLines(path);
            }
            else
            {
                Debug.LogError($"csv at path \"{path}\" does not exist");
            }

            return null;          
        }

        #endregion
    }
}

