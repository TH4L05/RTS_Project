
using System;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace UnitEditor.Toolbar
{
    public sealed class UnitEditorToolbar : Object
    {
        public static Action<int> ToolbarIndexChanged;

        private UnitEditorWindow editorwindow;
        private int toolbarIndex;
        private int lastToolbarIndex;

        [SerializeField] private string[] toolBarTexts = new string[] { "Buildings", "Characters" };

        public UnitEditorToolbar(UnitEditorWindow window)
        {
            editorwindow = window;            
        }

        public void OnGUI()
        {          
            var rect = new Rect(5f, 5f, 400f, 25f);
            toolbarIndex = GUI.Toolbar(rect, toolbarIndex, toolBarTexts);

            if (lastToolbarIndex != toolbarIndex)
            {
                ToolbarIndexChanged?.Invoke(toolbarIndex);
            }

            lastToolbarIndex = toolbarIndex;          
        }

        public void Destroy()
        {         
        }
    }
}

