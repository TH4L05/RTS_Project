

using UnityEditor;
using UnityEngine;
using UnitEditor.Toolbar;
using UnitEditor.Data;
using Object = UnityEngine.Object;
using System.Collections.Generic;

namespace UnitEditor.List
{
    public class ButtonList : Object
    {
        private Vector2 scrollPosition = Vector2.zero;
        private Vector2 scrollPositionLeft = Vector2.zero;
        private UnitEditorWindow window;
        private string[] unitNames;
        private DataHandler dataHandler;

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
            UnitEditorToolbar.ToolbarIndexChanged -= LoadList;
        }

        public void OnGUI()
        {
            GUILayout.BeginArea(new Rect(15f, 50f, 225f, window.position.height - 50f));
            if (unitNames.Length == 0)
            {
                GUILayout.Label("No Unit created");
            }
            else
            {
                GUILayout.Label("PartLists");
                EditorGUILayout.Space(5f);
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                for (int i = 0; i < unitNames.Length; i++)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(unitNames[i], GUILayout.Width(175f), GUILayout.Height(25f)))
                    {
                        /*if (editor != null) DestroyImmediate(editor);
                        index = i;
                        CreateEditor();*/
                    }
                    if (GUILayout.Button("X", GUILayout.Width(20f), GUILayout.Height(25f)))
                    {
                        /*index = i;
                        DeleteList();*/
                    }
                    GUILayout.EndHorizontal();
                    //EditorGUILayout.Space(2f);
                }

                EditorGUILayout.EndScrollView();
            }

            GUILayout.EndArea();
            EditorGUILayout.Separator();
        }

        private void LoadList(int index)
        {            
            LoadDataNames((UnitType)index);
        }

        private void LoadDataNames(UnitType type)
        {
            List<GameObject> units = new List<GameObject>();

            switch (type)
            {
                case UnitType.Undefined:
                default:
                    return;


                case UnitType.Building:
                    units = dataHandler.UnitEditorData.buildings;
                    unitNames = new string[units.Count];
                    break;


                case UnitType.Character:
                    units = dataHandler.UnitEditorData.characters;
                    unitNames = new string[units.Count];
                    break;

            }
        
            int index = 0;
            foreach (var unit in units)
            {
                unitNames[index] = unit.GetComponent<Unit>().name;
                index++;
            }

        }
    }
}

