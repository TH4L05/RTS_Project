using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddComponent;
using UnityEngine;

namespace UnitEditor
{
    public class ComponentsWindow : EditorWindow
    {
        private static ComponentsWindow window;
        private static GameObject obj;
        private static List<Component> componentList = new List<Component>();
        private static List<Editor> editors = new List<Editor>();
        private static bool[] foldouts;
        private static Vector2 scrollPosition = Vector2.zero;
        public static bool IsOpen;

        private void OnEnable()
        {
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

        private static void Intialize()
        {
            if (obj == null) return;

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
        }

        public static void SetObject(GameObject go)
        {
            obj = go;
            Intialize();
        }

        public static void OpenWindow()
        {
            window = GetWindow<ComponentsWindow>("Edit Components");
            window.minSize = new Vector2(200f, 400f);
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 400f, 600f);
        }

        public static void CloseWindow()
        {
            if(IsOpen && window != null) window.Close();
        }
    }
}
