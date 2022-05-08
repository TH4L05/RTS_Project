

using UnityEditor;
using UnityEngine;
using UnitEditor.Toolbar;
using UnitEditor.Data;
using Object = UnityEngine.Object;
using System.Collections.Generic;
using System;

namespace UnitEditor.UnitEditorGUI
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
        }

        public void Destroy()
        {
            if(editor != null) DestroyImmediate(editor);
            UnitEditorToolbar.ToolbarIndexChanged -= LoadList;
        }

        public void OnGUI()
        {
            GenericMenu menu = new GenericMenu();

            //Rect rect = new Rect(10f, 5f, 275f, window.position.height - 100f);
            //GUILayout.BeginArea(rect);

            if (unitNames.Length == 0)
            {
                GUILayout.Label("No Units created");
            }
            else
            {            
                //EditorGUILayout.Space(1f);
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(window.position.height - 25f), GUILayout.Width(290f));

                for (int i = 0; i < unitNames.Length; i++)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(unitNames[i], GUILayout.Width(175f), GUILayout.Height(25f)))
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
                    //EditorGUILayout.Space(2f);
                }

                EditorGUILayout.EndScrollView();
            }
            //GUILayout.EndArea();          
        }

        private void LoadList(int index)
        {            
            LoadDataNames((UnitType)index);
        }

        private void LoadDataNames(UnitType type)
        {
            Debug.Log(type);
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

