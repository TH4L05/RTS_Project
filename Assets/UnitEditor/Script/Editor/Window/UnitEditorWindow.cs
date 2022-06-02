/// <author> Thomas Krahl </author>

using UnityEngine;
using UnityEditor;

using UnitEditor.Data;
using UnitEditor.UI.Toolbar;
using UnitEditor.UI.ButttonList;
using UnitEditor.UI.PropertiesArea;
using UnitEditor.UI.Custom;

namespace UnitEditor.Window
{
    public class UnitEditorWindow : EditorWindow
    {
        #region Fields

        private static UnitEditorWindow window;
        private UnitEditorToolbar toolbar;
        private ButtonList buttonList;
        private PropertiesArea propertiesArea;

        private bool setupDone = false;
        private Vector2 scrollPosition1 = Vector2.zero;
        private Vector2 scrollPosition2 = Vector2.zero;

        private static bool needRepaint;

        #endregion
     
        #region UnityFunctions

        private void OnEnable()
        {
            Initialize();
        }

        private void OnGUI()
        {
            if (!setupDone) return;

            if (needRepaint)
            {
                Repaint();
                needRepaint = false;
            }

            toolbar.OnGUI();
            
            EditorGUILayout.Space(50f);
            EditorGUILayout.BeginHorizontal();

                EditorGUILayout.BeginVertical(GUILayout.Width(250f));
                    LeftArea();
                    LeftAreaBottom();
                EditorGUILayout.EndVertical();

                EditorGUILayout.Space(5f);

                EditorGUILayout.BeginVertical(GUILayout.Width(window.position.size.x - 250f));
                    RightArea();
                EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            Event e = Event.current;
            switch (e.type)
            {
                case EventType.KeyDown:
                    if (e.keyCode == KeyCode.Escape)
                    {
                        window.Close();                     
                    }
                    break;

                default:
                    break;
            }

            //Repaint();          
        }

        private void OnDestroy()
        {
            Destroy();
        }

        #endregion

        #region Initialize

        private void Initialize()
        {
            bool setupSuccess = DataHandler.Instance.Setup();

            if (!setupSuccess)
            {
                Close();
                Debug.LogError("Setup Failed");
                return;
            }

            toolbar = new UnitEditorToolbar();
            toolbar.ResetScrollPosition += ResetAllScrollPositions;

            buttonList = new ButtonList();
            buttonList.ResetScrollPosition += ResetScrollPosition;

            propertiesArea = new PropertiesArea();

            setupDone = true;
        }

        #endregion

        #region Destroy

        private void Destroy()
        {
            if (toolbar != null)
            {
                toolbar.ResetScrollPosition -= ResetAllScrollPositions;
                toolbar.Destroy();
                toolbar = null;
            }
            if (buttonList != null)
            {
                buttonList.ResetScrollPosition -= ResetScrollPosition;
                buttonList.Destroy(); 
                buttonList = null;
            }

            if (propertiesArea != null)
            {
                propertiesArea.Destroy();
                propertiesArea = null;
            }

            NewUnitWindow.CloseWindow();
            ComponentsWindow.CloseWindow();
            SettingsWindow.CloseWindow();
            LoadFromFileWIndow.CloseWindow();
        }

        #endregion

        #region Areas

        private void LeftArea()
        {        
            scrollPosition1 = EditorGUILayout.BeginScrollView(scrollPosition1, false, false, GUILayout.Width(250f));
                EditorGUILayout.Space(10f);
                GUILayout.BeginHorizontal();

                    EditorGUILayout.Space(10f);
                    Rect colorRect = new Rect(5f, 0f, 250f, window.position.size.y);
                    MyGUI.DrawColorRect(colorRect, new Color(0.35f, 0.35f, 0.35f));
                    buttonList.OnGUI();

                GUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();                    
        }

        private void LeftAreaBottom()
        {
            var newUnitGUIContent = new GUIContent("New Unit", "Create a new Unit");
            var settingsGUIContent = new GUIContent("Settings", "Edit the Unit Editor Settings");
            var manualGUIContent = new GUIContent("?", "Open the Manual");


            EditorGUILayout.Space(10f);
            EditorGUILayout.BeginHorizontal();

                GUILayout.Button("", GUILayout.Width(11f), GUILayout.Height(0.1f));
                if (GUILayout.Button(newUnitGUIContent, GUILayout.Width(90f), GUILayout.Height(35f)))
                {
                    NewUnitWindow.OpenWindow();
                }

                if (GUILayout.Button(settingsGUIContent, GUILayout.Width(90f), GUILayout.Height(35f)))
                {
                    SettingsWindow.OpenWindow();
                }

                if (GUILayout.Button(manualGUIContent, GUILayout.Width(35f), GUILayout.Height(35f)))
                {
                    string prefix = "File:///";
                    string dataPath = Application.dataPath;
                    string projectPath = dataPath.Replace("Assets", "");
                    string editorDataPath = DataHandler.Instance.EditorDataPath;

                    string filepath = prefix + projectPath + editorDataPath + "/Manual/UnitEditorManual.pdf";
                    Debug.Log(filepath);
                    Application.OpenURL(filepath);
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(15f);
        }

        private void RightArea()
        {
            Rect areaRect = new Rect(255f, 50f, window.position.size.x - 255f, window.position.size.y - 60f);
            Rect viewRect = new Rect(areaRect.x, areaRect.y, 1000f, 2500f);

            MyGUI.DrawColorRect(new Rect(viewRect.x, viewRect.y, areaRect.width, viewRect.height), new Color(0.35f, 0.35f, 0.35f));

            scrollPosition2 = GUI.BeginScrollView(areaRect, scrollPosition2, viewRect);
                GUILayout.BeginArea(viewRect);
                    propertiesArea.OnGUI();
                GUILayout.EndArea();
            GUI.EndScrollView();
        }

        #endregion

        #region Window

        [MenuItem("UnitEditor/UnitEditorWindow")]
        public static void ShowWindow()
        {
            window = GetWindow<UnitEditorWindow>("UnitEditor");
            window.minSize = new Vector2(650f, 350f);
        }

        #endregion

        public static void NeedRepaint()
        {
            needRepaint = true;
        }

        public static Rect GetWindowRect()
        {
            return window.position;
        }

        private void ResetAllScrollPositions()
        {
            scrollPosition1 = Vector2.zero;
            scrollPosition2 = Vector2.zero;
        }

        private void ResetScrollPosition()
        {
            scrollPosition2 = Vector2.zero;
        }
    }
}

