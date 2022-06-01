/// <author> Thomas Krahl </author>

using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

using UnitEditor.UI.Toolbar;
using UnitEditor.UI.ButttonList;
using UnitEditor.Data;

namespace UnitEditor.UI.PropertiesArea
{
    public sealed class PropertiesArea
    {
        #region Fields

        private Editor unitDataEditor;
        private int messageCode;

        #endregion

        public PropertiesArea()
        {
            Initialize();
        }

        #region Initialize

        public void Initialize()
        {
            ButtonList.OnButtonPressed += CreateEditor;
            ButtonList.OnUnitGetsDeleted += DestroyEditor;
            ButtonList.SetMessage += SetMessageCode;
            UnitEditorToolbar.ToolbarIndexChanged += DestroyEditor;
        }

        #endregion

        #region Destroy

        public void Destroy()
        {
            ButtonList.OnButtonPressed -= CreateEditor;
            ButtonList.OnUnitGetsDeleted -= DestroyEditor;
            ButtonList.SetMessage -= SetMessageCode;
            UnitEditorToolbar.ToolbarIndexChanged -= DestroyEditor;
            DestroyEditor(0);
        }

        /// <summary>
        /// Immediate Destroys an Editor
        /// </summary>
        /// <param name="index">can be set 0</param>
        private void DestroyEditor(int index)
        {
            if (unitDataEditor != null)
            {               
                Object.DestroyImmediate(unitDataEditor);
            }
        }

        #endregion

        #region GUI

        public void OnGUI()
        {          
            if (messageCode != 0)
            {              
                ShowHelpBox();
            }
            else
            {
                if (unitDataEditor != null) unitDataEditor.OnInspectorGUI();
            }         
        }

        /// <summary>
        /// Show a Helpbox with a message
        /// </summary>
        private void ShowHelpBox()
        {
            DestroyEditor(0);
            string message = string.Empty;
            MessageType messageType = MessageType.None;

            switch (messageCode)
            {
                case 1:
                    message = "No Units created - Press \"New Unit\" Button to create one.";
                    messageType = MessageType.Info;
                    break;

                case 10:
                    message = "Error - could not create Editor - object = null";
                    messageType = MessageType.Error;
                    Debug.LogError(message);
                    break;

                case 20:
                    message = "Data Error - could not create Editor - data = null";
                    messageType = MessageType.Error;
                    Debug.LogError(message);
                    break;
            }

            EditorGUILayout.Space(10f);
            EditorGUILayout.HelpBox(message, messageType);
        }

        private void SetMessageCode(int indx)
        {
            messageCode = indx;
        }

        #endregion

        #region Editor

        /// <summary>
        /// Creates an Editor based on UnitType
        /// </summary>
        /// <param name="go">active obj</param>
        private void CreateEditor(GameObject go)
        {
            DestroyEditor(0);
            SetMessageCode(0);

            DataHandler.Instance.SetActiveObj(go);

            if (go == null)
            {
                SetMessageCode(10);
                return;
            }
           
            UnitData data = GetUnitData(go);
       
            if (data == null)
            {
                SetMessageCode(20);
                return;
            }

            unitDataEditor = Editor.CreateEditor(data);           
            EditorUtility.SetDirty(data);
        }

        /// <summary>
        /// Get UnitData of an Obj based on UnitType
        /// </summary>
        /// <param name="obj">active obj</param>
        /// <returns>UnitData based on UnitType</returns>
        private UnitData GetUnitData(GameObject obj)
        {
            var type = obj.GetComponent<Unit>().UnitData.Type;
            Unit unit = null;
            UnitData data = null;

            switch (type)
            {
                default:
                    return data;


                case UnitType.Building:
                    unit = obj.GetComponent<Unit>() as Building;
                    data = unit.UnitData as BuildingData;
                    return data;


                case UnitType.Character:
                    unit = obj.GetComponent<Unit>() as Character;
                    data = unit.UnitData as CharacterData;
                    return data;
            }
        }

        #endregion
    }
}

