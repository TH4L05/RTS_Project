
using UnityEngine;
using UnityEditor;

using UnitEditor.Toolbar;
using UnitEditor.Data;
using UnitEditor.UnitEditorGUI;
using UnitEditor.CustomGUI;

namespace UnitEditor
{
    public class UnitEditorWindow : EditorWindow
    {
        #region Events



        #endregion

        #region SerializedFields



        #endregion

        #region PrivateFields

        private static UnitEditorWindow window;
        private UnitEditorToolbar toolbar;
        private ButtonList buttonlist;
        private PropertiesArea propertiesArea;
        private bool setupDone = false;

        private Vector2 scrollPosition1 = Vector2.zero;
        private Vector2 scrollPosition2 = Vector2.zero;

        #endregion

        #region PublicFields

        public DataHandler dataHandler;


        #endregion

        #region UnityFunctions

        private void OnEnable()
        {
            Initialize();                  
        }

        private void OnGUI()
        {
            if (!setupDone) return;
            toolbar.OnGUI();

            Rect leftRect = new Rect(20f, 50f, 300f, window.position.size.y - 50f);
            Rect rightRect = new Rect(leftRect.position.x + leftRect.size.x + 5f, 50f, window.position.width - leftRect.width - leftRect.x - 10f, window.position.size.y - 50f);

            Rect leftScrollRect = new Rect(0f, 0f, leftRect.width - 20f, 999f);
            Rect rightScrollRect = new Rect(0f, 0f, rightRect.width - 20f, 2000f);

            Color leftColor = new Color(0.45f, 0.45f, 0.45f);
            Color rightColor = new Color(0.35f, 0.35f, 0.35f);

            GUILayout.BeginArea(leftRect);
            scrollPosition1 = EditorGUILayout.BeginScrollView(scrollPosition1, GUILayout.Height(window.position.height), GUILayout.Width(300f));
            MyGUI.DrawColorRect(leftScrollRect, leftColor);
            buttonlist.OnGUI();
            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();

            GUILayout.BeginArea(rightRect);
            scrollPosition2 = EditorGUILayout.BeginScrollView(scrollPosition2, GUILayout.Height(window.position.height), GUILayout.Width(window.position.width - leftRect.width - 5f));
            MyGUI.DrawColorRect(rightScrollRect, rightColor);
            propertiesArea.OnGUI();
            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private void OnDestroy()
        {
            if (toolbar != null)
            {
                toolbar.Destroy();
                DestroyImmediate(toolbar);
            }
            if (buttonlist != null)
            {
                buttonlist.Destroy();
                DestroyImmediate(buttonlist);
            }

            if (propertiesArea != null)
            {
                propertiesArea.Destroy();
                DestroyImmediate(propertiesArea);
            }
        }

        #endregion

        #region Initialize

        [MenuItem("UnitEditor/UnitEditorWindow")]
        public static void ShowWindow()
        {
            window = GetWindow<UnitEditorWindow>("UnitEditor");
        }

        private void Initialize()
        {
            dataHandler = new DataHandler();

            bool setup = dataHandler.Setup();

            if (!setup)
            {
                window.Close();
                Debug.LogError("Setup Failed");
            }

            toolbar = new UnitEditorToolbar(this);
            buttonlist = new ButtonList(this, dataHandler);
            propertiesArea = new PropertiesArea(this, dataHandler);
            setupDone = true;
        }

        #endregion

        #region Destroy
        #endregion  
    }
}

