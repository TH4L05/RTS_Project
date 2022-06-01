/// <author> Thomas Krahl </author>

using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using static UnityEngine.Object;
using UnityEditor;

namespace UnitEditor.Data
{
    public sealed class DataHandler
    {
        #region PrivateFields

        private string editorDataPath;
        private UnitEditorData editorData;
        private Dictionary<string, List<GameObject>> unitTypeMasterList;
        private GameObject activeObj;
        private GUISkin mySkin;
        private Texture2D[] iconTextures;

        #endregion

        #region PublicFields

        public string EditorDataPath => editorDataPath;
        public UnitEditorData EditorData => editorData;
        public GameObject ActiveObj => activeObj;
        public GUISkin MySkin => mySkin; 
        public Texture2D[] IconTextures => iconTextures;

        #endregion

        public static readonly DataHandler Instance = new DataHandler();

        private DataHandler()
        {          
        }

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
            LoadSkin();
            LoadTextures();
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

        private bool LoadEditorData()
        {
            editorData = (UnitEditorData)LoadAsset(typeof(UnitEditorData), editorDataPath + "/Data/UnitEditorData.asset");

            if (editorData == null)
            {
                Debug.LogError("Could not load EditorData");
                return false;
            }
            return true;
        }

        private void LoadSkin()
        {
            mySkin = AssetDatabase.LoadAssetAtPath<GUISkin>(editorDataPath + "/Data/UnitDataSkin.guiskin");
        }

        private void LoadTextures()
        {
            iconTextures = new Texture2D[10];

            Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(editorDataPath + "/Data/Texture/IconNoIcon.png");
            if (tex == null) tex = new Texture2D(128, 128);
            iconTextures[0] = tex;

            tex = AssetDatabase.LoadAssetAtPath<Texture2D>(editorDataPath + "/Data/Texture/iconHealth.png");
            if (tex == null) tex = iconTextures[0];
            iconTextures[1] = tex;

            tex = AssetDatabase.LoadAssetAtPath<Texture2D>(editorDataPath + "/Data/Texture/iconMana.png");
            if (tex == null) tex = iconTextures[0];
            iconTextures[2] = tex;

            tex = AssetDatabase.LoadAssetAtPath<Texture2D>(editorDataPath + "/Data/Texture/IconArmor.png");
            if (tex == null) tex = iconTextures[0];
            iconTextures[3] = tex;

            tex = AssetDatabase.LoadAssetAtPath<Texture2D>(editorDataPath + "/Data/Texture/IconWeapon1.png");
            if (tex == null) tex = iconTextures[0];
            iconTextures[4] = tex;

            tex = AssetDatabase.LoadAssetAtPath<Texture2D>(editorDataPath + "/Data/Texture/R_Wood.png");
            if (tex == null) tex = iconTextures[0];
            iconTextures[5] = tex;

            tex = AssetDatabase.LoadAssetAtPath<Texture2D>(editorDataPath + "/Data/Texture/R_Gold.png");
            if (tex == null) tex = iconTextures[0];
            iconTextures[6] = tex;

            tex = AssetDatabase.LoadAssetAtPath<Texture2D>(editorDataPath + "/Data/Texture/R_Food.png");
            if (tex == null) tex = iconTextures[0];
            iconTextures[7] = tex;

            tex = AssetDatabase.LoadAssetAtPath<Texture2D>(editorDataPath + "/Data/Texture/R_Unit.png");
            if (tex == null) tex = iconTextures[0];
            iconTextures[8] = tex;
        }

        #endregion

        #region Folder

        private void CreateFolder(string parentFolder, string folderName)
        {
            AssetDatabase.CreateFolder(parentFolder, folderName);
            Debug.Log($"<color=#ACE8FF>Created new Folder with name : \"{folderName}\" at path: \"{parentFolder}\".</color>");
        }

        private bool CheckIfFolderExists(string path)
        {
            return AssetDatabase.IsValidFolder(path);
        }

        private void CheckResourceFolder()
        {
            bool folderExists = CheckIfFolderExists(editorData.resourcesPath);
            if (folderExists) return;
            CreateFolder("Assets", "Resources");
        }

        private void CheckUnitsFolder()
        {
            string path = editorData.resourcesPath + "/Resources/" + editorData.unitsRootFolderName;
            bool folderExists = CheckIfFolderExists(path);

            if (folderExists) return;
            CreateFolder(editorData.resourcesPath, editorData.unitsRootFolderName);
        }

        private void CheckUnitTypeFolder(string type)
        {
            string path = editorData.resourcesPath + "/Resources/" + editorData.unitsRootFolderName;
            bool folderExists = CheckIfFolderExists(path + "/" + type);

            if (folderExists) return;
            CreateFolder(path, type);
            CreateFolder(path + "/" + type, "Data");
        }

        #endregion

        #region Lists

        private void CreateUnitLists()
        {
            unitTypeMasterList = new Dictionary<string, List<GameObject>>();

            foreach (var unitType in Enum.GetValues(typeof(UnitType)))
            {
                CheckUnitTypeFolder(unitType.ToString());

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
            activeObj = list[listIndex];
            return list[listIndex];
        }

        public bool UnitNameExistanceCheck(string name, UnitType type)
        {
            List<GameObject> list = GetList(type);

            foreach (var item in list)
            {
                if (item.name == name)
                {
                    return true;
                }
            }

            return false;
        }

        private void DeleteFromList(UnitType type, GameObject obj)
        {
            List<GameObject> list = GetList(type);
            list.Remove(obj);          
        }

        #endregion

        #region CreateUnit

        public bool CreateNewUnit(UnitType type, string name)
        {
            string path = editorData.resourcesPath + "Resources/" + editorData.unitsRootFolderName + "/" + type.ToString() +"/";

            var unitData = CreateUnitData(type, name, path);
            if (unitData == null)
            {
                ShowMessage(true, true, name, path + "Data/");
                return false;
            }

            ShowMessage(false, true, name, path + "Data/");

            GameObject unit = CreateUnitObject(type, name, path, unitData);                   
            if (unit == null)
            {
                ShowMessage(true, true, name, path);             
                return false;
            }

            ShowMessage(false, true, name, path);

            List<GameObject> list = GetList(type);
            list.Add(unit);
            list.Sort((x, y) => string.Compare(x.name, y.name));

            return true;         
        }

        private UnitData CreateUnitData(UnitType type, string name, string path)
        {
            UnitData unitData = null;

            switch (type)
            {
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

        private GameObject CreateUnitObject(UnitType type, string name, string path, UnitData data)
        {
            GameObject newUnitObject = null;
            Unit unit = null;

            switch (type)
            {
                default:
                    return null;


                case UnitType.Building:
                    if (editorData.unitTemplates[0] == null)
                    {
                        Debug.LogError(type.ToString() + " Template is missing !! - check editorData");
                        return null;
                    }
                    newUnitObject = Instantiate(editorData.unitTemplates[0]);
                    unit = newUnitObject.GetComponent<Building>();
                    unit.SetUnitData(data);            
                    break;


                case UnitType.Character:
                    if (editorData.unitTemplates[1] == null)
                    {
                        Debug.LogError(type.ToString() + " Template is missing !! - check editorData");
                        return null;
                    }
                    newUnitObject = Instantiate(editorData.unitTemplates[1]);
                    unit = newUnitObject.GetComponent<Character>();
                    unit.SetUnitData(data);              
                    break;
            }

            newUnitObject.name = name;
            newUnitObject.layer = LayerMask.NameToLayer("Unit");
            newUnitObject.tag = type.ToString();

            var prefab = SaveAsPrefabAsset(newUnitObject, path);
            DestroyImmediate(newUnitObject);          
            return prefab;
        }

        private GameObject SaveAsPrefabAsset(GameObject obj, string path)
        {
            return PrefabUtility.SaveAsPrefabAsset(obj, path + obj.name + ".prefab");
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
            string unitDataPath = path + "Data/" + name + ".asset";
            bool deleteSuccess = AssetDatabase.DeleteAsset(unitDataPath);
            ShowMessage(deleteSuccess, false, name, path);
        }

        private void DeleteUnitObject(string path, string name)
        {          
            string unitObjectPath = path + name + ".prefab";
            bool deleteSuccess = AssetDatabase.DeleteAsset(unitObjectPath);
            ShowMessage(deleteSuccess, false, name, path);
        }

        #endregion

        #region

        private void ShowMessage(bool error, bool created, string name, string path)
        {
            string message = string.Empty;

            string mcreated = "has been created";
            string mdeleted = "has been deleted";
            string errorCreate = "ERROR!! - Could not create ";
            string errorDelete = "ERROR!! - Could not delete ";

            if (error)
            {
                message += "<color=#CC0000>";
                if (created)
                {
                    message += errorCreate;
                }
                else
                {
                    message += errorDelete;
                }
                
                message += " object with name: \"" + name + "\"";
                message += "at path: \"" + path + "\"";
                message += ".</color>";
                Debug.LogError(message);
            }
            else
            {
                message += "<color=#43F92A>";
                message += " object with name: \"" + name + "\"";
                message += "at path: \"" + path + "\"";

                if (created)
                {
                    message += mcreated;
                }
                else
                {
                    message += mdeleted;
                }

                message += ".</color>";
                Debug.Log(message);
            }
        }

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

        public string GetEditorDataPath()
        {
            return editorDataPath;
        }

        public void SetActiveObj(GameObject go)
        {
            activeObj = go;
        }

        public string[] LoadLinesFromCSV(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllLines(path);
            }
            else
            {
                Debug.LogError($"csv at path \"{path}\" does not exist");
            }

            return null;
        }

        public void InstantiateActiveObject()
        {
            if (activeObj == null) return;

            Vector3 position = Vector3.zero;
            Ray ray = SceneView.lastActiveSceneView.camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1.0f));

            if (Physics.Raycast(ray, out var hitInfo))
            {
                position = hitInfo.point;
            }

            Instantiate(activeObj, position, Quaternion.identity);
        }

        #endregion
    }
}

