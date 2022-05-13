using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

            EditorGUILayout.EndScrollView();
        }

        private static void Intialize()
        {
            if (obj == null) return;

            var components = obj.GetComponents(typeof(Component));
            //Debug.Log(components.Length);

            var editor = Editor.CreateEditor(obj);
            editors.Add(editor);



            foreach (var component in components)
            {
                //Debug.Log(component.name);
                var type = component.GetType();
                componentList.Add(component);
                //Debug.Log(type.FullName);

                if (type == typeof(Transform)) continue;

                editor = Editor.CreateEditor(component, type.ReflectedType);
                editors.Add(editor);
            }

            foldouts = new bool[editors.Count];
            Debug.Log(foldouts.Length);
        }

        public static void SetObject(GameObject go)
        {
            obj = go;
            //Debug.Log(obj.gameObject.name);
            Intialize();
        }

        public static void OpenWindow()
        {
            window = GetWindow<ComponentsWindow>("Edit Components");
    
            //window.maxSize = new Vector2(400f, 200f);
            window.minSize = new Vector2(400f, 200f);
            window.position = new Rect(Screen.width / 2 - 200f, Screen.height / 2 - 200f, 400f, 200f);
        }

        public static void CloseWindow()
        {
            if(IsOpen) window.Close();
        }
    }
}
