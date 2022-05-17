/// <author> Thomas Krahl </author>

using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

using UnitEditor.Toolbar;
using UnitEditor.Data;


namespace UnitEditor.UI
{
    public sealed class PropertiesArea : Object
    {
        #region Fields

        private UnitEditorWindow editorWindow;
        private DataHandler dataHandler;
        private Editor editorUnitData;
        private Editor editorUnitTypeData;
        private int messageCode;
        private Vector2 srollPosition = Vector2.zero;

        private static GameObject obj;
        private int index;

        #endregion

        public PropertiesArea(UnitEditorWindow window, DataHandler dataHandler)
        {
            this.editorWindow = window;
            this.dataHandler = dataHandler;
            Initialize();
        }

        #region Initialize

        public void Initialize()
        {
            ButtonList.OnButtonPressed += CreateEditor;
            ButtonList.UnitDeletion += OnUnitDeletion;
            UnitEditorToolbar.ToolbarIndexChanged += DestroyEditor;
        }

        #endregion

       
        public void Destroy()
        {
            ButtonList.OnButtonPressed -= CreateEditor;
            ButtonList.UnitDeletion -= OnUnitDeletion;
            UnitEditorToolbar.ToolbarIndexChanged -= DestroyEditor;
            DestroyEditor();
        }

        private void DestroyEditor()
        {
            if (editorUnitData != null) DestroyImmediate(editorUnitData);
            if (editorUnitTypeData != null) DestroyImmediate(editorUnitTypeData);
        }

        private void DestroyEditor(int index)
        {
            if (editorUnitData != null) DestroyImmediate(editorUnitData);
            if (editorUnitTypeData != null) DestroyImmediate(editorUnitTypeData);
        }

        private void OnUnitDeletion(int index, UnitType type)
        {
           var deletedObject = dataHandler.GetObjectFromList(type, index);

            if (deletedObject == obj)
            {
                CreateEditor(0, type);
            }
        }
       
        #region UI

        public void OnGUI()
        {          
            if (messageCode > 0)
            {
                ShowHelpBox();
            }
            else
            {
                if (editorUnitData != null) editorUnitData.OnInspectorGUI();
            }         
        }

        private void ShowHelpBox()
        {
            DestroyEditor();
            string message = string.Empty;
            MessageType messageType = MessageType.None;

            switch (messageCode)
            {
                case 1:
                    message = "No Units created - Press \"New Unit\" Button to create one.";
                    messageType = MessageType.Info;
                    break;

                case 10:
                    message = "Error - NO object to load - object = null";
                    messageType = MessageType.Error;
                    Debug.LogError(message);
                    break;

                case 20:
                    message = "Data Error - could not create Editor - data = null";
                    messageType = MessageType.Error;
                    Debug.LogError(message);
                    break;

                case 30:
                    break;
            }

            EditorGUILayout.HelpBox(message, messageType);
        }

        #endregion

        #region Editor

        /// <summary>
        /// Creates a new Editor based on UnitType
        /// </summary>
        /// <param name="index">Index from buttonList</param>
        /// <param name="type">unitType</param>
        private void CreateEditor(int index, UnitType type)
        {
            DestroyEditor();

            if (type == UnitType.Undefined) return;
            if (index == -1)
            {
                messageCode = 1;
                return;
            }

            messageCode = 0;

            obj = dataHandler.GetObjectFromList(type, index);
            if (obj == null)
            {
                messageCode = 10;                           
                return;
            }

            (Unit, UnitData) u = GetUnitAndData(obj, type);
            Unit unit = u.Item1;
            UnitData data = u.Item2;
       
            if (data == null)
            {
                messageCode = 20;
                return;
            }

            editorUnitData = Editor.CreateEditor(data);           
            EditorUtility.SetDirty(data);
        }

        private (Unit, UnitData) GetUnitAndData(GameObject obj,UnitType type)
        {
            Unit unit = null;
            UnitData data = null;

            switch (type)
            {
                case UnitType.Undefined:
                default:
                    return (unit, data);


                case UnitType.Building:
                    unit = obj.GetComponent<Unit>() as Building;
                    data = unit.UnitData as BuildingData;
                    return (unit, data);


                case UnitType.Character:
                    unit = obj.GetComponent<Unit>() as Character;
                    data = unit.UnitData as CharacterData;
                    return (unit, data);
            }
        }

        public static GameObject GetObj()
        {
            return obj;
        }

        #endregion

    }
}

