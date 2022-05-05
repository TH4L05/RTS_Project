

using UnityEditor;
using UnityEngine;
using UnitEditor.Toolbar;
using UnitEditor.Data;
using Object = UnityEngine.Object;
using System.Collections.Generic;

namespace UnitEditor.UnitEditorGUI
{
    public class PropertiesArea : Object
    {
        private UnitEditorWindow window;
        private DataHandler dataHandler;
        private Editor editorLeft;
        private Editor editorRight;

        private Vector2 scrollPositionLeft = Vector2.zero;
        private Vector2 scrollPositionRight = Vector2.zero;

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
            //Rect rightRect = new Rect(leftRect.position.x + leftRect.size.x + 25f, 25f, 500f, 2000f);

            //GUILayout.BeginHorizontal();
            
            GUILayout.BeginArea(rect);
            //CustomGUI.MyGUI.DrawColorRect(new Rect(0f, 0f, rightRect.width, rightRect.height), Color.red);
            if (editorLeft != null)
            {
                editorLeft.OnInspectorGUI();
            }
            GUILayout.EndArea();

            //scrollPositionRight = GUILayout.BeginScrollView(scrollPositionRight);
            //GUILayout.BeginArea(rightRect);
            //CustomGUI.MyGUI.DrawColorRect(new Rect(0f, 0f, rightRect.width, rightRect.height), Color.red);

            /*if (editorRight != null)
            {
                editorRight.DrawDefaultInspector();
                //editorRight.OnInspectorGUI();
            }*/
            //GUILayout.EndArea();
            //GUILayout.EndScrollView();
            //GUILayout.EndHorizontal();
        }

        private void CreateEditor(int index, UnitType type)
        {
            DestroyEditor();
            UnitData data = null;
            Unit unit = null;

            switch (type)
            {
                case UnitType.Undefined:
                default:
                    return;


                case UnitType.Building:
                    unit = dataHandler.UnitEditorData.buildings[index].GetComponent<Unit>() as Building;
                    data = unit.UnitData as BuildingData;
                    break;


                case UnitType.Character:
                    unit = dataHandler.UnitEditorData.characters[index].GetComponent<Unit>() as Character;
                    data = unit.UnitData as CharacterData;
                    break;

            }
          
            if (data != null)
            {
                editorLeft = Editor.CreateEditor(data);
                editorRight = Editor.CreateEditor(unit);
                EditorUtility.SetDirty(data);
            }
            else
            {
                Debug.LogError("Data Error - could not create Editor");
            }
        }

        private void DestroyEditor()
        {
            if (editorLeft != null) DestroyImmediate(editorLeft);
            //if (editorRight != null) DestroyImmediate(editorRight);
        }

        private void DestroyEditor(int index)
        {
            if (editorLeft != null) DestroyImmediate(editorLeft);
            //if (editorRight != null) DestroyImmediate(editorRight);
        }
    }
}

