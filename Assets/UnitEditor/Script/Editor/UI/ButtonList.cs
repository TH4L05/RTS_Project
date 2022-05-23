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

        public static Action<GameObject> OnButtonPressed;
        public static Action<int> OnUnitGetsDeleted;
        public Action ResetScrollPosition;

        #endregion

        #region Fields

        private string[] unitNames;
        private int index;
        private UnitType type;
        private GUISkin mySkin;
        private List<GameObject> activeUnitList;

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

            LoadList(0);
            mySkin = DataHandler.Instance.MySkin;
        }

        #endregion

        #region Destroy

        public void Destroy()
        {
            UnitEditorToolbar.ToolbarIndexChanged -= LoadList;
            NewUnitWindow.NewUnitCreated -= ReloadList;         
        }

        #endregion

        #region GUI

        public void OnGUI()
        {
            if (unitNames.Length == 0)
            {
                OnButtonPressed?.Invoke(null);
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
                        OnButtonPressed?.Invoke(activeUnitList[index]);
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
            OnUnitGetsDeleted?.Invoke(0);
            DataHandler.Instance.DeleteUnit(type, indx);
            ReloadList();
        }

        private void LoadList(int index)
        {            
            type = (UnitType)index;
            LoadDataNames(type);
        }

        private void ReloadList()
        {
            LoadDataNames(type);
            OnButtonPressed?.Invoke(activeUnitList[0]);
            UnitEditorWindow.NeedRepaint();
        }

        private void LoadDataNames(UnitType type)
        {
            activeUnitList = DataHandler.Instance.GetList(type);
            if(activeUnitList.Count == 0) return;

            unitNames = new string[activeUnitList.Count];

            int index = 0;
            foreach (var unit in activeUnitList)
            {
                unitNames[index] = unit.GetComponent<Unit>().name;
                index++;
            }
        }

        #endregion

    }
}

