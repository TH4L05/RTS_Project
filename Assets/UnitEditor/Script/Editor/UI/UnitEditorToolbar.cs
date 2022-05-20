/// <author> Thomas Krahl </author>

using System;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

using UnitEditor.Window;

namespace UnitEditor.UI.Toolbar
{
    public sealed class UnitEditorToolbar
    {
        #region Events

        public static Action<int> ToolbarIndexChanged;
        public Action ResetScrollPosition;

        #endregion

        #region Fields

        [SerializeField] private string[] toolBarTexts = new string[] { "Buildings", "Characters" };
        private UnitEditorWindow editorwindow;
        private int toolbarIndex;
        private int lastToolbarIndex;

        public static int ToolbarIndex;

        #endregion

        public UnitEditorToolbar()
        {
            Initialize();
        }

        #region Initialize

        private void Initialize()
        {
        }

        #endregion

        #region Destroy

        public void Destroy()
        {
        }

        #endregion

        #region GUI

        public void OnGUI()
        {
            if (NewUnitWindow.IsOpen) return;
            var rect = new Rect(5f, 5f, 400f, 25f);
            toolbarIndex = GUI.Toolbar(rect, toolbarIndex, toolBarTexts);

            if (lastToolbarIndex != toolbarIndex)
            {
                ToolbarIndexChanged?.Invoke(toolbarIndex);
                ResetScrollPosition?.Invoke();
            }

            lastToolbarIndex = toolbarIndex;
            ToolbarIndex = lastToolbarIndex;
        }

        #endregion

        public int GetIndex()
        {
            return toolbarIndex;
        }
    }
}

