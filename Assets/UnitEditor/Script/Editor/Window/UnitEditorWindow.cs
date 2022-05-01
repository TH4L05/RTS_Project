
using UnityEngine;
using UnityEditor;

using UnitEditor.Toolbar;
using UnitEditor.Data;
using UnitEditor.List;
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
        private bool setupDone = false;

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

            GUILayout.BeginHorizontal();
            buttonlist.OnGUI();
            MyGUI.DrawLine(new Rect(245f, 50f, 2f, window.position.height - 15f), Color.gray);          
            GUILayout.EndHorizontal();           
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
            setupDone = true;
        }

        #endregion

        #region Destroy
        #endregion  
    }
}

