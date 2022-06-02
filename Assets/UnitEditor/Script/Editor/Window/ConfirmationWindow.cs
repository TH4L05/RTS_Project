/// <author> Thomas Krahl </author>

using System;

using UnityEngine;
using UnityEditor;

using UnitEditor.Data;

namespace UnitEditor.Window
{
    public class ConfirmationWindow : EditorWindow
    {
        #region Events

        public static Action<int> UnitDeleted;

        #endregion


        #region Fields

        private static ConfirmationWindow window;
        public static bool IsOpen;

        private string unitName;
        private static int index;
        private static UnitType unitType;
        private GUIStyle label;

        #endregion

        #region UnityFunctions

        private void OnEnable()
        {
            bool setupSuccess = Initialize();

            if (!setupSuccess)
            {
                Close();
                Debug.LogError("ConfirmationWindow Initialize = Failed");
            }

            IsOpen = true;
        }

        private void OnDestroy()
        {
            IsOpen = false;
        }

        private void OnGUI()
        {
            GUI();
        }

        #endregion

        #region GUI

        private void GUI()
        {
            EditorGUILayout.Space(5f);
            EditorGUILayout.BeginHorizontal(GUILayout.Width(290f));

            EditorGUILayout.Space(10f);
            EditorGUILayout.BeginVertical(GUILayout.Height(90f));

            //EditorGUILayout.HelpBox("Do you really want to delete this unit ?", MessageType.Warning);
            EditorGUILayout.LabelField("Do you really want to delete this unit ?", label);

            EditorGUILayout.Space(10f);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Cancel", GUILayout.Width(100f), GUILayout.Height(35f)))
            {
                Close();
            }
            if (GUILayout.Button("Delete", GUILayout.Width(100f), GUILayout.Height(35f)))
            {
                DeleteUnit();
                Close();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        #endregion

        #region Initialize

        private bool Initialize()
        {
            //TODO: add to skin file
            label = new GUIStyle();
            label.fontStyle = FontStyle.Bold;
            label.normal.textColor = new Color(0.85f, 0f, 0f, 0.85f);
            label.fontSize = 14;

            return true;
        }

        #endregion

        #region Destroy
        #endregion

        #region Window

        public static void OpenWindow(UnitType type, int indx)
        {
            window = GetWindow<ConfirmationWindow>("DeleteUnit");
            window.minSize = new Vector2(300, 100);
            window.maxSize = new Vector2(300, 100);

            Rect mainWindow = UnitEditorWindow.GetWindowRect();
            window.position = new Rect(mainWindow.position.x + (mainWindow.size.x / 2) - (window.position.size.x / 2), 
                                       mainWindow.position.y + (mainWindow.size.y / 2) - (window.position.size.y / 2), 
                                       window.position.width, 
                                       window.position.height
                                       );

            unitType = type;
            index = indx;
        }

        public static void CloseWindow()
        {
            if (IsOpen && window != null) window.Close();
        }

        #endregion

        private void DeleteUnit()
        {
            bool success = DataHandler.Instance.DeleteUnit(unitType, index);
            if(success) UnitDeleted?.Invoke(0);
        }
    }
}

