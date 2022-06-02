/// <author> Thomas Krahl </author>

using System;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

using UnitEditor.Window;
using UnitEditor.UI.Toolbar;
using UnitEditor.Data;

namespace UnitEditor.UI.ButttonList
{
    public class ButtonList
    {
        #region Events

        public static Action<GameObject> OnButtonPressed;
        public Action ResetScrollPosition;
        public static Action<int> SetMessage;

        #endregion

        #region Fields

        private string[] unitNames;
        private UnitType type;
        private GUISkin mySkin;

        #endregion

        public ButtonList()
        {           
            Intitialize();
        }

        #region Intitialize
               
        public void Intitialize()
        {
            unitNames = new string[1];
            UnitEditorToolbar.ToolbarIndexChanged += LoadList;
            NewUnitWindow.NewUnitCreated += ReloadList;
            ConfirmationWindow.UnitDeleted += OnDeleteUnit;

            LoadList(0);
            mySkin = DataHandler.Instance.MySkin;
        }

        #endregion

        #region Destroy

        public void Destroy()
        {
            UnitEditorToolbar.ToolbarIndexChanged -= LoadList;
            NewUnitWindow.NewUnitCreated -= ReloadList;
            ConfirmationWindow.UnitDeleted -= OnDeleteUnit;
        }

        #endregion

        #region GUI

        public void OnGUI()
        {
            if (unitNames.Length == 0)
            {
                SetMessage?.Invoke(1);
            }
            else
            {
                GUILayout.BeginVertical();
                for (int i = 0; i < unitNames.Length; i++)
                {
                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button(unitNames[i], mySkin.customStyles[10]))
                    {                    
                        GameObject obj = GetObjFromDataHandler(type, i);
                        OnButtonPressed?.Invoke(obj);
                        ResetScrollPosition?.Invoke();
                    }

                    if (GUILayout.Button("X", GUILayout.Width(20f), GUILayout.Height(25f)))
                    {
                        ConfirmationWindow.OpenWindow(type, i);                       
                    }

                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }                      
        }

        #endregion

        #region List

        private void LoadList(int index)
        {            
            type = (UnitType)index;
            LoadDataNames(type);
            SetMessage?.Invoke(0);
        }

        private void ReloadList()
        {
            LoadDataNames(type);
            GameObject obj = GetObjFromDataHandler(type, 0);
            OnButtonPressed?.Invoke(obj);
            UnitEditorWindow.NeedRepaint();
        }

        private void LoadDataNames(UnitType type)
        {
            var  activeUnitList = DataHandler.Instance.GetList(type);
            if (activeUnitList.Count == 0)
            {
                unitNames = new string[0];
                return;
            }
            else
            {
                unitNames = new string[activeUnitList.Count];
            }

            int index = 0;
            foreach (var unit in activeUnitList)
            {
                unitNames[index] = unit.GetComponent<Unit>().name;
                index++;
            }
        }

        #endregion

        private GameObject GetObjFromDataHandler(UnitType type, int index)
        {
            return DataHandler.Instance.GetObjectFromList(type, index);            
        }

        private void OnDeleteUnit(int indx)
        {
            ReloadList();
        }
    }
}

