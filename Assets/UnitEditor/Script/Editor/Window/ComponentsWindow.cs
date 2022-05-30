/// <author> Thomas Krahl </author>

using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

using UnitEditor.Data;

namespace UnitEditor.Window
{
    public class ComponentsWindow : EditorWindow
    {
        #region Fields

        private static ComponentsWindow window;
        private GameObject obj;
        private List<Component> componentList = new List<Component>();
        private List<Editor> editors = new List<Editor>();
        private static bool[] foldouts;
        private static Vector2 scrollPosition = Vector2.zero;

        public static bool IsOpen;

        #endregion

        #region UnityFunctions

        private void OnEnable()
        {
            bool setupSuccess = Initialize();

            if (!setupSuccess)
            {
                Close();
                Debug.LogError("LoadFromFileWIndow Setup = Failed");
            }

            IsOpen = true;
        }

        private void OnDestroy()
        {
            IsOpen = false;
            

            if (obj == null) return;
            if(editors.Count == 0) return;

            foreach (var editor in editors)
            {
                DestroyImmediate(editor);
            }

            obj = null;
            componentList.Clear();
            editors.Clear();
        }

        private void OnGUI()
        {
            if (obj == null) return;

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, false);
            GUILayout.Space(10f);

            GUILayout.BeginVertical(GUILayout.Width(250f));
            int idx = 0;
            foreach (var editor in editors)
            {
                GUILayout.BeginVertical(GUILayout.Width(window.position.size.x - 25f));
                foldouts[idx] = EditorGUILayout.Foldout(foldouts[idx], componentList[idx].GetType().Name);

                if (foldouts[idx])
                {
                    editor.DrawHeader();
                    editor.OnInspectorGUI();
                    EditorGUILayout.Space(10f);
                }
                
                idx++;
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();

            EditorGUILayout.Space(10f);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Button("", GUILayout.Height(0.1f), GUILayout.Width(window.position.size.x / 2 - 50f));
            if (GUILayout.Button("--", GUILayout.Height(40f), GUILayout.Width(100f)))
            {
                
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndScrollView();
        }

        #endregion

        #region Initialize

        private bool Initialize()
        {
            obj = DataHandler.Instance.ActiveObj;
            if (obj == null) return false;

            var components = obj.GetComponents(typeof(Component));
            var editor = Editor.CreateEditor(obj);
            editors.Add(editor);

            foreach (var component in components)
            {
                var type = component.GetType();
                componentList.Add(component);

                if (type == typeof(Transform)) continue;
                editor = Editor.CreateEditor(component, type.ReflectedType);
                editors.Add(editor);
            }

            foldouts = new bool[editors.Count];

            return true;
        }

        #endregion

        #region Destroy
        #endregion

        #region Window

        public static void OpenWindow()
        {
            window = GetWindow<ComponentsWindow>("Edit Components");
            window.minSize = new Vector2(200f, 400f);

            Rect mainWindowRect = UnitEditorWindow.GetWindowRect();
            window.position = new Rect(mainWindowRect.x + (mainWindowRect.width / 2) - 200f, mainWindowRect.y + (mainWindowRect.height / 2) - 300f, 400f, 600f);
        }

        public static void CloseWindow()
        {
            if(IsOpen && window != null) window.Close();
        }

        #endregion
    }
}
