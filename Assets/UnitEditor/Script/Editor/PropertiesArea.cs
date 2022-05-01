

using UnityEditor;
using UnityEngine;
using UnitEditor.Toolbar;
using UnitEditor.Data;
using Object = UnityEngine.Object;
using System.Collections.Generic;
using UnitEditor.CustomGUI;

namespace UnitEditor.UnitEditorGUI
{
    public class PropertiesArea : Object
    {
        private UnitEditorWindow window;
        private DataHandler dataHandler;
        private Editor editor;

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
            if (editor != null)
            {
                editor.OnInspectorGUI();
            }
        }

        private void CreateEditor(int index, UnitType type)
        {
            DestroyEditor();
            UnitData data = null;

            switch (type)
            {
                case UnitType.Undefined:
                default:
                    return;


                case UnitType.Building:
                    data = dataHandler.UnitEditorData.buildings[index].GetComponent<Unit>().UnitData as BuildingData;
                    break;


                case UnitType.Character:
                    data = dataHandler.UnitEditorData.characters[index].GetComponent<Unit>().UnitData as CharacterData;
                    break;

            }
          
            if (data != null)
            {
                editor = Editor.CreateEditor(data);
                EditorUtility.SetDirty(data);
            }
            else
            {
                Debug.LogError("Data Error - could not create Editor");
            }
        }

        private void DestroyEditor()
        {
            if (editor != null) DestroyImmediate(editor);
        }

        private void DestroyEditor(int index)
        {
            if (editor != null) DestroyImmediate(editor);
        }
    }
}

