/// <author> Thomas Krahl </author>

using System;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

using UnitEditor.Window;
using UnitEditor.UI.Toolbar;
using UnitEditor.Data;

namespace UnitEditor.UI.ButttonList
{
    public class ButtonList
    {
        #region Events

        public static Action<int, UnitType> OnButtonPressed;
        public static Action<int, UnitType> UnitDeletion;
        public Action ResetScrollPosition;

        #endregion

        #region Fields

        private UnitEditorWindow editorwindow;
        private string[] unitNames;
        private Editor editor;
        private int index;
        private UnitType type;
        private GUISkin mySkin;

        #endregion

        public ButtonList(UnitEditorWindow window)
        {
            editorwindow = window;              
            Intitialize();
        }

        #region Intitialize
               
        public void Intitialize()
        {
            unitNames = new string[1];
            UnitEditorToolbar.ToolbarIndexChanged += LoadList;
            NewUnitWindow.NewUnitCreated += ReloadList;

            LoadList(0);
            var path = DataHandler.Instance.EditorDataPath;
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
            UnitEditorToolbar.ToolbarIndexChanged -= LoadList;
            NewUnitWindow.NewUnitCreated -= ReloadList;
            if(editor != null) Object.DestroyImmediate(editor);
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
                        DeleteUnitButtonClicked(i);
                    }

                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }                      
        }

        private void DeleteUnitButtonClicked(int indx)
        {
            if (type == UnitType.Undefined) return;

            UnitDeletion?.Invoke(indx, type);
            DataHandler.Instance.DeleteUnit(type, indx);

            this.index = 0;
            ReloadList();
        }

        private void LoadList(int index)
        {            
            type = (UnitType)index;

            if (type == UnitType.Undefined) return;
            LoadDataNames(type);
        }

        private void ReloadList()
        {
            if (editor != null) Object.DestroyImmediate(editor);
            LoadDataNames(type);
            OnButtonPressed?.Invoke(index, type);
            UnitEditorWindow.NeedRepaint();
        }

        private void LoadDataNames(UnitType type)
        {
            if (type == UnitType.Undefined) return;

            List<GameObject> units = new List<GameObject>();
            units = DataHandler.Instance.GetList(type);
            if(units.Count == 0) return;

            unitNames = new string[units.Count];

            int index = 0;
            foreach (var unit in units)
            {
                unitNames[index] = unit.GetComponent<Unit>().name;
                index++;
            }

        }

        #endregion

    }
}

