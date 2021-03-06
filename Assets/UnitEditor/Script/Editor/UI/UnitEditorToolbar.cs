/// <author> Thomas Krahl </author>

using System;
using UnityEngine;

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

        [SerializeField] private string[] toolBarStrings;
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
            toolBarStrings = Enum.GetNames(typeof(UnitType));
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

            int amount = Enum.GetValues(typeof(UnitType)).Length;

            var rect = new Rect(5f, 5f, 200f * amount, 25f);
            toolbarIndex = GUI.Toolbar(rect, toolbarIndex, toolBarStrings);

            if (lastToolbarIndex != toolbarIndex)
            {
                ToolbarIndexChanged?.Invoke(toolbarIndex);
                ResetScrollPosition?.Invoke();
            }

            lastToolbarIndex = toolbarIndex;
            ToolbarIndex = lastToolbarIndex;
        }

        #endregion
    }
}

