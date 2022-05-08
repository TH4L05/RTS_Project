

using UnityEditor;
using UnityEngine;
using UnitEditor.Toolbar;
using UnitEditor.Data;
using Object = UnityEngine.Object;
using System.Collections.Generic;
using System;

namespace UnitEditor.UnitEditorGUI
{
    public class PropertiesArea : Object
    {
        private UnitEditorWindow window;
        private DataHandler dataHandler;
        private Editor editorUnitData;
        private Editor editorUnitTypeData;
        private Vector2 scrollPosition = Vector2.zero;

        public PropertiesArea(UnitEditorWindow window, DataHandler dataHandler)
        {
            this.window = window;
            this.dataHandler = dataHandler;
            Setup();
        }

        public void Setup()
        {
            ButtonList.OnButtonPressed += CreateEditor;
            UnitEditorToolbar.ToolbarIndexChanged += DestroyEditor;
        }

        public void Destroy()
        {
            ButtonList.OnButtonPressed -= CreateEditor;
            UnitEditorToolbar.ToolbarIndexChanged -= DestroyEditor;
            DestroyEditor();
        }

        public void OnGUI()
        {
            Rect rect = new Rect(25f, 25f, 1500f, 2000f);
            GUILayout.BeginArea(rect);
            if (editorUnitData != null)
            {
                editorUnitData.OnInspectorGUI();
            }
            GUILayout.EndArea();
        }

        /// <summary>
        /// Creates a new Editor based on UnitType
        /// </summary>
        /// <param name="index">Index from buttonList</param>
        /// <param name="type">unitType</param>
        private void CreateEditor(int index, UnitType type)
        {
            DestroyEditor();

            GameObject obj = null;
            Unit unit = null;
            UnitData data = null;

            obj = dataHandler.GetObjectFromList(type, index);
            if (obj == null) return;

            switch (type)
            {
                case UnitType.Undefined:
                default:
                    return;


                case UnitType.Building:
                    unit = obj.GetComponent<Unit>() as Building;
                    data = unit.UnitData as BuildingData;
                    break;


                case UnitType.Character:
                    unit = obj.GetComponent<Unit>() as Character;
                    data = unit.UnitData as CharacterData;
                    break;

            }
          
            if (data != null)
            {
                editorUnitData = Editor.CreateEditor(data);
                EditorUtility.SetDirty(data);
            }
            else
            {
                Debug.LogError("Data Error - could not create Editor");
            }
        }

        private void DestroyEditor()
        {
            if (editorUnitData != null) DestroyImmediate(editorUnitData);
        }

        private void DestroyEditor(int index)
        {
            if (editorUnitData != null) DestroyImmediate(editorUnitData);
        }
    }
}

