using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnitEditor.CustomGUI
{
    public class MyGUI
    {
        public static void DrawLine(Color color, int thickness = 2, int padding = 10)
        {
            Rect rect = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            rect.height = thickness;
            rect.y += padding / 2;
            rect.x -= 2;
            rect.width += 6;
            UnityEditor.EditorGUI.DrawRect(rect, color);
        }

        public static void DrawLine(Rect rect, Color color)
        {
            UnityEditor.EditorGUI.DrawRect(rect, color);
        }

        public static void DrawColorRect(Rect rect, Color color)
        {
            UnityEditor.EditorGUI.DrawRect(rect, color);
        }
    }
}

