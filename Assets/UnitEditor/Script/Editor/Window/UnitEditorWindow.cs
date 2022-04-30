
using UnityEngine;
using UnityEditor;

namespace UnityEditor
{
    public class UnitEditorWindow : EditorWindow
    {
        #region Actions



        #endregion

        #region SerializedFields

        private static UnitEditorWindow window;
        private static string dataPath;

        private int toolbarIndex = 0;

        #endregion

        #region PrivateFields
        #endregion

        #region PublicFields



        #endregion

        #region UnityFunctions

        private void OnEnable()
        {
            bool setupSuccess = Setup();

            if (!setupSuccess)
            {
                Debug.LogError("Setup Failed");
            }

        }

        private void OnGUI()
        {
            DrawElements();
        }

        private void OnDestroy()
        {

        }

        #endregion

        #region Setup

        private bool Setup()
        {
            bool result = CheckEditorPath();

            if (!result)
            {
                return false;
            }
            return true;
        }

        private bool CheckEditorPath()
        {
            string[] path = AssetDatabase.FindAssets("UnitEditor");
            dataPath = AssetDatabase.GUIDToAssetPath(path[0]);

            if (string.IsNullOrEmpty(dataPath))
            {
                Debug.LogError("Could not find DataPath");
                return false;
            }
            return true;
        }

        #endregion

        #region Destroy
        #endregion

        #region Style

        private void DrawElements()
        {
            var rect = new Rect(0, 0, window.position.width, 25f);

            string[] toolBarTexts = new string[] { "TEST1", "Test2" };

            toolbarIndex =  GUI.Toolbar(rect, toolbarIndex, toolBarTexts);
            CheckToolBarIndex();
        }

        private void CheckToolBarIndex()
        {
            if (toolbarIndex == 0)
            {
                Tab1();
                window.Repaint();
            }
            else if (toolbarIndex == 1)
            {
                Tab2();
                window.Repaint();
            }
        }

        private void Tab1()
        {
            var rect = new Rect(10f, 30f, window.position.width, window.position.height);
            GUILayout.BeginArea(rect);
            EditorGUI.DrawRect(rect, Color.cyan);
            EditorGUILayout.HelpBox("TEST TAB1", MessageType.Info);
            GUILayout.EndArea();
        }

        private void Tab2()
        {
            var rect = new Rect(10f, 30f, window.position.width, window.position.height);
            GUILayout.BeginArea(rect);
            EditorGUI.DrawRect(rect, Color.red);
            EditorGUILayout.HelpBox("TEST TAB2", MessageType.Info);

            var droprect = new Rect(10f, 50, 100f, 35f);
            var content = new GUIContent("DropDownTest");

            var content1 = new GUIContent("DropDownTest1");
            var content2 = new GUIContent("DropDownTest2");
            var content3 = new GUIContent("DropDownTest3");
            var content4 = new GUIContent("DropDownTest4");

            if (EditorGUI.DropdownButton(droprect, content, FocusType.Passive))
            {
                GenericMenu dropMenu = new GenericMenu();

                
                dropMenu.AddItem(content1, true, DropMenuFunction, 1);
                dropMenu.AddItem(content2, false, DropMenuFunction,2);
                dropMenu.AddItem(content3, false, DropMenuFunction,3);
                dropMenu.AddItem(content4, false, DropMenuFunction,4);
                dropMenu.AddSeparator("");
                dropMenu.ShowAsContext();

               

            }

            GUILayout.EndArea();
        }

        void OnColorSelected(object color)
        {
            //m_Color = (Color)color;
        }


        void DropMenuFunction(object index)
        {
            Debug.Log("TEST");
        }
         
        #endregion


        [MenuItem("UnitEditor/UnitEditorWindow")]
        public static void OpenWindow()
        {
            window = GetWindow<UnitEditorWindow>("UnitEditor");
        }
    }
}

