

using UnityEngine;
using UnityEditor;
using System;

namespace UnitEditor.Data
{
    public class DataHandler
    {
        #region PrivateFields

        private string dataPath;
        private UnitEditorData data;

        #endregion

        #region PublicFields

        public UnitEditorData UnitEditorData => data;

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
            return true;
        }

        private bool CheckEditorPath()
        {
            string[] path = AssetDatabase.FindAssets("UnitEditor");
            dataPath = AssetDatabase.GUIDToAssetPath(path[0]);

            if (string.IsNullOrEmpty(dataPath))
            {
                Debug.LogError("Could not find DataPath");
                return false;
            }
            return true;
        }

        public bool LoadEditorData()
        {
            data = (UnitEditorData)LoadAsset(typeof(UnitEditorData), dataPath + "/Data/UnitEditorData.asset");

            //data = AssetDatabase.LoadAssetAtPath<UnitEditorData>(dataPath + "/UnitEditorData.asset");

            if (data == null)
            {
                Debug.LogError("Could not load EditorData");
                return false;
            }
            return true;
        }

        #endregion

        public object LoadAsset(Type type, string path)
        {
            var loadedObj = AssetDatabase.LoadAssetAtPath(path, type);
            //Debug.Log(loadedObj);
            return loadedObj;
        }
    }
}

