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

        private Vector2 scrollPosition = Vector2.zero;
        private UnitEditorWindow window;
        private string[] unitNames;
        private DataHandler dataHandler;
        private Editor editor;
        private int index;
        private UnitType type;
        private GUISkin mySkin;

        public ButtonList(UnitEditorWindow window, DataHandler dataHandler)
        {
            this.window = window;
            this.dataHandler = dataHandler;         
            Setup();
        }

        public void Setup()
        {
            unitNames = new string[0];
            LoadList(0);
            UnitEditorToolbar.ToolbarIndexChanged += LoadList;

            var path = DataHandler.GetEditorDataPath();
            LoadSkin(path + "/Data/UnitDataSkin.guiskin");
        }

        public void Destroy()
        {
            if(editor != null) DestroyImmediate(editor);
            UnitEditorToolbar.ToolbarIndexChanged -= LoadList;
        }

        private void LoadSkin(string path)
        {
            mySkin = AssetDatabase.LoadAssetAtPath<GUISkin>(path);
        }

        public void OnGUI()
        {
            if (unitNames.Length == 0)
            {
                GUILayout.Label("No Units created");
            }
            else
            {
                Rect rect = new Rect(10f, 15f, 250f, 28f);

                for (int i = 0; i < unitNames.Length; i++)
                {
                    GUILayout.BeginArea(rect);
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(unitNames[i], mySkin.customStyles[10]))
                    {
                        index = i;
                        OnButtonPressed(index, type);
                      
                    }
                    if (GUILayout.Button("X", GUILayout.Width(20f), GUILayout.Height(25f)))
                    {
                        //index = i;
                        //DeleteList();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.EndArea();

                    rect.y += rect.height;
                }
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
    }
}

