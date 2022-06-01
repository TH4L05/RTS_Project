/// <author> Thomas Krahl </author>

using UnityEditor;
using UnityEngine;

using UnitEditor.Data;

namespace UnitEditor.Window
{
    public class AddComponentWindow : EditorWindow
    {
        #region Fields

        private static AddComponentWindow window;
        public static bool IsOpen;

        #endregion

        #region UnityFunctions

        private void OnEnable()
        {
            bool setupSuccess = Initialize();

            if (!setupSuccess)
            {
                Close();
                Debug.LogError("AddComponentWindow Initialize = Failed");
            }

            IsOpen = true;
        }

        private void OnDestroy()
        {
        }

        private void OnGUI()
        {           
        }

        #endregion

        #region Initialize

        private bool Initialize()
        {         
            return true;
        }

        #endregion

        #region Destroy
        #endregion

        #region Window

        public static void OpenWindow()
        {
            window = GetWindow<AddComponentWindow>("Add Component");
        }

        public static void CloseWindow()
        {
            if (IsOpen && window != null) window.Close();
        }

        #endregion
    }
}

