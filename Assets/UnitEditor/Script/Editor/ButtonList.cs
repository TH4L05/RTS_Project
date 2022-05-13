/// <author> Thomas Krahl </author>

using System;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

using UnitEditor.Toolbar;
using UnitEditor.Data;

namespace UnitEditor.UI
{
    public class ButtonList : Object
    {
        public static Action<int, UnitType> OnButtonPressed;
        public static Action<int, UnitType> UnitDeletion;
        public Action ResetScrollPosition;

        private UnitEditorWindow editorwindow;
        private Vector2 scrollPosition = Vector2.zero;
        private string[] unitNames;
        private DataHandler dataHandler;
        private Editor editor;
        private int index;
        private UnitType type;
        private GUISkin mySkin;

        public ButtonList(UnitEditorWindow window, DataHandler dataHandler)
        {
            editorwindow = window;
            this.dataHandler = dataHandler;         
            Intitialize();
        }

        #region Intitialize

        public void Intitialize()
        {
            unitNames = new string[0];
            UnitEditorToolbar.ToolbarIndexChanged += LoadList;
            NewUnitWindow.NewUnitCreated += ReloadList;

            LoadList(0);
            var path = DataHandler.GetEditorDataPath();
            LoadSkin(path + "/Data/UnitDataSkin.guiskin");
        }
        private void LoadSkin(string path)
        {
            mySkin = AssetDatabase.LoadAssetAtPath<GUISkin>(path);
        }

        #endregion

        #region Destroy

        public void Destroy()
        {
            if(editor != null) DestroyImmediate(editor);
            UnitEditorToolbar.ToolbarIndexChanged -= LoadList;
            NewUnitWindow.NewUnitCreated -= ReloadList;
        }

        #endregion

        #region GUI

        public void OnGUI()
        {
            if (unitNames.Length == 0)
            {
                OnButtonPressed?.Invoke(-1, type);
            }
            else
            {
                GUILayout.BeginVertical();
                for (int i = 0; i < unitNames.Length; i++)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(unitNames[i], mySkin.customStyles[10]))
                    {
                        index = i;
                        OnButtonPressed?.Invoke(index, type);
                        ResetScrollPosition?.Invoke();
                    }
                    if (GUILayout.Button("X", GUILayout.Width(20f), GUILayout.Height(25f)))
                    {
                        DeleteUnit(i);
                    }

                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }                      
        }

        private void LoadList(int index)
        {            
            type = (UnitType)index;

            if (type == UnitType.Undefined) return;
            LoadDataNames(type);
        }


        private void LoadDataNames(UnitType type)
        {
            if (type == UnitType.Undefined) return;

            List<GameObject> units = new List<GameObject>();
            units = dataHandler.GetList(type);
            if(units.Count == 0) return;

            unitNames = new string[units.Count];

            int index = 0;
            foreach (var unit in units)
            {
                unitNames[index] = unit.GetComponent<Unit>().name;
                index++;
            }

        }

        private void ReloadList()
        {
            if (editor != null) DestroyImmediate(editor);
            LoadDataNames(type);
            OnButtonPressed?.Invoke(index, type);
            editorwindow.Repaint();
        }

        private void DeleteUnit(int indx)
        {
            if (type == UnitType.Undefined) return;

            UnitDeletion?.Invoke(indx, type);
            dataHandler.DeleteUnit(type, indx);

            this.index = 0;
            ReloadList();
        }

        #endregion
    }
}

